using System;
using System.Collections.Generic;

#nullable disable

namespace Clinic_DbModel.Models
{
    public  class Scheduletiming
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int TimeDuration { get; set; }
        public string Day { get; set; }
        public DateTime? Time { get; set; }

        public virtual Doctor Doctor { get; set; }
    }
}
