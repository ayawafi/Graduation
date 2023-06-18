using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult CreateAppointments([FromForm]AppointmentModelView appointment)
        {
            try
            {
                var result = _appointmentManager.CreateAppointments(_DoctorId, appointment);
                return Ok(result);
            }
     
            catch (Exception ex)
            {
                return Ok(new ResponseApi
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = null
                });

            }
        }
        [Route("api/appoitment/BookedAppointments")]
        [HttpGet]
        public IActionResult BookedAppointments(int doctorId,DateTime date)
        {
            var result = _appointmentManager.BookedAppointments(doctorId, date);
            return Ok(result);
        }

        [Route("api/appoitment/UpdateMyAppointment")]
        [HttpPut]
        public IActionResult UpdateMyAppointment(int appointmetId,[FromForm] AppointmentModelView appointment)
        {
            var result = _appointmentManager.UpdateMyAppointment(appointmetId, appointment);
            return Ok(result);
        }

        [Route("api/appoitment/DeleteMyAppointment")]
        [HttpPut]
        public IActionResult DeleteMyAppointment(int appointmentId)
        {
            var result = _appointmentManager.DeleteMyAppointment(appointmentId);
            return Ok(result);
        }
    }
}
