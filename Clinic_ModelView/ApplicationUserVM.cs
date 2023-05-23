using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_ModelView
{
    public class ApplicationUserVM
    {
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Image { get; set; }

        public string UserName { get; set; }
        public string PhoneNumber { get; set; }


    }
}
