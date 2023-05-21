using Clinic_Core.Managers.Interfaces;
using Clinic_ModelView;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    [Authorize]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private IPatientManager _patientManager;
        public PatientController(IPatientManager patientManager)
        {
            _patientManager = patientManager;
        }
        [Route("api/patient/SignUp")]
        [HttpPost]
        public IActionResult SignUp(PatientRegistrationModelView patientReg)
        {
            var res = _patientManager.SignUp(patientReg);
            return Ok(res);
        }


        //[Route("api/Patient/SignIn")]
        //[HttpPost]
        //public IActionResult SignIn(PatientLoginModelView patientLogin)
        //{
        //    var res = _patientManager.SignIn(patientLogin);
        //    return Ok(res);
        //}
    }
}
