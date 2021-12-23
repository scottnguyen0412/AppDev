using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppDev.Models;
namespace AppDev.ViewModel
{
    public class AssignTrainee
    {
        public int TraineeId { get; set; }
        public IEnumerable<Trainee> trainee { get; set; }
        public int CourseId { get; set; }
        public IEnumerable<Course> courses { get; set; }
    }
}
