using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_ModelView
{
    public class ScheduletimingVM
    {
        public int DoctorId { get; set; }
        public string Day { get; set; }
     
        public string AvailableTime { get; set; }


    }
}
