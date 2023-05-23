using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_ModelView
{
    public class UpdateDoctorVM
    {
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
        public string Experience { get; set; }
        public int? ExperienceFrom { get; set; }
        public int? ExperienceTo { get; set; }
        public string Awards { get; set; }
        public int? AwardsYear { get; set; }
        public string Registration { get; set; }
        public int? RegistrationYear { get; set; }
        public string Membership { get; set; }

        public string Designation { get; set; }
    }
}
