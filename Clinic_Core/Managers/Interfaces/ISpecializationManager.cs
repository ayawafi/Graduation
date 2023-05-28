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
        ResponseApi CreateSpecialty(SpectalizationModelView specialtyMV);
        ResponseApi UpdateSpecialty(SpectalizationModelView currentSpecialty);
        ResponseApi DeleteSpecialty(SpectalizationModelView currentSpecialty);
        ResponseApi GetSpecialtiesBySpecificNum(int NumberOfSpecialties);
    }
}
