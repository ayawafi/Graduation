using Clinic_DbModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Core.Managers.Interfaces
{
    public interface ISpecializationManager
    {
        List<Specialization> GetAllSpecialties();
    }
}
