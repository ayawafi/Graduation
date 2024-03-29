﻿using AutoMapper;
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
using System.Security.Cryptography;


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
            var result = _dbContext.Specializations.Where(z => z.IsDelete == false).Select(x => new
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
            var result = _dbContext.Specializations.Where(z => z.IsDelete == false).Select(x => new
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

        public ResponseApi CreateSpecialty(string adminId,SpectalizationModelView specialtyMV)
        {
            var admId = _dbContext.Users.FirstOrDefault(c => c.Id == adminId);

            if (admId.UserType != "Admin")
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "You don't have a permission to delete",
                    Data = null
                };
                return response;
            }
            if (_dbContext.Specializations
                          .Any(a => a.SpecialtyName.Equals(specialtyMV.SpecialtyName,
                                    StringComparison.InvariantCultureIgnoreCase)))
            {
                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Specialty already exist",
                    Data = null
                };
                return response;
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
            var response1 = new ResponseApi
            {
                IsSuccess = true,
                Message = "Success",
                Data = result
            };
            return response1;
        }

        public ResponseApi UpdateSpecialty(string adminId,Specialization currentSpecialty, int specialtyId)
        {
            var admId = _dbContext.Users.FirstOrDefault(c => c.Id == adminId);

            if (admId.UserType != "Admin")
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "You don't have a permission to delete",
                    Data = null
                };
                return response;
            }
            var specialty = _dbContext.Specializations
                        .FirstOrDefault(a => a.Id == specialtyId);

            if (specialty == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "specialty not found",
                    Data = null
                };
                return response;
            }
            else
            {
            specialty.SpecialtyName = currentSpecialty.SpecialtyName;

            _dbContext.SaveChanges();
            var result = _mapper.Map<SpectalizationModelView>(specialty);

            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Success",
                Data = result
            };
            return response;
            }
        }
        public ResponseApi DeleteSpecialty(string adminId,int SpecialtyId)
        {
            var admId = _dbContext.Users.FirstOrDefault(c => c.Id == adminId);

            if(admId.UserType != "Admin")
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "You don't have a permission to delete",
                    Data = null
                };
                return response;
            }
            var specialty = _dbContext.Specializations.FirstOrDefault(a => a.Id == SpecialtyId);

            if(specialty == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "specialty not found",
                    Data = null
                };
                return response;
            }
            else
            {
             specialty.IsDelete = true;

            _dbContext.SaveChanges();
            var result = _mapper.Map<SpectalizationModelView>(specialty);

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
                    folder += Guid.NewGuid().ToString() + "_" + ImgeFile.FileName;
                    string ImageURL = "/" + folder;
                    string serverFolder = Path.Combine(_host.WebRootPath, folder);
                    ImgeFile.CopyTo(new FileStream(serverFolder, FileMode.Create));
                    return ImageURL;
                }
        #endregion Private
    }
}

