using Clinic_DbModel.Models;
using Clinic_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Core.Managers.Interfaces
{
    public interface IPatientManager
    {
        Task<ResponseApi> SignUp(PatientRegistrationModelView PatientReg);
        ResponseApi UpdateProfilePatient(string userId, UpdatePatientProfilVM appUser);
        ResponseApi CompletePatientProfile(string userId, PatientProfileSettings profileSettings);
    }
}
