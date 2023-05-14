using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_ModelView
{
    public class ScheduletimingModelView
    {
        public int Id { get; set; }
        public int TimeDuration { get; set; }
        public string Day { get; set; }
        [Timestamp]
        public DateTime? Time { get; set; }
    }
}
