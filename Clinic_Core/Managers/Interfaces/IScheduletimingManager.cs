using Clinic_ModelView;
using System;
using System.Collections.Generic;

namespace Clinic_Core.Managers.Interfaces
{
    public interface IScheduletimingManager
    {
        ResponseApi AddScheduletiming(string DoctorId, ScheduletimingModelView scheduletiming);
        ResponseApi GetBusinessHoursForDoctor(int DoctorId);
        ResponseApi GetScheduletimingsForDoctor(int doctorId, DateTime date);

    }
}
