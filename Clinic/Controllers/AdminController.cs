using Clinic_Core.Managers.Interfaces;
using Clinic_ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    [Authorize]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IAdminManager _adminManager;
        public AdminController(IAdminManager adminManager)
        {
            _adminManager = adminManager;      
        }

        [Route("api/admin/GetAllPatients")]
        [HttpGet]
        public IActionResult GetAllPatients()
        {
            var result = _adminManager.GetAllPatients();
            return Ok(result);
        }

        [Route("api/admin/GetAllDoctors")]
        [HttpGet]
        public IActionResult GetAllDoctors()
        {
            var result = _adminManager.GetAllDoctors();
            return Ok(result);
        }

        [Route("api/admin/GetAllAppointment")]
        [HttpGet]
        public IActionResult GetAllAppointment()
        {
            var result = _adminManager.GetAllAppointment();
            return Ok(result);
        }
    }
}
