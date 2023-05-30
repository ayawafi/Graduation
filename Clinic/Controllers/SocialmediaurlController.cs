using Clinic_Core.Managers.Interfaces;
using Clinic_ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    [Authorize]
    [ApiController]
    public class SocialmediaurlController : BaseController
    {
        private ISocialMediaUrlManager _socialMediaUrlManager;
        private readonly IHttpContextAccessor __httpContextAccessor;

        public SocialmediaurlController(ISocialMediaUrlManager socialMediaUrlManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _socialMediaUrlManager = socialMediaUrlManager;
            __httpContextAccessor = httpContextAccessor;
                    
        }

        [Route("api/socialmediaurl/AddSocialMediaURL")]
        [HttpPost]
        public IActionResult AddSocialMediaURL([FromForm]SocialMediaUrlModelView urlVM)
        {
            var result = _socialMediaUrlManager.AddSocialMediaURL(_DoctorId, urlVM);
            return Ok(result);
        }

        [Route("api/socialmediaurl/EditSocialMediaURL")]
        [HttpPut]
        public IActionResult EditSocialMediaURL([FromForm]SocialMediaUrlModelView urlVM)
        {
            var result = _socialMediaUrlManager.EditSocialMediaURL(_DoctorId, urlVM);
            return Ok(result);
        }
    }
}
