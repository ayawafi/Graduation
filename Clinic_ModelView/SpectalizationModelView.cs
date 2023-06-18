using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Clinic_ModelView
{
    public class SpectalizationModelView
    {
        public string SpecialtyName { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Image { get; set; }
    }
}
