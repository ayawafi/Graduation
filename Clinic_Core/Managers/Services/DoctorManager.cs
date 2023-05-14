using AutoMapper;
using Clinic_Common.Extensions;
using clinic_Core.Managers.Interfaces;
using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Core.Managers.Services
{
    public class DoctorManager : IDoctorManager
    {
        private clinic_dbContext _dbContext;
        private IMapper _mapper;

        public DoctorManager(clinic_dbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #region Public
        public LoginDoctorResponse SignUp(DoctorRegistrationModelView DoctorReg)
        {
            if (_dbContext.Doctors.Any(x => x.Email.Equals(DoctorReg.Email, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ServiceValidationException("Email already Exist !");
            }

            var hashedPassword = HashPassword(DoctorReg.Password);
            var doctor = _dbContext.Doctors.Add(new Doctor
            {
                FirstName = DoctorReg.FirstName,
                LastName = DoctorReg.LastName,
                Email = DoctorReg.Email,
                PhoneNumber = DoctorReg.PhoneNumber,  
                Password = hashedPassword,
                ConfirmPassword = hashedPassword,
                SpecialtyId = 1
            }).Entity;

            _dbContext.SaveChanges();
            var result = _mapper.Map<LoginDoctorResponse>(doctor);
            return result;
        }

        public LoginDoctorResponse SignIn(PatientLoginModelView DoctorLogin)
        {
            var doctor = _dbContext.Doctors.FirstOrDefault(x => x.Email
                          .Equals(DoctorLogin.Email,
                          StringComparison.InvariantCultureIgnoreCase));

            if (doctor == null || !VerifyHashPassword(DoctorLogin.Password, doctor.Password))
            {
                throw new ServiceValidationException(300, "Invalid Email or password received");
            }

            var result = _mapper.Map<LoginDoctorResponse>(doctor);
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
        #endregion private
    }
}
