using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AppDev.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [Required]
        [StringLength(50)]
        public string CourseName { get; set; }

        [ForeignKey("CourseCategory")]
        public int CategoryId { get; set; }
        //Linking object
        public CourseCategory CourseCategory { get; set; }          
        [Required]
        [StringLength(255)]
        public string CourseDescription { get; set; }
    }
}