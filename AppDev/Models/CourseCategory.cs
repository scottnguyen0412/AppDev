using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppDev.Models
{
    public class CourseCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }
    }
}