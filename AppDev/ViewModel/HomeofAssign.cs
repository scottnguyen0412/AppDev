using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppDev.Models;
namespace AppDev.ViewModel
{
    public class HomeofAssign
    {
        public Course course { get; set; }
        public IEnumerable<Trainee> trainees { get; set; }
        public IEnumerable<Trainner> trainers { get; set; }
    }
}