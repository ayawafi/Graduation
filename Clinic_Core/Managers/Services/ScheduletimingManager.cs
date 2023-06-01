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
        private IAppointmentManager _appointmentManager;

        public ScheduletimingManager(clinic_dbContext dbContext, IMapper mapper, IAppointmentManager appointmentManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _appointmentManager = appointmentManager;
        }
        public ResponseApi AddScheduletiming(string DoctorId , ScheduletimingModelView scheduletiming)
        {
           var uId =_dbContext.Doctors.FirstOrDefault(x=>x.UserId == DoctorId);
            if(uId == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "There is no schedule ",
                    Data = null
                };
                return response;
            }
            else
            {
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
                    Message = "Success",
                    Data = newScheduletiming
                };
                return response;
            }
           
           }

        //هاي لعرض Doctor Profile/Business Hours
        public ResponseApi GetBusinessHoursForDoctor(int DoctorId)
        {
            var uId = _dbContext.Doctors.FirstOrDefault(x => x.Id == DoctorId);
            if (uId == null)
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "There is no business hours ",
                    Data = null
                };
                return response;
            }
            else
            {
                var result = _dbContext.Scheduletimings.Where(x => x.DoctorId == uId.Id)
                                                    .Select(z => new ScheduletimingVM
                                                    {
                                                        DoctorId = uId.Id,
                                                        Day = z.Day,
                                                        AvailableTime = z.StartTime.ToString("hh:mm tt") + "-" + z.EndTime.ToString("hh:mm tt")
                                                    })
                                                    .ToList();
                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Success",
                    Data = result
                };
                return response;
            }

            
            
        }
      
        public ResponseApi GetScheduletimingsForDoctor(int doctorId,string day )
        {
            var uId = _dbContext.Doctors.FirstOrDefault(x => x.Id == doctorId);
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
                DateTime date =  workHours.StartTime.Date;
                var dId = workHours.DoctorId;

                List<(string TimeOfAppointment, int IsBooked)> timeIntervals = GenerateTimeIntervals(startTime, finishTime, durationTime, date, dId);

                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Success",
                    Data = timeIntervals
                };
                return response;
            }

            
        }
        private List<(string TimeOfAppointment, int IsBooked)> GenerateTimeIntervals(DateTime start, DateTime finish, TimeSpan timeDuration, DateTime date, int dId)
        {
            _appointmentManager.BookedAppointments(dId, date);

            List<(string TimeOfAppointment, int IsBooked)> timeIntervals = new List<(string TimeOfAppointment, int IsBooked)>();

            for (DateTime current = start; current.Add(timeDuration) <= finish; current = current.Add(timeDuration))
            {
                string timeInterval = current.ToString("h:mm tt") + " - " + current.Add(timeDuration).ToString("h:mm tt");

                bool isBooked = _dbContext.Appointments.Any(c => c.StartTime == current && c.EndTime == current.Add(timeDuration));
                int bookingStatus = isBooked ? 1 : 0;

                timeIntervals.Add((timeInterval, bookingStatus));
            }

            return timeIntervals;
        }

    }
}
