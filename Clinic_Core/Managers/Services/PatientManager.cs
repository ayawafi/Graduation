using AutoMapper;
using Clinic_Common.Extensions;
using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Core.Managers.Services
{
    public class PatientManager : IPatientManager
    {
        private clinic_dbContext _dbContext;
        private IMapper _mapper;

        public PatientManager(clinic_dbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #region Public
        public LoginPatientResponse SignUp(PatientRegistrationModelView PatientReg)
        {
            if (_dbContext.Users.Any(x => x.Email.Equals(PatientReg.Email, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ServiceValidationException("Email already Exist !");
            }

            var hashedPassword = HashPassword(PatientReg.Password);
            var patient = _dbContext.Users.Add(new ApplicationUser
            {
                FirstName = PatientReg.FirstName,
                LastName = PatientReg.LastName,
                Email = PatientReg.Email,
                PasswordHash = hashedPassword,
            }).Entity;

            _dbContext.SaveChanges();
            var result = _mapper.Map<LoginPatientResponse>(patient);
            return result;
        }

        //public LoginPatientResponse SignIn(PatientLoginModelView PatientLogin)
        //{
        //    var Patient = _dbContext.Users.FirstOrDefault(x => x.Email
        //                  .Equals(PatientLogin.Email,
        //                  StringComparison.InvariantCultureIgnoreCase));

        //    if (Patient == null || !VerifyHashPassword(PatientLogin.Password, Patient.PasswordHash))
        //    {
        //        throw new ServiceValidationException(300, "Invalid Email or password received");
        //    }

        //    var result = _mapper.Map<LoginPatientResponse>(Patient);
        //    return result;
        //}
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
        #endregion private
    }
}
