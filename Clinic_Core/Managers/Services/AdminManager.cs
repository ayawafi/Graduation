using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using System;
using System.Linq;

namespace Clinic_Core.Managers.Services
{
    public class AdminManager : IAdminManager
    {
        private clinic_dbContext _dbContext;
        public AdminManager(clinic_dbContext dbContext)
        {
            _dbContext = dbContext;      
        }

        public ResponseApi GetAllPatients()
        {
            var result = _dbContext.Users.Where(x => x.UserType == "Patient").Select(a => new
            {

                PatientName = a.FirstName + " " + a.LastName,
                PatientImage = a.Image,
                Address = a.Address,

            }).ToList();

            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Success",
                Data = result
            };
            return response;

        }

        public ResponseApi GetAllDoctors()
        {
            var result = _dbContext.Doctors.Select(x => new
            { 
                Name = x.ApplicationUser.FirstName +" "+ x.ApplicationUser.LastName,
                Specialty = x.Specialty.SpecialtyName
            }).ToList();
            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Success",
                Data = result
            };
            return response;
        }

        public ResponseApi GetAllAppointment()
        {
            var result = _dbContext.Appointments.Select(x => new
            {
                DoctorName = x.Doctor.ApplicationUser.FirstName + " " + x.Doctor.ApplicationUser.LastName,
                DoctorImage = x.Doctor.ApplicationUser.Image,
                Specialty = x.Doctor.Specialty.SpecialtyName,
                PatientName = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName,
                PatientImge =x.ApplicationUser.Image,
                AppointmentDate =x.Date.ToString("yyyy-MM-dd") + " "+x.Day+" "+
                                 x.StartTime.ToString("hh:mm tt") + "-" 
                                 + x.EndTime.ToString("hh:mm tt")

            }).ToList();
            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Success",
                Data = result
            };
            return response;
        }
        #region Private
        //private int CalculateAge(DateTime birthday)
        //{
        //    DateTime today = DateTime.Today;
        //    int age = today.Year - birthday.Year;

        //    if (today.Month < birthday.Month || (today.Month == birthday.Month && today.Day < birthday.Day))
        //    {
        //        age--;
        //    }

        //    return age;
        //}
        #endregion Private
    }
}
