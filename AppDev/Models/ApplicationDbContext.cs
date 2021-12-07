using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace AppDev.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Trainner> trainers { get; set; }
        public DbSet<TrainningStaff> trainningStaffs { get; set; }
        public DbSet<Trainee> trainees { get; set; }
        public DbSet<Course> courses { get; set; }
        public DbSet<AssignTraineeToCourse> assignTraineeToCourses { get; set; }
        public DbSet<AssignTrainerToCourse> assignTrainerToCourses { get; set; }
        public DbSet<CourseCategory> courseCategories { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}