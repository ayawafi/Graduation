using Clinic_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Core.Managers.Interfaces
{
    public interface IAdminManager
    {
        ResponseApi GetAllPatients();
        ResponseApi GetAllDoctors();
        ResponseApi GetAllAppointment();
    }
}
