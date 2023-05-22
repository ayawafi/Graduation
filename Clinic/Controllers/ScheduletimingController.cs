using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    [ApiController]
    public class ScheduletimingController : Controller
    {
        private IScheduletimingManager _scheduletimingManager;
        public ScheduletimingController(IScheduletimingManager scheduletimingManager)
        {
            _scheduletimingManager = scheduletimingManager;
        }
        [Route("api/scheduletiming/addscheduletiming")]
        [HttpPost]
        public IActionResult AddScheduletiming(int doctorId, ScheduletimingModelView scheduletiming)
        {
            var time = _scheduletimingManager.AddScheduletiming(doctorId, scheduletiming);
            return Ok(time);
        }

        [Route("api/scheduletiming/getscheduletimingfordoctor")]
        [HttpGet]
        public IActionResult GetScheduletimingForDoctor(int doctorId)
        {
            var result = _scheduletimingManager.GetScheduletimingForDoctor(doctorId);
            return Ok(result);
        }
    }
}
