using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppDev.Models;
using AppDev.Utils;
using Microsoft.AspNet.Identity;

namespace AppDev.Controllers
{
    public class IndexforTrainerandTraineeController : Controller
    {
        // GET: IndexForTrainerAndTrainee
        private ApplicationDbContext _context;

        public IndexforTrainerandTraineeController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: IndexforTrainerandTrainee
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = Roles.Trainner)]
        public ActionResult HomeOfTrainer()
        {
            var userId = User.Identity.GetUserId();
            var trainerInDb = _context.trainers.SingleOrDefault(t => t.TrainerId == userId);
            return View(trainerInDb);
        }
        [HttpGet]
        [Authorize(Roles = Roles.Trainner)]
        public ActionResult UpdateProfileOfTrainer(int id)
        {
            var traineeInDb = _context.trainers.SingleOrDefault(t => t.Id == id);
            if (traineeInDb == null)
            {
                return HttpNotFound();
            }
            return View(traineeInDb);
        }
    }
}