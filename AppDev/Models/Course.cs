using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppDev.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }

        public int CategoryId { get; set; }
        public string CourseCategory { get; set; }

        [Required]
        [StringLength(255)]
        public string CourseDescription { get; set; }
    }
}