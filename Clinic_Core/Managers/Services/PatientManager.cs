using AutoMapper;
using Clinic_Common.Extensions;
using Clinic_Core.Helper;
using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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


        public PatientManager(clinic_dbContext dbContext, IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JWT> jwt, IConfiguration configuration)
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
                      IsValid = true,
                      Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                }
            };
            return response;
        }

        public ResponseApi UpdateProfilePatient(string userId, UpdatePatientProfilVM appUser)
        {
            var user = _dbContext.Users.Find(userId);
            if (user == null)
                throw new ServiceValidationException("user not exist");
            user.Email = appUser.Email;
            user.PhoneNumber = appUser.PhoneNumber;
            user.Address = appUser.Address;
            _dbContext.SaveChanges();

            var userAfterUpdate = _dbContext.Users.Find(userId);
            var response1 = new ResponseApi
            {
                IsSuccess = true,
                Message = "Updated Successfully",
                Data = userAfterUpdate
            };
            return response1;
            
        }

        public ResponseApi CompletePatientProfile(string userId, PatientProfileSettings profileSettings)
        {
            var user = _dbContext.Users.Find(userId);
            if (user == null)
                throw new ServiceValidationException("user not exist");
            user.DateOfBirth = profileSettings.DateOfBirth;
            user.BloodGroup = profileSettings.BloodGroup;
            user.Address = profileSettings.Address;

            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Success , But Doctor was exist",
                Data = user
            };
            return response;
           
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
        #endregion private
    }
}
