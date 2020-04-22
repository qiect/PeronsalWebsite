﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.AdminWeb.Models
{
    public class PermissionAddModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}