using Clinic_DbModel.Models;
using Clinic_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Core.Managers.Interfaces
{
    public interface IAppointmentManager 
    {
        List<Appointment> BookedAppointments(int doctorId, DateTime date);
        ResponseApi CreateAppointments(string patientId, AppointmentModelView appointment);
    }
}
