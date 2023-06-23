using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_ModelView
{ 
    public class DoctorRegistrationModelView
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
