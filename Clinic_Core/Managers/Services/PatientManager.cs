using AutoMapper;
using Clinic_Common.Extensions;
using Clinic_Core.Helper;
using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Core.Managers.Services
{
    public class PatientManager : IPatientManager
    {
        private clinic_dbContext _dbContext;
        private IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWT _jwt;
        private readonly IConfiguration _configuration;
        private IWebHostEnvironment _host;


        public PatientManager(clinic_dbContext dbContext, IMapper mapper,
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IOptions<JWT> jwt, IConfiguration configuration,
            IWebHostEnvironment host)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            //_jwt = jwt.Value;
            _configuration = configuration;
            _jwt = Binding();
            _host = host;
        }
        
        #region Public
        public async Task<ResponseApi> SignUp(PatientRegistrationModelView PatientReg)
        {
            if (_dbContext.Users.Any(x => x.Email.Equals(PatientReg.Email, StringComparison.InvariantCultureIgnoreCase)))
            {
                var response1 = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "Email already exist",
                    Data = null
                };
                return response1;
            }

            var hashedPassword = HashPassword(PatientReg.Password);
            var patient = _dbContext.Users.Add(new ApplicationUser
            {
                FirstName = PatientReg.FirstName,
                LastName = PatientReg.LastName,
                Email = PatientReg.Email,
                PasswordHash = hashedPassword,
                UserType = "Patient"
            }).Entity;

            _dbContext.SaveChanges();
            var jwtSecurityToken = await CreateJwtToken(patient);

            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Register Successfully",
                Data = new
                {
                      Email = patient.Email,
                      FirstName = patient.FirstName,
                      LastName = patient.LastName,
                      UserType = patient.UserType,
                      IsValid = true,
                      Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                }
            };
            return response;
        }

        public ResponseApi UpdateProfilePatient(string userId, UpdatePatientProfilVM appUser)
        {
            var user = _dbContext.Users.Find(userId);
            string folder = "Uploads/PatientImages";
            folder = UploadImage(folder, appUser.ImageFile);
            var Image = folder;
            if (user == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "user not exist",
                    Data = null
                };
                return response;
            }else
            {
                user.Image = Image;
                user.PhoneNumber = appUser.PhoneNumber;
                user.Address = appUser.Address;
                user.Gender = appUser.Gender;
                _dbContext.SaveChanges();

                var userAfterUpdate = _dbContext.Users.Find(userId);
                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Updated Successfully",
                    Data = new
                    {
                        PatientName = user.FirstName + " " + user.LastName,
                        Email = user.Email,
                        DateOfBirth = userAfterUpdate.DateOfBirth,
                        BloodGroup = userAfterUpdate.BloodGroup,
                        Address = userAfterUpdate.Address,
                        Image = userAfterUpdate.Image
                    }
                };
                return response;
            }
        }

        public ResponseApi CompletePatientProfile(string userId, PatientProfileSettings profileSettings)
        {
            var user = _dbContext.Users.Find(userId);
            string folder = "Uploads/PatientImages";
            folder = UploadImage(folder, profileSettings.ImageFile);
            profileSettings.Image = folder;

            if (user == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "user not exist",
                    Data = null
                };
                return response;

            } else
             {
                user.DateOfBirth = profileSettings.DateOfBirth;
                user.BloodGroup = profileSettings.BloodGroup;
                user.Image = profileSettings.Image;
                user.Address = profileSettings.Address;
              user.UserName = profileSettings.UserName;

                _dbContext.SaveChanges();

                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Success",
                    Data = new
                    {
                        PatientName = user.FirstName +" "+user.LastName,
                        Email = user.Email,
                        DateOfBirth = profileSettings.DateOfBirth,
                        BloodGroup = profileSettings.BloodGroup,
                        Address = profileSettings.Address,
                        Image = profileSettings.Image,
                        UserName = profileSettings.UserName
            }
                };
                return response;
             }
        }


        public ResponseApi FavouritesDoctor(string userId, int doctorId)
        {
            var favDoc = new FavDoctors
            {
                ApplicationUserId = userId,
                DoctorId = doctorId
            };

            _dbContext.FavDoctors.Add(favDoc);
            _dbContext.SaveChanges();

            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Success",
                Data = favDoc
            };
            return response;
        }

        public ResponseApi GetFavouritesDoctor(string userId)
        {
            var result = _dbContext.FavDoctors.Where(x => x.ApplicationUserId == userId).Select(z => new
            {
                DoctorId = z.DoctorId,
                DoctorName = z.ApplicationUser.FirstName + " " + z.ApplicationUser.LastName,
                DoctorImage = z.ApplicationUser.Image,
                DoctorSpecialty = z.Dcotor.Specialty.SpecialtyName
            }).ToList();
            if (result.Any())
            {
                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Success",
                    Data = result
                };
                return response;
            }
            else
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "No Data",
                    Data = null
                };
                return response;
            }
            
        }


        public ResponseApi DeleteFavouriteDoctor(string userId, int doctorId)
        {
            var result = _dbContext.FavDoctors
                                   .FirstOrDefault(x => x.DoctorId == doctorId 
                                    && x.ApplicationUserId == userId);
            if(result == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "No Data",
                    Data = null
                };
                return response;
            }
            else
            {
                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Successfully Deleted!",
                    Data = null
                };
                _dbContext.FavDoctors.Remove(result);
                _dbContext.SaveChanges();
                return response;
            }
            
        }

        public ResponseApi GetMyAppointment(string userId)
        {
            var appointments = _dbContext.Appointments.Where(x => x.UserId == userId)
                                        .Select(x => new
                                        {
                                            AppointmentId = x.Id,
                                            DoctorName = x.Doctor.ApplicationUser.FirstName + " " + x.Doctor.ApplicationUser.LastName,
                                            DoctorId = x.Doctor.ApplicationUser.Id,
                                            DoctorSpecialty = x.Doctor.Specialty.SpecialtyName,
                                            DoctorImage = x.Doctor.ApplicationUser.Image,
                                            Day = x.Day,
                                            Date = x.Date.ToString("yyyy-MM-dd"),
                                            Time = x.StartTime.ToString("hh:mm tt")+"-"+ x.EndTime.ToString("hh:mm tt"),
                                            Status = x.Status

                                        }).ToList();
            if (!appointments.Any())
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "You don't have a Booked Appointment",
                    Data = null
                };
                return response;
            }
            else
            {
                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Success",
                    Data = appointments
                };
                return response;
            }
        }

        #endregion Public 

        #region private
        private static string HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            return hashedPassword;
        }

        private static bool VerifyHashPassword(string password, string HashedPasword)
        {
            return BCrypt.Net.BCrypt.Verify(password, HashedPasword);
        }

        private async Task<LoginPatientResponse> GetTokenAsync(PatientLoginModelView model)
        {
            try
            {
                LoginPatientResponse authModel = new();

                // var user = await _userManager.FindByEmailAsync(model.usernameOrEmail);
                var user = await _userManager.FindByEmailAsync(model.Email);
                var result = await _userManager.CheckPasswordAsync(user, model.Password);
                if (user is null || !result)
                {
                    authModel.Message = "Email or Password is incorrect!";
                    return authModel;
                }

                var jwtSecurityToken = await CreateJwtToken(user);
                //var rolesList = await _userManager.GetRolesAsync(user);

                authModel.IsValid = true;
                authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authModel.Email = user.Email;
                //authModel.Roles = rolesList.ToList();
                return authModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return null;
            }
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var Centerclaims = new List<Claim>
                            {
                            new System.Security.Claims.Claim("Email",user.Email),
                            };

            //var roleClaims = new List<Claim>();

            //foreach (var role in roles)
            //    roleClaims.Add(new Claim("roles", role));


            var claims = new[]
            {
                //new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                 new Claim("UserType", user.UserType),

                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }

            .Union(userClaims)
            //.Union(roleClaims)
            .Union(Centerclaims);


            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        private string UploadImage(string folder, IFormFile ImgeFile)
        {
            folder += Guid.NewGuid().ToString() + "_" + ImgeFile.FileName;
            string ImageURL = "/" + folder;
            string serverFolder = Path.Combine(_host.WebRootPath, folder);
            ImgeFile.CopyTo(new FileStream(serverFolder, FileMode.Create));
            return ImageURL;
        }

        private JWT Binding()
        {
            return new JWT
            {
                Audience = _configuration["JWTOken:Audience"],
                Issuer = _configuration["JWTOken:Issuer"],
                Key = _configuration["JWTOken:Key"],
                DurationInDays = int.TryParse(_configuration["JWTOken:DurationInDays"], out var result) ? result : 0
            };
        }
        #endregion private
    }
}
