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
        [Authorize]
        [Route("api/Patient/FavouritesDoctor")]
        [HttpPost]
        public IActionResult FavouritesDoctor(int doctorId)
        {
            var res = _patientManager.FavouritesDoctor(_DoctorId, doctorId);
            return Ok(res);
        }

        [Authorize]
        [Route("api/Patient/GetFavouritesDoctor")]
        [HttpGet]
        public IActionResult GetFavouritesDoctor()
        {
            var res = _patientManager.GetFavouritesDoctor(_DoctorId);
            return Ok(res);
        }

        [Authorize]
        [Route("api/Patient/DeleteFavouriteDoctor")]
        [HttpDelete]
        public IActionResult DeleteFavouriteDoctor(int doctorId)
        {
            var res = _patientManager.DeleteFavouriteDoctor(_DoctorId, doctorId);
            return Ok(res);
        }

    }
}
