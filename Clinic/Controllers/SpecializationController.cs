using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Clinic.Controllers
{
    [Authorize]
    [ApiController]
    public class SpecializationController : BaseController
    {
        private ISpecializationManager _specializationManager;
        private readonly ILogger<SpecializationController> _logger;
        private readonly IHttpContextAccessor __httpContextAccessor;
        public SpecializationController(ILogger<SpecializationController> logger,
                              ISpecializationManager specializationManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
            _specializationManager = specializationManager;
            __httpContextAccessor = httpContextAccessor;
        }
        [AllowAnonymous]
        [Route("api/specialties/GetAllSpecialties")]
        [HttpGet]
        public IActionResult GetAllSpecialties()
        {
            var result = _specializationManager.GetAllSpecialties();
            return Ok(result);

        }
        [AllowAnonymous]
        [Route("api/specialties/GetSpecialtiesBySpecificNum")]
        [HttpGet]
        public IActionResult GetSpecialtiesBySpecificNum(int NumberOfSpecialties)
        {
            var result = _specializationManager.GetSpecialtiesBySpecificNum(NumberOfSpecialties);
            return Ok(result);

        }

        [Route("api/specialties/CreateSpecialty")]
        [HttpPost]
         public IActionResult CreateSpecialty([FromForm] SpectalizationModelView specialtyMV)
        {
            var result = _specializationManager.CreateSpecialty(_DoctorId,specialtyMV);
            return Ok(result);
        }

        
        [Route("api/specialties/UpdateSpecialty")]
        [HttpPut]
        public IActionResult UpdateSpecialty([FromForm] Specialization currentSpecialty, [FromForm]int specialtyId)
        {
            var result = _specializationManager.UpdateSpecialty(_DoctorId,currentSpecialty, specialtyId);
            return Ok(result);
        }

        [Route("api/specialties/DeleteSpecialtyById")]
        [HttpPut]
        public IActionResult DeleteSpecialty([FromForm]int SpecialtyId)
        {
            var result = _specializationManager.DeleteSpecialty(_DoctorId,SpecialtyId);
            return Ok(result);

        }
    }



}

