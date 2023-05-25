using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string CreateAppointments(string patientId, AppointmentModelView appointment)
        {
            var newAppointment = new Appointment
            {
                UserId = patientId,
                DoctorId = appointment.DoctorId,
                Date = appointment.Date,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Day= appointment.Day
            };

            _dbContext.Appointments.Add(newAppointment);
            _dbContext.SaveChanges();
            return "done";
        }


    }
}
