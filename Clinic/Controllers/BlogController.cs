using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    [Authorize]
    [ApiController]
    public class BlogController : BaseController
    {
        private IBlogManager _blogManager;
        private readonly IHttpContextAccessor __httpContextAccessor;
        public BlogController(IBlogManager blogManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _blogManager = blogManager;
            __httpContextAccessor = httpContextAccessor;
        }
        [Route("api/blog/createblog")]
        [HttpPost]
        public IActionResult CreateBlog(int DoctorId,[FromForm]BlogModelView blogVM)
        {
            var result = _blogManager.CreateBlog(DoctorId, blogVM);
            return Ok(result);
        }
        [Route("api/blog/getallblogs")]
        [HttpGet]
        public IActionResult GetAllBlogs()
        {
            var result = _blogManager.GetAllBlogs();
            return Ok(result);

        }
    }
}
