using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DbModel.Models
{
    public class FavDoctors
    {
        public int DoctorId { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Doctor Dcotor { get; set; }
    }
}
