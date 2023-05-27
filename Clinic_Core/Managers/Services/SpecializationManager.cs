using AutoMapper;
using Clinic_Common.Extensions;
using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;


namespace Clinic_Core.Managers.Services
{
    public class SpecializationManager : ISpecializationManager
    {
        private clinic_dbContext _dbContext;
        private IMapper _mapper;
        private IWebHostEnvironment _host;



        public SpecializationManager(clinic_dbContext dbContext, IMapper mapper, IWebHostEnvironment host)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _host = host;
        }
        public ResponseApi GetAllSpecialties()
        {
            var result = _dbContext.Specializations.Select(x => new
            {
                Id = x.Id,
                SpecialtyName = x.SpecialtyName,
                Image = x.Image
            }).ToList();

            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Done",
                Data = result
            };
            return response;
        }

        public ResponseApi GetSpecialtiesBySpecificNum(int NumberOfSpecialties)
        {
            var result = _dbContext.Specializations.Select(x => new
            {
                Id = x.Id,
                SpecialtyName = x.SpecialtyName,
                Image = x.Image
            }).Take(NumberOfSpecialties).ToList();

            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Done",
                Data = result
            };
            return response;
            
        }

        public SpectalizationModelView CreateSpecialty(SpectalizationModelView specialtyMV)
        {
            if (_dbContext.Specializations
                          .Any(a => a.SpecialtyName.Equals(specialtyMV.SpecialtyName,
                                    StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ServiceValidationException("Specialty already exist");
            }

            string folder = "Uploads/SpecialtyImage";
            folder = UploadImage(folder, specialtyMV.ImageFile);
            specialtyMV.Image = folder;


            var specialty = _dbContext.Specializations.Add(new Specialization
            {
                SpecialtyName = specialtyMV.SpecialtyName,
                Image = specialtyMV.Image
            }).Entity;

            _dbContext.SaveChanges();

            var result = _mapper.Map<SpectalizationModelView>(specialty);

            return result;
        }

        public SpectalizationModelView UpdateSpecialty(SpectalizationModelView currentSpecialty)
        {
            var specialty = _dbContext.Specializations
                        .FirstOrDefault(a => a.Id == currentSpecialty.Id)
                        ?? throw new ServiceValidationException("Specialty not found");

            specialty.SpecialtyName = currentSpecialty.SpecialtyName;

            _dbContext.SaveChanges();
            return _mapper.Map<SpectalizationModelView>(specialty);
        }
        public SpectalizationModelView DeleteSpecialty(SpectalizationModelView currentSpecialty)
        {
            var specialty = _dbContext.Specializations
                        .FirstOrDefault(a => a.Id == currentSpecialty.Id)
                        ?? throw new ServiceValidationException("specialty not exist");

            specialty.IsDelete = true;

            _dbContext.SaveChanges();
            return _mapper.Map<SpectalizationModelView>(specialty);
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

