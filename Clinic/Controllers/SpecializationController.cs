using clinic_Core.Managers.Interfaces;
using Clinic_Core.Managers.Interfaces;
using Clinic_Core.Managers.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private ISpecializationManager _specializationManager;
        public SpecializationController(ISpecializationManager specializationManager)
        {
            _specializationManager = specializationManager;
        }

        [Route("api/specialties/GetAllSpecialties")]
        [HttpGet]
        public IActionResult GetAllSpecialties()
        {
            var result = _specializationManager.GetAllSpecialties();
            return Ok(result);

        }



    }
}
