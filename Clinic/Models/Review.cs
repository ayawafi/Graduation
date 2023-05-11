using System;
using System.Collections.Generic;

#nullable disable

namespace Clinic.Models
{
    public partial class Review
    {
        public int Id { get; set; }
        public int PatientId { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
