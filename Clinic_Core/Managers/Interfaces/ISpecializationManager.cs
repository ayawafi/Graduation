using Clinic_DbModel.Models;
using Clinic_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Core.Managers.Interfaces
{
    public interface ISpecializationManager
    {
        ResponseApi GetAllSpecialties();
        ResponseApi CreateSpecialty(string adminId,SpectalizationModelView specialtyMV);
        ResponseApi UpdateSpecialty(string adminId, Specialization currentSpecialty, int specialtyId);
        ResponseApi DeleteSpecialty(string adminId, int SpecialtyId);
        ResponseApi GetSpecialtiesBySpecificNum(int NumberOfSpecialties);
    }
}
