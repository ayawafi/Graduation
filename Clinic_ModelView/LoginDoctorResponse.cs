﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_ModelView
{
    public class LoginDoctorResponse
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
