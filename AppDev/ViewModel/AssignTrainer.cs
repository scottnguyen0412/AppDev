using AppDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppDev.ViewModel
{
    public class AssignTrainer
    {
        public int TrainerId { get; set; }
        public IEnumerable<Trainner> trainners { get; set; }
        public int CourseId { get; set; }
        public IEnumerable<Course> courses { get; set; }
    }
}