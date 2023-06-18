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

        public ResponseApi UpdateMyAppointment(int appointmetId, AppointmentModelView appointment)
        {
            var myAppoitment = _dbContext.Appointments.FirstOrDefault(z => z.Id == appointmetId);

            if(myAppoitment == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "There is no appointment with this Id",
                    Data = null
                };
                return response;
            }
            else {
                myAppoitment.Day = appointment.Day;
                myAppoitment.Date = appointment.Date;
                myAppoitment.StartTime = appointment.StartTime;
                myAppoitment.EndTime = appointment.EndTime;

                _dbContext.SaveChanges();
                var newAppointment = _dbContext.Appointments.FirstOrDefault(z => z.Id == appointmetId);
                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Successfully Updated",
                    Data = newAppointment
                };

                return response;
            }
           
        }

        public ResponseApi DeleteMyAppointment(int appointmentId)
        {
            var appId = _dbContext.Appointments.FirstOrDefault(v => v.Id == appointmentId);

            if(appId == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "There is no appointment with this Id",
                    Data = null
                };
                return response;
            }
            else
            {
                appId.IsDeleted = true;
                _dbContext.SaveChanges();
                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Appointment is deleted Successfully",
                    Data = null
                };
                return response;
            }

        }


    }
}
