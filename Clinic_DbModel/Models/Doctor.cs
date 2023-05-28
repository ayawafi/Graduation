using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Clinic_DbModel.Models
{
    public  class Doctor
    {
        public Doctor()
        {
            Appointments = new HashSet<Appointment>();
            Blogs = new HashSet<Blog>();
            Scheduletimings = new HashSet<Scheduletiming>();
            Socialmediaurls = new HashSet<Socialmediaurl>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string AboutMe { get; set; }
        public string Pricing { get; set; }
        public string DoctorServices { get; set; }
        public string Degree { get; set; }
        public string College { get; set; }
        public string YearOfCompletion { get; set; }
        public int? SpecialtyId { get; set; }
        public string ClinicName { get; set; }
        public string ClinicAddress { get; set; }
        public string HospitalName { get; set; }
        public int? HospitalFrom { get; set; }
        public int? HospitalTo { get; set; }
        public string Designation { get; set; }
        public string Awards { get; set; }
        public int? AwardsYear { get; set; }
        public string Registration { get; set; }
        public int? RegistrationYear { get; set; }
        public string Membership { get; set; }


        [ForeignKey(nameof(UserId))]

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Specialization Specialty { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Scheduletiming> Scheduletimings { get; set; }
        public virtual ICollection<Socialmediaurl> Socialmediaurls { get; set; }
    }
}
