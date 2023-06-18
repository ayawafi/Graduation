using clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        public async Task<IActionResult> SignUpAsync([FromForm]DoctorRegistrationModelView doctorReg)
        {
            var res = await _doctorManager.SignUp(doctorReg);

            return Ok(res);
        }

        [AllowAnonymous]
        [Route("api/doctor/SignIn")]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromForm] PatientLoginModelView DoctorLogin)
        {
            var res = await _doctorManager.SignIn(DoctorLogin);
            return Ok(res);
        }

   

        [Route("api/doctor/changepassword")]
        [HttpPut]
        public IActionResult ChangePassword([FromForm] ChangePasswordViewModel changePasswordVM)
        {
            var result = _doctorManager.ChangePassword(_DoctorId, changePasswordVM);
            return Ok(result);
        }

        [AllowAnonymous]
        [Route("api/doctor/SendEmailResetPassword")]
        [HttpPut]
        public async Task<IActionResult> SendEmailResetPassword([FromForm]string email)
        {
            var result =await _doctorManager.SendEmailResetPassword(email);
            return Ok(result);
        }
        [AllowAnonymous]
        [Route("api/doctor/ConfirmationCode")]
        [HttpPost]
        public IActionResult ConfirmationCode([FromForm] int confirmationCode,string email)
        {
            var result = _doctorManager.ConfirmationCode(confirmationCode, email);
            return Ok(result);
        }

        [AllowAnonymous]
        [Route("api/doctor/ResetPassword")]
        [HttpPost]
        public IActionResult ResetPassword(string email, [FromForm] ResetPasswordVM resetPasswordVM)
        {
            var result = _doctorManager.ResetPassword(email, resetPasswordVM);
            return Ok(result);
        }

        
        [Route("api/doctor/GetAllPatients")]
        [HttpGet]
        public IActionResult GetAllPatients()
        {
            var result = _doctorManager.GetAllPatients();
            return Ok(result);

        }
        [AllowAnonymous]
        [Route("api/doctor/GetTopDoctors")]
        [HttpGet]
        public IActionResult GetTopDoctors()
        {
            var result = _doctorManager.GetTopDoctors();
            return Ok(result);
        }
        [AllowAnonymous]
        [Route("api/doctor/GetTopDoctorsBySpecificNumber")]
        [HttpGet]
        public IActionResult GetTopDoctorsBySpecificNumber(int Number)
        {
            var result = _doctorManager.GetTopDoctorsBySpecificNumber(Number);
            return Ok(result);
        }

        [Route("api/doctor/CompleteDoctorProfile")]
        [HttpPost]
        public IActionResult CompleteDoctorProfile([FromForm] CompleteDoctorVM doctor)
        {
            var result = _doctorManager.CompleteDoctorProfile(_DoctorId, doctor);
            return Ok(result);
        }
        [Route("api/doctor/UpdateDoctorProfile")]
        [HttpPut]
        public IActionResult UpdateDoctorProfile([FromForm] UpdateDoctorVM doctor)
        {
            var result = _doctorManager.UpdateDoctorProfile(_DoctorId, doctor);
            return Ok(result);
        }
        [AllowAnonymous]
        [Route("api/doctor/SearchDoctorsByGender&Specialty")]
        [HttpGet]
        public IActionResult SearchDoctors(string gender, int Specialty)
        {
            var result = _doctorManager.SearchDoctors(gender, Specialty);
            return Ok(result);
        }

        [Route("api/doctor/SearchByClinicNameOrAddress")]
        [HttpGet]
        public IActionResult SearchByClinicNameOrAddress(string clinicAddress, string clinicName)
        {
            var result = _doctorManager.Search(clinicAddress, clinicName);
            return Ok(result);
        }


        [Route("api/doctor/GetMyPatientAppointment")]
        [HttpGet]
        public IActionResult GetMyPatientAppointment()
        {
            var result = _doctorManager.GetMyPatientAppointment(_DoctorId);
            return Ok(result);
        }

        [Route("api/doctor/GetMyPatient")]
        [HttpGet]
        public IActionResult GetMyPatient()
        {
            var result = _doctorManager.GetMyPatient(_DoctorId);
            return Ok(result);
        }

        [Authorize]
        [Route("api/doctor/GetDoctorProfileById")]
        [HttpGet]
        public IActionResult GetDoctorProfileById(int doctorId)
        {
            var res = _doctorManager.GetDoctorProfileById(doctorId);
            return Ok(res);
        }
    }
}
