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
    }
}