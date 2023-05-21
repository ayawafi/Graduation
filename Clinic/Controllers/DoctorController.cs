﻿using clinic_Core.Managers.Interfaces;
using Clinic_ModelView;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clinic.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> SignUpAsync(DoctorRegistrationModelView doctorReg)
        {
            var res = await _doctorManager.SignUp(doctorReg);
            return Ok(res);
        }


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


    }
}
