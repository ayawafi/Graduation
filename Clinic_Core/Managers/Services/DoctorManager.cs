using AutoMapper;
using clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Clinic_Core.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using MimeKit;
using System.Net.Mail;

namespace Clinic_Core.Managers.Services
{
    public class DoctorManager : IDoctorManager
    {
        private clinic_dbContext _dbContext;
        private IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWT _jwt;
        private readonly IConfiguration _configuration;
        private IWebHostEnvironment _host;
        private readonly EmailConfiguration _emailConfig;

        public DoctorManager(clinic_dbContext dbContext, IMapper mapper,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            IOptions<JWT> jwt, IConfiguration configuration, 
            IWebHostEnvironment host, IOptions<EmailConfiguration> emailConfig)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            //_jwt = jwt.Value;
            _configuration = configuration;
            _jwt = Binding();
            _host = host;
            _emailConfig = emailConfig.Value;

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

        #region Public
        public async Task<ResponseApi> SignUp(DoctorRegistrationModelView DoctorReg)
        {
      
                if (_dbContext.Users.Any(x => x.Email.Equals(DoctorReg.Email, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var response = new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "Email already Exist !",
                        Data = null
                    };
                         return response;
            }
                var hashedPassword = HashPassword(DoctorReg.Password);
           
            var doctor = _dbContext.Users.Add(new ApplicationUser
                {
                    FirstName = DoctorReg.FirstName,
                    LastName = DoctorReg.LastName,
                    Email = DoctorReg.Email,
                    PasswordHash = hashedPassword,
                    UserType ="Doctor",
                    

                    
            }).Entity;

                _dbContext.SaveChanges();
            var jwtSecurityToken = await CreateJwtToken(doctor);
                var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                var newDoc = _dbContext.Users.FirstOrDefault(x => x.Email == doctor.Email);
                var response1 = new ResponseApi
                 {
                        IsSuccess = true,
                        Message = "Register Successfully",
                        Data = new
                {
                          Id = newDoc.Id,
                          Email = doctor.Email,
                          DoctorName = doctor.FirstName+" "+doctor.LastName,
                          UserType = doctor.UserType,
                          IsValid = true,
                          Token = token,
                }
                };  
                          return response1;
            

        }

        public async Task<ResponseApi>  SendEmailResetPassword(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
            Random random = new Random();
            
            if (user == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "Invalid Email ",
                    Data = null
                };
                return response;
            }
            else
            {
                int randomNumber = random.Next(1000, 10000);
                user.ConfirmationCode = randomNumber;
                _dbContext.SaveChanges();

                var code = user.ConfirmationCode;
                var name = user.FirstName + " " + user.LastName;
               await SendEmailForResetPasswordAsync(code, name, user.Email);
                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "we have sent a message to your email",
                    Data = new
                    {
                        Email = user.Email,
                        Code = code
                    }
                };
                return response;
            }
        }

