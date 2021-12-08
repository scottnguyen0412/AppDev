﻿using AppDev.Models;
using AppDev.Utils;
using AppDev.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace AppDev.Controllers
{
    [Authorize(Roles = Roles.TrainingStaff)]
    public class TrainningStaffController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _context;
        public TrainningStaffController()
        {
            _context = new ApplicationDbContext();
        }
        public TrainningStaffController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: TrainningStaff
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateTrainee()
        {
            return View();
        }
        // POST: /Account/Register through viewModel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTrainee(RegisterForTraineeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = viewModel.Email, Email = viewModel.Email };
                var result = await UserManager.CreateAsync(user, viewModel.Password);
                var traineeId = user.Id;
                var trainee = new Trainee()
                {
                    TraineeId = traineeId,
                    FullName = viewModel.FullName,
                    Email = viewModel.Email,
                    Age = viewModel.Age,
                    DateOfBirth = viewModel.DateOfBirth,
                    Education = viewModel.Education
                };
                if (result.Succeeded)
                {
                    //if create success then add staff to dbcontext
                    await UserManager.AddToRoleAsync(user.Id, Roles.Trainee);
                    _context.trainees.Add(trainee);
                    _context.SaveChanges();
                    return RedirectToAction("IndexForTrainingStaff", "TrainningStaff");
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult IndexForTrainingStaff()
        {
            var indexTrainee = _context.trainees.ToList();
            return View(indexTrainee);
        }

        [HttpGet]
        public ActionResult UpdateForTrainee(int id)
        {
            var TraineeInDb = _context.trainees.SingleOrDefault(s => s.Id == id);
            if (TraineeInDb == null)
            {
                return HttpNotFound();
            }
            return View(TraineeInDb);
        }

        [HttpPost]
        public ActionResult UpdateForTrainee(Trainee trainee)
        {
            if (!ModelState.IsValid)
            {
                return View(trainee);
            }
            //get id to post
            var TraineeInDb = _context.trainees.SingleOrDefault(s => s.Id == trainee.Id);
            if (TraineeInDb == null)
            {
                return HttpNotFound();
            }
            TraineeInDb.FullName = trainee.FullName;
            TraineeInDb.Age = trainee.Age;
            TraineeInDb.DateOfBirth = trainee.DateOfBirth;
            TraineeInDb.Education = trainee.Education;

            _context.SaveChanges();
            return RedirectToAction("IndexForTrainingStaff", "TrainningStaff");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}