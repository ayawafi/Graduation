using AutoMapper;
using Clinic_Common.Extensions;
using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Core.Managers
{
    public class BlogManager : IBlogManager
    {
        private clinic_dbContext _dbContext;
        private IMapper _mapper;

        public BlogManager(clinic_dbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public BlogModelView CreateBlog(int DoctorId, BlogModelView blogVM)
        {
             if (!_dbContext.Doctors.Any(d => d.Id == DoctorId))
                {
                   throw new ServiceValidationException(300,"Invalid Doctor");
                }

                var blog = new Blog
                {
                    DoctorId = DoctorId,
                    Title = blogVM.Title,
                    Content = blogVM.Content,
                    Image = blogVM.Image,
                    CreatedDate = blogVM.CreatedDate
                };

                _dbContext.Blogs.Add(blog);
                _dbContext.SaveChanges();

                var result = _mapper.Map<BlogModelView>(blog);
                return result;
            }

        public List<Blog> GetAllBlogs()
        {
            var result = _dbContext.Blogs.ToList();

            return result;
        }
    }
    }

