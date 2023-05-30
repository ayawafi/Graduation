using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_ModelView
{
    public class PatientProfileSettings : ApplicationUserVM
    {
        //public DateTime? DateOfBirth { get; set; }
        public string BloodGroup { get; set; }
        public string Address { get; set; }
        public IFormFile ImageFile { get; set; }


    }
}
