using AutoMapper;
using Clinic_Common.Extensions;
using clinic_Core.Managers.Interfaces;
using Clinic_Core.Managers.Interfaces;
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
using System.Net.WebSockets;

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

        public DoctorManager(clinic_dbContext dbContext, IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JWT> jwt, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            //_jwt = jwt.Value;
            _configuration = configuration;
            _jwt = Binding();

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
        public async Task<LoginPatientResponse> SignUp(DoctorRegistrationModelView DoctorReg)
        {
      
                if (_dbContext.Users.Any(x => x.Email.Equals(DoctorReg.Email, StringComparison.InvariantCultureIgnoreCase)))
                {
                    throw new ServiceValidationException("Email already Exist !");
                }

                var hashedPassword = HashPassword(DoctorReg.Password);
                var doctor = _dbContext.Users.Add(new ApplicationUser
                {
                    FirstName = DoctorReg.FirstName,
                    LastName = DoctorReg.LastName,
                    Email = DoctorReg.Email,
                    PasswordHash = hashedPassword,

                }).Entity;


                _dbContext.SaveChanges();
                var jwtSecurityToken = await CreateJwtToken(doctor);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                var newDoc = _dbContext.Users.FirstOrDefault(x => x.Email == doctor.Email);
            return new LoginPatientResponse
            {
                Id = newDoc.Id,
                Email = doctor.Email,
                IsValid = true,
                Token = token,
            Message = "Login Successfully"
                };
            
              }

        public async Task<LoginDoctorResponse> SignIn(PatientLoginModelView DoctorLogin)
        {
            var doctor = _dbContext.Users.FirstOrDefault(x => x.Email
                          .Equals(DoctorLogin.Email,
                          StringComparison.InvariantCultureIgnoreCase));

            if (doctor == null || !VerifyHashPassword(DoctorLogin.Password, doctor.PasswordHash))
            {
                throw new ServiceValidationException(300, "Invalid Email or password received");
            }
            var jwtSecurityToken = await CreateJwtToken(doctor);
            var result = _mapper.Map<LoginDoctorResponse>(doctor);
            result.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return result;
        }

        public List<Doctor> GetTopDoctors()
        {
            var result = _dbContext.Doctors.Include(x => x.ApplicationUser)
                                            .Where(x => x.UserId == x.ApplicationUser.Id).ToList();
            return result;
        }

        public List<Doctor> GetTopDoctorsBySpecificNumber(int Number)
        {
            var result = _dbContext.Doctors.Include(x => x.ApplicationUser)
                                            .Where(x => x.UserId == x.ApplicationUser.Id).Take(Number).ToList();
            return result;
        }

        public List<ApplicationUser> GetAllPatients()
        {
            var result = _dbContext.Users.ToList();

            return result;
        }



        public string CompleteDoctorProfile(string DoctorId, UpdateDoctorVM doctor)
        {
            var doc = new Doctor
            {
                UserId = DoctorId,
                ClinicAddress = doctor.ClinicAddress,
                ClinicName = doctor.ClinicName,
                Degree = doctor.Degree,
                College = doctor.College,
                YearOfCompletion = doctor.YearOfCompletion,
                Experience= doctor.Experience,
                ExperienceFrom = doctor.ExperienceFrom,
                ExperienceTo = doctor.ExperienceTo,
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
            var existDoctor = _dbContext.Doctors.FirstOrDefault(x => x.UserId == DoctorId);
            if (existDoctor != null)
                return " Doctor was exist";



            _dbContext.Doctors.Add(doc);
            var existUser = _dbContext.Users.FirstOrDefault(x => x.Id == DoctorId);
            if(existUser == null)
                return " User doesn't exist";
            
            existUser.UserName = doctor.UserName;
            existUser.PhoneNumber = doctor.PhoneNumber;
            existUser.DateOfBirth = doctor.DateOfBirth;
            existUser.Image = doctor.Image;
            existUser.Gender = doctor.Gender;
            _dbContext.SaveChanges();
            return "Profile is Complete";
        }
       
        public string UpdateDoctorProfile(string DoctorId, UpdateDoctorVM doctor)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == DoctorId);
            if (user == null)
                return "user desen't exist";
            user.PhoneNumber = doctor.PhoneNumber;
            user.Image = doctor.Image;

            var doc = _dbContext.Doctors.FirstOrDefault(x => x.UserId == DoctorId);
            if (doc == null)
                return "user desen't exist";

            doc.ClinicAddress = doctor.ClinicAddress;
            doc.ClinicName = doctor.ClinicName;
            doc.Experience = doctor.Experience;
            doc.ExperienceFrom = doctor.ExperienceFrom;
            doc.ExperienceTo = doctor.ExperienceTo;
            doc.Designation = doctor.Designation;
            doc.Registration = doctor.Registration;
            doc.RegistrationYear = doctor.RegistrationYear;
            doc.Awards = doctor.Awards;
            doc.AwardsYear = doctor.AwardsYear;
            doc.DoctorServices = doctor.DoctorServices;
            doc.AboutMe = doctor.AboutMe;
            doc.Pricing = doctor.Pricing;


            _dbContext.SaveChanges();

            return "done";
        }
       
        public List<Doctor> SesrchDoctors(string gender, string Specialty)
        {
            var result = _dbContext.Doctors.Include(x => x.Specialty)
                                            .Where( x => x.ApplicationUser.Gender == gender
                                            && x.Specialty.SpecialtyName == Specialty).ToList();


            return result;
           
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
                //new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("DoctorId", user.Id),
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
        #endregion private
    }
}
