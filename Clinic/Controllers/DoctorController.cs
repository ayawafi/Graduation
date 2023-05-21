using clinic_Core.Managers.Interfaces;
using Clinic_ModelView;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    //[Authorize]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private IDoctorManager _doctorManager;
        public DoctorController(IDoctorManager doctorManager)
        {
            _doctorManager = doctorManager;
        }
        [Route("api/doctor/SignUp")]
        [HttpPost]
        public IActionResult SignUp(DoctorRegistrationModelView doctorReg)
        {
            var res = _doctorManager.SignUp(doctorReg);
            return Ok(res);
        }


        [Route("api/doctor/SignIn")]
        [HttpPost]
        public IActionResult SignIn(PatientLoginModelView DoctorLogin)
        {
            var res = _doctorManager.SignIn(DoctorLogin);
            return Ok(res);
        }

        [Route("api/doctor/GetAllPatients")]
        [HttpGet]
        public IActionResult GetAllPatients()
        {
            var result = _doctorManager.GetAllPatients();
            return Ok(result);

        }


    }
}
