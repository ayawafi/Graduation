using Clinic_ModelView;
using System.Collections.Generic;

namespace Clinic_Core.Managers.Interfaces
{
    public interface IScheduletimingManager
    {
        ResponseApi AddScheduletiming(string DoctorId, ScheduletimingModelView scheduletiming);
        ResponseApi GetBusinessHoursForDoctor(string DoctorId);
        ResponseApi GetScheduletimingsForDoctor(string doctorId, string day);

    }
}
