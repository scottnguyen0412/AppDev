using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AppDev.Models
{
    public class AssignTrainerToCourse
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Trainer")]
        public int TrainerId { get; set; }
        public Trainner Trainer { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}