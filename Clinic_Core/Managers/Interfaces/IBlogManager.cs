using Clinic_DbModel.Models;
using Clinic_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Core.Managers.Interfaces
{
    public interface IBlogManager
    {
        ResponseApi CreateBlog(string DoctorId, BlogModelView blogVM);
        ResponseApi GetAllBlogs();
        ResponseApi GetBlogById(int blogId);
        ResponseApi EditBlog(string doctorId,BlogModelView currentblog,int blogId);
        ResponseApi DeleteBlog(string doctorId,int blogId);
        ResponseApi GetBlogBySpecificNum(int num);
    }
}
