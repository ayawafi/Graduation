using AutoMapper;
using Clinic_Common.Extensions;
using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;



namespace Clinic_Core.Managers.Services
{
    public class ScheduletimingManager : IScheduletimingManager
    {
        private clinic_dbContext _dbContext;
        private IMapper _mapper;

        public ScheduletimingManager(clinic_dbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public ResponseApi AddScheduletiming(string DoctorId , ScheduletimingModelView scheduletiming)
        {
         var uId=   _dbContext.Doctors.FirstOrDefault(x=>x.UserId==DoctorId);
            var newScheduletiming = new ScheduletimingModelView
            {
                DoctorId = uId.Id,
                Day = scheduletiming.Day,
                StartTime = scheduletiming.StartTime,
                EndTime = scheduletiming.EndTime,
                DurationTime = scheduletiming.DurationTime,
            };
            var model = _mapper.Map<Scheduletiming>(newScheduletiming);
            _dbContext.Scheduletimings.Add(model);
            _dbContext.SaveChanges();

            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Success , But Doctor was exist",
                Data = model
            };
            return response;
           }

        //هاي لعرض Doctor Profile/Business Hours
        public ResponseApi GetBusinessHoursForDoctor(string DoctorId)
        {
            var uId = _dbContext.Doctors.FirstOrDefault(x => x.UserId == DoctorId);
            var result = _dbContext.Scheduletimings.Where(x => x.DoctorId == uId.Id)
                                                    .Select(z => new ScheduletimingVM { 
                                                        Day = z.Day,
                                                        AvailableTime =z.StartTime.ToString("hh:mm tt") + "-" + z.EndTime.ToString("hh:mm tt") })
                                                    .ToList();
            var response = new ResponseApi
            {
                IsSuccess = true,
                Message = "Success",
                Data = result
            };
            return response;
            
        }
      
        public ResponseApi GetScheduletimingsForDoctor(string doctorId,string day )
        {
            var uId = _dbContext.Doctors.FirstOrDefault(x => x.UserId == doctorId);
            var workHours = _dbContext.Scheduletimings.Where(z => z.DoctorId == uId.Id)
                                                        .FirstOrDefault(x => x.Day == day); 

            if (workHours == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "Work hours not found in the database.",
                    Data = null
                };
                return response;

            }else
            {
                DateTime startTime = workHours.StartTime;
                DateTime finishTime = workHours.EndTime;
                TimeSpan durationTime = TimeSpan.FromMinutes(workHours.DurationTime);

                List<string> timeIntervals = GenerateTimeIntervals(startTime, finishTime, durationTime);

                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Success",
                    Data = timeIntervals
                };
                return response;
            }

            
        }
        private List<string> GenerateTimeIntervals(DateTime start, DateTime finish, TimeSpan timeDuration)
        {
            List<string> timeIntervals = new List<string>();

            for (DateTime current = start; current.Add(timeDuration) <= finish; current = current.Add(timeDuration))
            {
                string timeInterval = current.ToString("h:mm tt") + " - " + current.Add(timeDuration).ToString("h:mm tt");
                timeIntervals.Add(timeInterval);
            }

            return timeIntervals;

        }
    }
}
