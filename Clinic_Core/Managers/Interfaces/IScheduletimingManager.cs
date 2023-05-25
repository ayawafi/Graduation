using Clinic_ModelView;
using System.Collections.Generic;

namespace Clinic_Core.Managers.Interfaces
{
    public interface IScheduletimingManager
    {
        ScheduletimingModelView AddScheduletiming(string DoctorId, ScheduletimingModelView scheduletiming);
        List<ScheduletimingVM> GetBusinessHoursForDoctor(string DoctorId);
        List<string> GetScheduletimingsForDoctor(string doctorId, string day);

    }
}
