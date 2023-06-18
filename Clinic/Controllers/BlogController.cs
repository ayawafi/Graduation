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
        public IActionResult CreateBlog([FromForm]BlogModelView blogVM)
        {
            var result = _blogManager.CreateBlog(_DoctorId,blogVM);
            return Ok(result);
        }
        [AllowAnonymous]
        [Route("api/blog/getallblogs")]
        [HttpGet]
        public IActionResult GetAllBlogs()
        {
            var result = _blogManager.GetAllBlogs();
            return Ok(result);

        }

        [Route("api/blog/getblogbyid")]
        [HttpGet]
        public IActionResult GetBlogById(int blogId)
        {
            var result = _blogManager.GetBlogById(blogId);
            return Ok(result);

        }
        [AllowAnonymous]
        [Route("api/blog/getblogbyspecificnum")]
        [HttpGet]
        public IActionResult GetBlogBySpecificNum(int num)
        {
            var result = _blogManager.GetBlogBySpecificNum(num);
            return Ok(result);
        }

        [Route("api/blog/EditBlog")]
        [HttpPut]
        public IActionResult EditBlog([FromForm]BlogModelView currentblog, int blogId)
        {
            var result = _blogManager.EditBlog(_DoctorId, currentblog, blogId);
            return Ok(result);

        }

        [Route("api/blog/DeleteBlog")]
        [HttpDelete]
        public IActionResult DeleteBlog(int blogId)
        {
            var result = _blogManager.DeleteBlog(_DoctorId, blogId);
            return Ok(result);

        }
    }
}
