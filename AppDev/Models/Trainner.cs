using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AppDev.Models
{
    public class Trainner
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Let's fill out the information")]
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Specialty { get; set; } 
        public int Age { get; set; } 
        public string Address { get; set; }

        //create foreignkey with table user
        [ForeignKey("User")]
        public string TrainerId { get; set; }
        public ApplicationUser User { get; set; }
    }
}