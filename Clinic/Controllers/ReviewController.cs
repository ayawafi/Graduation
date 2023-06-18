using Clinic_Core.Managers.Interfaces;
using Clinic_ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    [Authorize]
    [ApiController]
    public class ReviewController : BaseController
    {
        private IReviewManager _reviewManager;
        private readonly IHttpContextAccessor __httpContextAccessor;
        public ReviewController(IReviewManager reviewManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _reviewManager = reviewManager;
            __httpContextAccessor = httpContextAccessor;
        }

        [AllowAnonymous]
        [Route("api/review/GetAllReviewsForDoctor")]
        [HttpGet]
        public IActionResult GetAllReviewsForDoctor(int doctorId)
        {
            var result = _reviewManager.GetAllReviewsForDoctor(doctorId);
            return Ok(result);
        }

        [Route("api/review/GetAllReviewsForDoctor")]
        [HttpPost]
        public IActionResult AddReview( int doctorId, [FromForm]ReviewModelView reviewVM)
        {
            var result = _reviewManager.AddReview(_DoctorId, doctorId, reviewVM);
            return Ok(result);
        }
    }
}
