using AutoMapper;
using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Core.Managers.Services
{
    public class ReviewManager : IReviewManager
    {
        private clinic_dbContext _dbContext;
        private IMapper _mapper;
        public ReviewManager(clinic_dbContext dbContext, IMapper mapper)
        {
                _dbContext = dbContext;
                _mapper = mapper;
        }

        public ResponseApi AddReview(string userId , int doctorId , ReviewModelView reviewVM)
        {
            var userID = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
            var doctorID = _dbContext.Doctors.FirstOrDefault(z => z.Id == doctorId);
            
            if(userID == null || doctorID == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "Please enter valid id",
                    Data = null
                };
                return response;

            }
            else
            {
                var review = new Review
                {
                    UserId = userId,
                    DoctorId = doctorId,
                    Comment = reviewVM.Comment,
                    CreatedDate = DateTime.Now
                };

                _dbContext.Reviews.Add(review);
                _dbContext.SaveChanges();

                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Successfully Created",
                    Data = null
                };
                return response;
            }

        }

        public ResponseApi GetAllReviewsForDoctor(int doctorId)
        {
            var doctorID = _dbContext.Doctors.FirstOrDefault(v => v.Id == doctorId);

            if(doctorID == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "Please enter valid id",
                    Data = null
                };
                return response;
            }
            else
            {
                var review = _dbContext.Reviews.Where(x => x.DoctorId == doctorId)
                                               .Select(x => new
                                               {
                                                   PatientId = x.UserId,
                                                   PatientName = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName,
                                                   PatientImage = x.ApplicationUser.Image,
                                                   Review = x.Comment,
                                                   CreatdDate = x.CreatedDate.ToString("yyyy MM dd")
                                               }).ToList();
                if (!review.Any())
                {
                    var response = new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "This Doctor doesn't has any review",
                        Data = null
                    };
                    return response;
                }
                else
                {
                    var response = new ResponseApi
                    {
                        IsSuccess = true,
                        Message = "Successfully Added",
                        Data = review
                    };
                    return response;
                }
            }
        }

    }
}
