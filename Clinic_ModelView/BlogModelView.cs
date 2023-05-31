﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_ModelView
{
    public class BlogModelView
    {
       
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public IFormFile ImageFile { get; set; }
        
    }
}
