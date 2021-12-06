using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppDev.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseCategory { get; set; }
        public string CourseDescription { get; set; }
    }
}