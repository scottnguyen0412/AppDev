using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppDev.Models;
using AppDev.Utils;

namespace TrainingManagement.Controllers
{
    [Authorize(Roles = Roles.Trainee)]
    public class TraineeController : Controller
    {
        private ApplicationDbContext _context;
        // GET: Trainee
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            var userId = User.Identity.GetUserId();
            /*
            var todo = _context.Todos
              .Include(t => t.Category)
              .SingleOrDefault(t => t.Id == id && t.UserId == userId);
            */
            var user = _context.Users.SingleOrDefault(u => u.Id.Equals(id));
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult ShowAllCourses()
        {
            var userId = User.Identity.GetUserId();
            var courses = _context.CoursesUsers
                .Where(u => u.UsearId.Equals(userId))
                .Select(u => u.Course)
                .Tolist();
            if (courses == null)
            { return HttpNotFound();
            }
            return View(courses);                                
        }
        [HttpGet]
        public ActionResult ShowOtherTrainee(int Id)
        {
            var coursesId = Id;
            var userId = User.Identity.GetUserId();
            var users = _context.CoursesUsers
                .Where(u => u.CourseId == courseId)
                .Select(u => u.User)
                .Tolist();
            var role = _context.Roles
                .SingleOrDefault(r => r.Name.Equals(role.Trainee));
            var newusers = users
                .Where(m => m.Roles.Any(r => r.Roles.Equals(role.Id)) && m.Id != userId)
                .Tolist();
            return View(Id, newusers);
        }
    }
}