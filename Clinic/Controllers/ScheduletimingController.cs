﻿using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Clinic.Controllers
{
    [Authorize]
    [ApiController]
    
    public class ScheduletimingController : BaseController
    {
        private IScheduletimingManager _scheduletimingManager;
        private readonly IHttpContextAccessor __httpContextAccessor;
        public ScheduletimingController(IScheduletimingManager scheduletimingManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _scheduletimingManager = scheduletimingManager;
            __httpContextAccessor = httpContextAccessor;
        }
        [Route("api/scheduletiming/addscheduletiming")]
        [HttpPost]
        public IActionResult AddScheduletiming([FromForm]ScheduletimingModelView scheduletiming)
        {
            try
            {
                var time = _scheduletimingManager.AddScheduletiming(_DoctorId, scheduletiming);
                return Ok(time);

            }
            catch(Exception ex)
            {
                return Ok(new ResponseApi
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = null
                });
               
            }
            
            
        }

        [Route("api/scheduletiming/getBusinessHoursForDoctor")]
        [HttpGet]
        public IActionResult GetBusinessHoursForDoctor(int doctorId)
        {
            var d = _DoctorId;
            var result = _scheduletimingManager.GetBusinessHoursForDoctor(doctorId);
            return Ok(result);
        }
        [Route("api/scheduletiming/getScheduletimingsForDoctor")]
        [HttpGet]
        public IActionResult GetScheduletimingsForDoctor(DateTime date, int doctorId)
        {
            var result = _scheduletimingManager.GetScheduletimingsForDoctor(doctorId, date);
            return Ok(result);
        }
    }
}
