using System;
using System.Collections.Generic;

#nullable disable

namespace Clinic.Models
{
    public partial class Blog
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public byte? IsDeleted { get; set; }

        public virtual Doctor Doctor { get; set; }
    }
}
