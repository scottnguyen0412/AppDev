using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppDev.Models;
namespace AppDev.ViewModel
{
    public class CourseViewModel
    {
        public Course course { get; set; }

        //create list for Course Category
        public IEnumerable<CourseCategory> courseCategories { get; set; }
    }
}