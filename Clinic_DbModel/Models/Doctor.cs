using System;
using System.Collections.Generic;

#nullable disable

namespace Clinic_DbModel.Models
{
    public partial class Doctor
    {
        public Doctor()
        {
            Appointments = new HashSet<Appointment>();
            Blogs = new HashSet<Blog>();
            Clinics = new HashSet<Clinic>();
            Scheduletimings = new HashSet<Scheduletiming>();
            Socialmediaurls = new HashSet<Socialmediaurl>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string AboutMe { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string DoctorSpecialization { get; set; }
        public string DoctorServices { get; set; }
        public string DoctorImage { get; set; }
        public string Degree { get; set; }
        public string College { get; set; }
        public string YearOfCompletion { get; set; }
        public int? SpecialtyId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public virtual Specialization Specialty { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Clinic> Clinics { get; set; }
        public virtual ICollection<Scheduletiming> Scheduletimings { get; set; }
        public virtual ICollection<Socialmediaurl> Socialmediaurls { get; set; }
    }
}
