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
        Task<LoginPatientResponse> SignUp(PatientRegistrationModelView PatientReg);
        ApplicationUser UpdateProfilePatient(string userId, UpdatePatientProfilVM appUser);
        string CompletePatientProfile(string userId, PatientProfileSettings profileSettings);
        //LoginPatientResponse SignIn(PatientLoginModelView PatientLogin);
    }
}
