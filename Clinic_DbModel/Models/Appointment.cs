using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Clinic_DbModel.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string Day { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [NotMapped]
        public string AvailableTime { get { return StartTime.ToString("hh:mm")+"-"+ EndTime.ToString("hh:mm"); } }



        public virtual Doctor Doctor { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
