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
            var Image = folder;
            var blog = new Blog
                {
                    DoctorId = DId.Id,
                    Title = blogVM.Title,
                    Content = blogVM.Content,
                    Image = Image,
                    CreatedDate = DateTime.Now
                };

                _dbContext.Blogs.Add(blog);
                _dbContext.SaveChanges();
                var result = _mapper.Map<BlogModelView>(blog);
                var response = new ResponseApi
                {
                IsSuccess = true,
                Message = "Blog is created successfully",
                Data = new
                {
                    DoctorName = _dbContext.Users.Where(z => z.Id == DoctorId).Select(x => x.FirstName+" "+x.LastName),
                    Title = blogVM.Title,
                    Content = blogVM.Content,
                    Image = Image,
                    CreatedDate = DateTime.Now
                }
                };
                return response;
            }

        public ResponseApi GetAllBlogs()
        {
            var result = _dbContext.Blogs
                            .Select(x => new
                            {
                                name = x.Doctor.ApplicationUser.FirstName+" "+ x.Doctor.ApplicationUser.LastName,
                                Title = x.Title,
                                Content = x.Content,
                                CreatedDate = x.CreatedDate,
                                BlogImage = x.Image,
                                DoctorImage = x.Doctor.ApplicationUser.Image
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

        public ResponseApi GetBlogBySpecificNum(int num)
        {
            var result = _dbContext.Blogs
                            .Select(x => new
                            {
                                name = x.Doctor.ApplicationUser.FirstName + " " + x.Doctor.ApplicationUser.LastName,
                                Title = x.Title,
                                Content = x.Content,
                                CreatedDate = x.CreatedDate,
                                BlogImage = x.Image,
                                DoctorImage = x.Doctor.ApplicationUser.Image
                            }).Take(num).ToList();

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
        public ResponseApi GetBlogById(int blogId)
        {
            var blog = _dbContext.Blogs.FirstOrDefault(x => x.Id == blogId);
            if(blog == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "Blog dosen't exist",
                    Data = null
                };
                return response;
            }
            else
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "Blog dosen't exist",
                    Data = blog
                };
                return response;
            }
        }

        public ResponseApi EditBlog(string doctorId,BlogModelView currentblog, int blogId)
        {
            var DocId = _dbContext.Doctors.FirstOrDefault(c => c.UserId == doctorId);
            var blog = _dbContext.Blogs
                    .FirstOrDefault(a => a.DoctorId == DocId.Id && a.Id == blogId);
            string folder = "Uploads/BlogsImages";
            folder = UploadImage(folder, currentblog.ImageFile);
             var Image = folder;
            if (blog == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "Blog dosen't exist",
                    Data = null
                };
                return response;
            }
            else
            {
                blog.Title = currentblog.Title;
                blog.Content = currentblog.Content;
                blog.Image = Image;
                blog.CreatedDate = DateTime.Now;

                _dbContext.SaveChanges();
                var result = _mapper.Map<BlogModelView>(blog);

                var respnse = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "blog updated successfully",
                    Data = new
                    {
                        Title = blog.Title,
                        Content =blog.Content,
                        Image = Image,
                        CreatedDate = DateTime.Now
                    }
                };
                return respnse;
            }
        }

        public ResponseApi DeleteBlog(string doctorId, int blogId)
        {
            var DocId = _dbContext.Doctors.FirstOrDefault(c => c.UserId == doctorId);
            var blog = _dbContext.Blogs.FirstOrDefault(a => a.DoctorId == DocId.Id && a.Id == blogId);
            if(blog == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "blog doesn't exist",
                    Data = null
                };
                return response;
            }else
            {
                blog.IsDeleted = true;
                _dbContext.SaveChanges();
                var result = _mapper.Map<BlogModelView>(blog);

                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Successfully Deleted ",
                    Data = result
                };
                return response;
            }
        }

        #region Private
        private string UploadImage(string folder, IFormFile ImgeFile)
        {
            folder += Guid.NewGuid().ToString().Substring(0,10) + "_" + ImgeFile.FileName;
            string ImageURL = "/" + folder;
            string serverFolder = Path.Combine(_host.WebRootPath, folder);
            ImgeFile.CopyTo(new FileStream(serverFolder, FileMode.Create));
            return ImageURL;
        }
        #endregion Private
    }
}

