using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_ModelView
{
    public class UpdatePatientProfilVM
    {
        public string PhoneNumber { get; set; }

        public String Address { get; set; }
        public IFormFile ImageFile { get; set; }
       // public string Image { get; set; }
    }
}
