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
        SpectalizationModelView CreateSpecialty(SpectalizationModelView specialtyMV);
        SpectalizationModelView UpdateSpecialty(SpectalizationModelView currentSpecialty);
        SpectalizationModelView DeleteSpecialty(SpectalizationModelView currentSpecialty);
        ResponseApi GetSpecialtiesBySpecificNum(int NumberOfSpecialties);
    }
}
