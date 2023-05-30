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
    public class SocialMediaUrlManager : ISocialMediaUrlManager
    {
        private clinic_dbContext _dbContext;
        private IMapper _mapper;
        public SocialMediaUrlManager(clinic_dbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ResponseApi AddSocialMediaURL(string doctorId, SocialMediaUrlModelView urlVM)
        {
            var docId = _dbContext.Doctors.FirstOrDefault(b => b.UserId == doctorId);

            var url = new Socialmediaurl
            {
                 DoctorId = docId.Id,
                 TwitterUrl = urlVM.TwitterUrl,
                 FacebookUrl = urlVM.FacebookUrl,
                 InstagramUrl = urlVM.InstagramUrl,
                 LinkedInUrl = urlVM.LinkedInUrl,
                 WebsiteUrl = urlVM.WebsiteUrl,
            };

            _dbContext.Socialmediaurls.Add(url);

            _dbContext.SaveChanges();
            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Added Successfully",
                Data = url
            };
            return response;
        }

        public ResponseApi EditSocialMediaURL(string doctorId, SocialMediaUrlModelView urlVM)
        {
            var docId = _dbContext.Doctors.FirstOrDefault(b => b.UserId == doctorId);
            var urlDoc = _dbContext.Socialmediaurls.FirstOrDefault(c => c.DoctorId == docId.Id);

                urlDoc.DoctorId = docId.Id;
                urlDoc.TwitterUrl = urlVM.TwitterUrl;
                urlDoc.FacebookUrl = urlVM.FacebookUrl;
                urlDoc.InstagramUrl = urlVM.InstagramUrl;
                urlDoc.LinkedInUrl = urlVM.LinkedInUrl;
                urlDoc.WebsiteUrl = urlVM.WebsiteUrl;
                    
            _dbContext.SaveChanges();
            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Added Successfully",
                Data = urlVM
            };
            return response;
        }

    }
}