        public async Task<ResponseApi> SignIn(PatientLoginModelView DoctorLogin)
        {
            var doctor = _dbContext.Users.FirstOrDefault(x => x.Email
                          .Equals(DoctorLogin.Email,
                          StringComparison.InvariantCultureIgnoreCase));

            if (doctor == null || !VerifyHashPassword(DoctorLogin.Password, doctor.PasswordHash))
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "Invalid Email or password received",
                    Data = null
                };
                return response;
            }
            else
            {
            var jwtSecurityToken = await CreateJwtToken(doctor);
            var result = _mapper.Map<LoginDoctorResponse>(doctor);
            result.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Login Successfully",
                Data = new
                {
                    Id = doctor.Id,
                    DoctorName = doctor.FirstName+" "+doctor.LastName,
                    Email = doctor.Email,
                    Image = doctor.Image,
                    UserType = doctor.UserType,
                    Token = result.Token,
                    IsValid = true

                }
            };
            return response;

            }
        }
      public  ResponseApi ResetPassword(string email, ResetPasswordVM resetPasswordVM)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "Invalid Email",
                    Data = null
                };
                return response;
            }
            else
            {
                if (resetPasswordVM == null || resetPasswordVM.NewPassword != resetPasswordVM.ConfirmNewPassword)
                {

                    var response = new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "The new password and confirmation of the new password don't match ",
                        Data = null
                    };

                    return response;
                }
                else
                {
                    user.PasswordHash = HashPassword(resetPasswordVM.NewPassword);
                    _dbContext.SaveChanges();
                    var response = new ResponseApi
                    {
                        IsSuccess = true,
                        Message = " The Password Reset Successfully",
                        Data = null
                    };

                    return response;

                }

            }

        }

        public ResponseApi ConfirmationCode(int confirmationCode ,string email )
        {
            var user = _dbContext.Users
                           .FirstOrDefault(a => a.ConfirmationCode
                                                    .Equals(confirmationCode)
                                                    && a.Email ==email);
            if(user == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = " Invalid Confirmation Code ",
                    Data = null


                };
                return response;
            }
            else {
                user.EmailConfirmed = true;
                user.ConfirmationCode = 0;
                _dbContext.SaveChanges();
                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Confirmation Successfully,Now you can Reset your password",
                    Data = email


                };
                return response;
            }
                      

          
        }

        public ResponseApi GetTopDoctors()
        {
            var result = _dbContext.Doctors.Include(x => x.ApplicationUser)
                                            .Select(x => new
                                            {
                                                DoctorImage = x.ApplicationUser.Image,
                                                DoctorName = x.ApplicationUser.FirstName +" "+x.ApplicationUser.LastName,
                                                SpecialityName = x.Specialty.SpecialtyName,
                                                DoctorId = x.Id,
                                                ClinicAddress = x.ClinicAddress
                                            }).ToList();
            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Success",
                Data = result
            };
            return response;
            
        }

        public ResponseApi GetTopDoctorsBySpecificNumber(int Number)
        {
            var result = _dbContext.Doctors.Include(x => x.ApplicationUser)
                                            .Where(x => x.UserId == x.ApplicationUser.Id).Take(Number).ToList();

            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Success , But Doctor was exist",
                Data = result
            };
            return response;

        }

        public ResponseApi GetAllPatients()
        {
            var result = _dbContext.Users.Where(x => x.UserType == "Patient").Select(a => new
            {
                
                PatientName = a.FirstName +" "+a.LastName,
                PatientEmail = a.Email,
                BloodGroup = a.BloodGroup,
                PatientImage = a.Image
            }).ToList();

            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Success",
                Data = result
            };
            return response;

           
        }
        public ResponseApi CompleteDoctorProfile(string DoctorId, CompleteDoctorVM doctor)
        {
            var doc = new Doctor
            {
                UserId = DoctorId,
                ClinicAddress = doctor.ClinicAddress,
                ClinicName = doctor.ClinicName,
                Degree = doctor.Degree,
                College = doctor.College,
                YearOfCompletion = doctor.YearOfCompletion,
                HospitalName = doctor.HospitalName,
                HospitalFrom = doctor.HospitalFrom,
                HospitalTo = doctor.HospitalTo,
                Designation = doctor.Designation,
                Registration = doctor.Registration,
                RegistrationYear = doctor.RegistrationYear,
                Membership = doctor.Membership,
                Awards = doctor.Awards,
                AwardsYear = doctor.AwardsYear,
                DoctorServices = doctor.DoctorServices,
                SpecialtyId = doctor.SpecialtyId,
                AboutMe = doctor.AboutMe,
                Pricing = doctor.Pricing,
            };

            string folder = "Uploads/DoctorImages";
            folder = UploadImage(folder, doctor.ImageFile);
            doctor.Image = folder;
            var existDoctor = _dbContext.Doctors.FirstOrDefault(x => x.UserId == DoctorId);
            if (existDoctor != null)
            {
                var response1 = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Success , But Doctor was exist",
                    Data = existDoctor
                };
                return response1;
            }
            _dbContext.Doctors.Add(doc);
            var existUser = _dbContext.Users.FirstOrDefault(x => x.Id == DoctorId);         

            existUser.UserName = doctor.UserName;
            existUser.PhoneNumber = doctor.PhoneNumber;
            existUser.DateOfBirth = doctor.DateOfBirth;
            existUser.Image = doctor.Image;
            existUser.Gender = doctor.Gender;
            _dbContext.SaveChanges();

            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Success",
                Data = new
                {
                    UserId = DoctorId,
                    ClinicAddress = doctor.ClinicAddress,
                    ClinicName = doctor.ClinicName,
                    Degree = doctor.Degree,
                    College = doctor.College,
                    YearOfCompletion = doctor.YearOfCompletion,
                    HospitalName = doctor.HospitalName,
                    HospitalFrom = doctor.HospitalFrom,
                    HospitalTo = doctor.HospitalTo,
                    Designation = doctor.Designation,
                    Registration = doctor.Registration,
                    RegistrationYear = doctor.RegistrationYear,
                    Membership = doctor.Membership,
                    Awards = doctor.Awards,
                    AwardsYear = doctor.AwardsYear,
                    DoctorServices = doctor.DoctorServices,
                    SpecialtyName = _dbContext.Specializations.Where(z => z.Id == doctor.SpecialtyId).Select(z => z.SpecialtyName),
                    AboutMe = doctor.AboutMe,
                    Pricing = doctor.Pricing,
                    UserName = doctor.UserName,
                    PhoneNumber = doctor.PhoneNumber,
                    DateOfBirth = doctor.DateOfBirth,
                    Image = doctor.Image,
                    Gender = doctor.Gender
        }
            };
            return response;
        }

        public ResponseApi UpdateDoctorProfile(string DoctorId, UpdateDoctorVM doctor)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == DoctorId);
            string folder = "Uploads/DoctorImages";
            folder = UploadImage(folder, doctor.ImageFile);
            doctor.Image = folder;

            if (user == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "user desen't exist",
                    Data = null
                };
                return response;
            }
            user.PhoneNumber = doctor.PhoneNumber;
            user.Image = doctor.Image;

            var doc = _dbContext.Doctors.FirstOrDefault(x => x.UserId == DoctorId);
            if (doc == null)
            {
                var response1 = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "user desen't exist",
                    Data = null
                };
                return response1;
            }
            doc.ClinicAddress = doctor.ClinicAddress;
            doc.ClinicName = doctor.ClinicName;
            doc.HospitalName = doctor.HospitalName;
            doc.HospitalFrom = doctor.HospitalFrom;
            doc.HospitalTo = doctor.HospitalTo;
            doc.Designation = doctor.Designation;
            doc.Registration = doctor.Registration;
            doc.RegistrationYear = doctor.RegistrationYear;
            doc.Awards = doctor.Awards;
            doc.AwardsYear = doctor.AwardsYear;
            doc.DoctorServices = doctor.DoctorServices;
            doc.AboutMe = doctor.AboutMe;
            doc.Pricing = doctor.Pricing;

            _dbContext.SaveChanges();
            var response2 = new ResponseApi
            {
                IsSuccess = true,
                Message = "Success",
                Data = doc
            };
            return response2;
        }

        public ResponseApi SearchDoctors(string gender, int specialtyId)
        {
            var result = _dbContext.Doctors
            .Where(d => d.SpecialtyId == specialtyId)
            .Join(_dbContext.Users, d => d.UserId, au => au.Id, (d, au) => new { Doctor = d, ApplicationUser = au })
            .Where(x => x.ApplicationUser.Gender == gender)
            .Select(x => new
            {
                DoctorImage = x.ApplicationUser.Image,
                DoctorName = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName,
                SpecialityName = x.Doctor.Specialty.SpecialtyName,
                IdDoctor = x.Doctor.Id,
                ClinicAddress = x.Doctor.ClinicAddress,
                Degree = x.Doctor.Degree
            }).ToList();

            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Success",
                Data = result
            };
            return response;
           
        }

        public ResponseApi GetMyPatientAppointment(string userId)
        {
            var doctor = _dbContext.Doctors.FirstOrDefault(d => d.UserId == userId);
            var appointments = _dbContext.Appointments.Where(d => d.DoctorId == doctor.Id).OrderBy(b => b.Date)
                .Select(x => new
                {
                    PatientName = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName,
                    PatientImage = x.ApplicationUser.Image,
                    Address = x.ApplicationUser.Address,
                    Email = x.ApplicationUser.Email,
                    Day = x.Day,
                    Time = x.StartTime.ToString("hh:mm tt") + "-" + x.EndTime.ToString("hh:mm tt"),
                    Date = x.Date.ToString("yyyy-MM-dd"),
                    Status = x.Status
                }).ToList();

            if (!appointments.Any())
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "You don't have any patient appointment",
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

        public ResponseApi GetMyPatient(string userId)
        {
            var doctor = _dbContext.Doctors.FirstOrDefault(d => d.UserId == userId);
            var appointments = _dbContext.Appointments.Where(d => d.DoctorId == doctor.Id).OrderBy(b => b.Date)
                .Select(x => new
                {
                    PatientName = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName,
                    PatientImage = x.ApplicationUser.Image,
                    Address = x.ApplicationUser.Address,
                    Email = x.ApplicationUser.Email,
                    BloodGroup = x.ApplicationUser.BloodGroup
                    }).ToList();

            if (!appointments.Any())
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "You don't have any patient",
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

        public ResponseApi ChangePassword(string userId, ChangePasswordViewModel changePasswordVM)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
            
            if(changePasswordVM == null || !VerifyHashPassword(changePasswordVM.OldPassword, user.PasswordHash))
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "Your Old Password is incorrect",
                    Data = null
                };
                return response;
            }
            else
            {
                if(changePasswordVM.NewPassword != changePasswordVM.ConfirmNewPassword)
                {
                    var response = new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "The new password and confirmation of the new password don't match ",
                        Data = null
                    };
                    return response;
                }
                else
                {
                   
                    var hashedNewPassword = HashPassword(changePasswordVM.NewPassword);
                    user.PasswordHash = hashedNewPassword;
                    _dbContext.SaveChanges();
                    var response = new ResponseApi
                    {
                        IsSuccess = true,
                        Message = "The Password has been Changed Successfully",
                        Data = null
                    };
                    return response;
                }
               
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
            //int DoctorId = 5;
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
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("DoctorId", user.Id),
                new Claim("UserType", user.UserType),
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


        private async System.Threading.Tasks.Task SendEmailForResetPasswordAsync(int code, string name, string email)
        {
            try
            {
                MimeMessage emailMessage = new MimeMessage();
                //now do the HTML formatting
                MailboxAddress emailFrom = new MailboxAddress(_emailConfig.SmtpServer, _emailConfig.From);
                emailMessage.From.Add(emailFrom);
                var message = new MailMessage();
                MailboxAddress emailTo = new MailboxAddress("Customer", email);
                emailMessage.To.Add(emailTo);
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = $"<div width = \"100% !important\" style=\"background:#fff; width:100%!important; margin:0; padding:0; font-family:'Roboto',Helvetica,sans-serif; color:rgb(70,72,74,.9); font-size:15px; line-height:1.5em\">" +
                       "<table align = \"center\" bgcolor=\"#e1f1fd\" border=\"0\" cellpadding=\"35\" cellspacing=\"0\" width=\"90%\" style=\"margin:0px auto; max-width:800px; display:table\">" +
                       "<tbody><tr><td align = \"left\" style=\"border-collapse:collapse; text-align:left; font-family:'Muli',Helvetica,sans-serif; font-size:32px; font-weight:900; color:#1B5379; padding-top:20px; letter-spacing:-1px; line-height:1em\">" +
                       "<img data-imagetype=\"External\" src=\"\" width=\"200\" style=\"width:200px\">" +
                       "<br aria-hidden=\"true\"><br aria-hidden=\"true\"><br aria-hidden=\"true\">" +
                       "<span style = \"width:80%; display:block; margin-bottom:40px\">" +
                       "</br>Rest Your Account Password .</br></span> </td></tr></tbody></table>" +
                       "<table align = \"center\" bgcolor= \"#ffffff\" border= \"0\" cellpadding= \"35\" cellspacing= \"0\" width= \"90%\" style= \"margin:0px auto; max-width:800px; display:table\" >" +
                       $"<tbody><tr><td align= \"left\" style= \"border-collapse:collapse; text-align:left\" > Hi" +
                       $"<br>" +
$"<br>" +
                       $"Please follow the link to reset your account password : {code}. <br><br>Thanks,<br> The Health Care Team<br></td></tr></tbody></table>" +
                       "<table align=\"center\" bgcolor=\"#3599e8\" border=\"0\" cellpadding=\"35\" cellspacing=\"0\" width=\"90%\" style=\"margin:0px auto; text-align:center; max-width:800px; color:rgba(255,255,255,.5); font-size:11px; display:table\">" +
                       "<tbody>" +
                       "<tr width=\"100%\"><td width=\"100%\" style=\"border - collapse:collapse\"><img data-imagetype=\"External\" src=\"\" alt=\"Health\" width=\"25\" style=\"width:25px; opacity:.5\">" +
                       "<br aria-hidden=\"true\"><span style=\"color:#fff; color:rgba(255,255,255,.5)\">Thank you for your cooperation</span> | " +
                       "<a href=\"\" target=\"_blank\" rel=\"noopener noreferrer\" data-auth=\"NotApplicable\" style=\"text-decoration:none; color:#fff; color:rgba(255,255,255,.5)\" data-linkindex=\"1\">Unsubscripted</a> </td></tr></tbody>";
                emailMessage.Body = emailBodyBuilder.ToMessageBody();
                MailKit.Net.Smtp.SmtpClient emailClient = new MailKit.Net.Smtp.SmtpClient();
                await emailClient.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                emailClient.Authenticate(_emailConfig.From, _emailConfig.Password);
                await emailClient.SendAsync(emailMessage);
                emailClient.Disconnect(true);
                emailClient.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        #endregion private
    }
}
