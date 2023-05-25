using clinic_Core.Managers.Interfaces;
using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace Clinic.Controllers
{
    [Authorize]
    [ApiController]
    public class AppointmentController : BaseController
    {
        private IAppointmentManager _appointmentManager;
        private readonly IHttpContextAccessor __httpContextAccessor;
        public AppointmentController(IAppointmentManager appointmentManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _appointmentManager = appointmentManager;
            __httpContextAccessor = httpContextAccessor;
        }
        [Route("api/appoitment/CreateAppointments")]
        [HttpPost]
        public IActionResult CreateAppointments(AppointmentModelView appointment)
        {
            var result = _appointmentManager.CreateAppointments(_DoctorId, appointment);
            return Ok(result);
        }
        [Route("api/appoitment/BookedAppointments")]
        [HttpGet]
        public IActionResult BookedAppointments(int doctorId,DateTime date)
        {
            var result = _appointmentManager.BookedAppointments(doctorId, date);
            return Ok(result);
        }
    }
}
