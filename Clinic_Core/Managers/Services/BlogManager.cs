using AutoMapper;
using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace Clinic_Core.Managers
{
    public class BlogManager : IBlogManager
    {
        private clinic_dbContext _dbContext;
        private IMapper _mapper;
        private IWebHostEnvironment _host;

        public BlogManager(clinic_dbContext dbContext, IMapper mapper, IWebHostEnvironment host)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _host = host;
        }

        public ResponseApi CreateBlog(string DoctorId, BlogModelView blogVM)
        {
            var DId = _dbContext.Doctors.FirstOrDefault(u => u.UserId== DoctorId);
            string folder = "Uploads/BlogsImages";
            folder = UploadImage(folder, blogVM.ImageFile);
            blogVM.Image = folder;
            var blog = new Blog
                {
                    DoctorId = DId.Id,
                    Title = blogVM.Title,
                    Content = blogVM.Content,
                    Image = blogVM.Image,
                    CreatedDate = DateTime.Now
                };


                _dbContext.Blogs.Add(blog);
                _dbContext.SaveChanges();

                
                var result = _mapper.Map<BlogModelView>(blog);
           
                var response = new ResponseApi
                {
                IsSuccess = true,
                Message = "Blog is created successfully",
                Data = result
                };
                return response;

            }

        public ResponseApi GetAllBlogs()
        {
            var result = _dbContext.Blogs.Include(y => y.Doctor)
                            .ThenInclude(z => z.ApplicationUser)
                            .Select(x => new
                            {
                                x.Doctor.ApplicationUser.FirstName,
                                x.Title,
                                x.Content,
                                x.CreatedDate,
                                x.Image
                            }).ToList();

            if (!result.Any())
            {
                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Success , But Data is null",
                    Data = result
                };
                return response;
            }
            else
            {
                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Success",
                    Data = result
                };
                return response;

            }
        }

        #region Private
        private string UploadImage(string folder, IFormFile ImgeFile)
        {
            folder += Guid.NewGuid().ToString() + "_" + ImgeFile.FileName;
            string ImageURL = "/" + folder;
            string serverFolder = Path.Combine(_host.WebRootPath, folder);
            ImgeFile.CopyTo(new FileStream(serverFolder, FileMode.Create));
            return ImageURL;
        }
        #endregion Private
    }
}

