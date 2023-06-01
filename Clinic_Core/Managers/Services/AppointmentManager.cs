using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clinic_Core.Managers
{
    public class AppointmentManager : IAppointmentManager
    {
        private clinic_dbContext _dbContext;
        public AppointmentManager(clinic_dbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Appointment> BookedAppointments(int doctorId ,DateTime date )
        {
            
            var bookedAppointments = _dbContext.Appointments.Where(x => x.DoctorId == doctorId
                                                                   &&  x.Date.Date == date.Date).ToList();
            
            return bookedAppointments;
        }

        public ResponseApi CreateAppointments(string patientId, AppointmentModelView appointment)
        {

            var newAppointment = new Appointment
            {
                UserId = patientId,
                DoctorId = appointment.DoctorId,
                Date = appointment.Date,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Day = appointment.Day
            };

            _dbContext.Appointments.Add(newAppointment);
            _dbContext.SaveChanges();

            var doc = _dbContext.Doctors.Where(x => x.Id == appointment.DoctorId)
                .Select(doc => new
                {

                    DoctorId = doc.Id,
                    DoctorName = doc.ApplicationUser.FirstName + " " + doc.ApplicationUser.LastName,
                    DoctorImage = doc.ApplicationUser.Image,
                    DoctorSpeciality = doc.Specialty.SpecialtyName,
                    Day = appointment.Day,
                    Date = appointment.Date,
                    StartTime = appointment.StartTime,
                    EndTime = appointment.EndTime,
                    Status = appointment.Status
                })
                .AsSingleQuery()
                .AsNoTracking()
                .FirstOrDefault();

            var response = new ResponseApi
            {

                IsSuccess = true,
                Message = "Successfully Created Appointment",
                Data = doc
            };
            return response;
           
        }


    }
}
