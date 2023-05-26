using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clinic.Controllers
{
    
    [ApiController]
    public class PatientController : BaseController
    {
        private IPatientManager _patientManager;
        private readonly IHttpContextAccessor __httpContextAccessor;     
        public PatientController(IPatientManager patientManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _patientManager = patientManager;
            __httpContextAccessor = httpContextAccessor;

        }
        [Route("api/patient/SignUp")]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm]PatientRegistrationModelView patientReg)
        {
            var res = await _patientManager.SignUp(patientReg);
            return Ok(res);
        }
       
        [Route("api/patient/CompletePatientProfile")]
        [HttpPost]
        public IActionResult CompletePatientProfile([FromForm] PatientProfileSettings profileSettings)
        {
            var result = _patientManager.CompletePatientProfile(_DoctorId, profileSettings);
            return Ok(result);
        }
        [Route("api/patient/updateProfilePatient")]
        [HttpPut]
        public IActionResult UpdateProfilePatient([FromForm] UpdatePatientProfilVM appUser)
        {
            var result = _patientManager.UpdateProfilePatient(_DoctorId, appUser);
            return Ok(result);
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
