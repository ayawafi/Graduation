using clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clinic.Controllers
{

    [Authorize]
    [ApiController]
    public class DoctorController : BaseController
    {
        private IDoctorManager _doctorManager;
        private readonly IHttpContextAccessor __httpContextAccessor;

        public DoctorController(IDoctorManager doctorManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _doctorManager = doctorManager;
            __httpContextAccessor = httpContextAccessor;

        }
        [AllowAnonymous]
        [Route("api/doctor/SignUp")]
        [HttpPost]
        public async Task<IActionResult> SignUpAsync(DoctorRegistrationModelView doctorReg)
        {
            var res = await _doctorManager.SignUp(doctorReg);

            return Ok(res);
        }

        [AllowAnonymous]
        [Route("api/doctor/SignIn")]
        [HttpPost]
        public async Task<IActionResult> SignIn(PatientLoginModelView DoctorLogin)
        {
            var res = await _doctorManager.SignIn(DoctorLogin);
            return Ok(res);
        }

        [Route("api/doctor/GetAllPatients")]
        [HttpGet]
        public IActionResult GetAllPatients()
        {
            var result = _doctorManager.GetAllPatients();
            return Ok(result);

        }

        [Route("api/doctor/GetTopDoctors")]
        [HttpGet]
        public IActionResult GetTopDoctors()
        {
            var result = _doctorManager.GetTopDoctors();
            return Ok(result);
        }

        [Route("api/doctor/GetTopDoctorsBySpecificNumber")]
        [HttpGet]
        public IActionResult GetTopDoctorsBySpecificNumber(int Number)
        {
            var result = _doctorManager.GetTopDoctorsBySpecificNumber(Number);
            return Ok(result);
        }

        [Route("api/doctor/CompleteDoctorProfile")]
        [HttpPost]
        public IActionResult CompleteDoctorProfile([FromBody]UpdateDoctorVM doctor, [FromQuery] ApplicationUserVM appUser)
        {
            var result = _doctorManager.CompleteDoctorProfile(_DoctorId, doctor, appUser);
            return Ok(result);
        }
        [Route("api/doctor/updateDoctorProfile")]
        [HttpPost]
        public IActionResult UpdateDoctorProfile([FromBody] UpdateDoctorVM doctor, [FromQuery] ApplicationUserVM appUser)
        {
            var result = _doctorManager.UpdateDoctorProfile(_DoctorId, doctor, appUser);
            return Ok(result);
        }
        [AllowAnonymous]
        [Route("api/doctor/sesrchDoctors")]
        [HttpGet]
        public IActionResult SesrchDoctors(string gender, string Specialty)
        {
            var result = _doctorManager.SesrchDoctors(gender, Specialty);
            return Ok(result);
        }


    }
}
