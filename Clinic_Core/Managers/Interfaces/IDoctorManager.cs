using Clinic_DbModel.Models;
using Clinic_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_Core.Managers.Interfaces
{
    public interface IDoctorManager
    {
        LoginDoctorResponse SignUp(DoctorRegistrationModelView DoctorReg);
        LoginDoctorResponse SignIn(PatientLoginModelView DoctorLogin);
        List<Specialization> GetAllSpecialties();
    }
}
