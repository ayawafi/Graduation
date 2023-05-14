using System;
using System.Collections.Generic;

#nullable disable

namespace Clinic_DbModel.Models
{
    public partial class Clinic
    {
        public int Id { get; set; }
        public string ClinicName { get; set; }
        public int DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }
    }
}
