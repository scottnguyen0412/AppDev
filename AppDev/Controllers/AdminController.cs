using AppDev.Models;
using AppDev.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using AppDev.ViewModel;

namespace AppDev.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        // GET: Admin
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _context;
        public AdminController()
        {
            _context = new ApplicationDbContext();
        }
        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CreateTrainingStaff()
        {
            return View();
        }

        // POST: /Account/Register through viewModel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTrainingStaff(RegisterForStaffViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = viewModel.Email, Email = viewModel.Email };
                var result = await UserManager.CreateAsync(user, viewModel.Password);
                var stafId = user.Id;
                var staff = new TrainningStaff()
                { 
                    StaffId = stafId,
                    FullName = viewModel.FullName,
                    Email = viewModel.Email,
                    Age = viewModel.Age,
                    Address = viewModel.Address
                };
                if (result.Succeeded)
                {
                    //if create success then add staff to dbcontext
                    await UserManager.AddToRoleAsync(user.Id, Roles.TrainingStaff);
                    _context.trainningStaffs.Add(staff);
                    _context.SaveChanges();
                    return RedirectToAction("IndexForStaff", "Admin");
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }
        [HttpGet]
        public ActionResult IndexForStaff()
        {
            var indexStaff = _context.trainningStaffs.ToList();
            return View(indexStaff);
        }
        [HttpGet]
        public ActionResult EditStaff(int id)
        {
            //SingleOrDefault return default value is 1 if the value is empty
            //get id of trainingStaffs Models
            var StaffinDb = _context.trainningStaffs.SingleOrDefault(s => s.Id == id);
            if (StaffinDb == null)
            {
                return HttpNotFound();
            }
            return View(StaffinDb);
        }

        //Get model from Form 
        [HttpPost]
        public ActionResult EditStaff(TrainningStaff staff)
        {
            if(!ModelState.IsValid)
            {
                return View(staff);
            }
            //get id to post
            var StaffinDb = _context.trainningStaffs.SingleOrDefault(s => s.Id == staff.Id);
            if(StaffinDb == null)
            {
                return HttpNotFound();
            }
            StaffinDb.FullName = staff.FullName;
            StaffinDb.Age = staff.Age;
            StaffinDb.Address = staff.Address;

            _context.SaveChanges();
            return RedirectToAction("IndexForStaff","Admin");
        }

        //var staffuser = _context.Users.SingleOrDefault(i => i.Id == id); clear user in database
        [HttpGet]
        public ActionResult DeleteStaff(int id)
        {   //Lấy userID của account đang login hiện tại
            var staffInDb = _context.trainningStaffs.SingleOrDefault(d => d.Id == id);
            if (staffInDb == null)
            {
                return HttpNotFound();
            }

            //if find out the ID then remove out of database
            _context.trainningStaffs.Remove(staffInDb);
            //Save again
            _context.SaveChanges();
            return RedirectToAction("IndexForStaff", "Admin");
        }

        [HttpGet]
        public ActionResult StaffDetails(string id)
        { 
            //get inform by PK staffId
            var StaffInDb = _context.trainningStaffs.SingleOrDefault(t => t.StaffId == id);

            if (StaffInDb == null)
            {
                return HttpNotFound();
            }
            return View(StaffInDb);
        }

       
        [HttpGet]
        public ActionResult ChangePasswordForStaff()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePasswordForStaff(ChangingPasswordViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                
            }
            return View();
        }

        [HttpGet]
        public ActionResult IndexForTrainer()
        {
            var indexTrainer = _context.trainers.ToList();
            return View(indexTrainer);
        }

        [HttpGet]
        public ActionResult CreateTrainer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTrainer(RegisterForTrainerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = viewModel.Email, Email = viewModel.Email };
                var result = await UserManager.CreateAsync(user, viewModel.Password);
                var trainerId = user.Id;
                var trainer = new Trainner()
                {
                    TrainerId = trainerId,
                    FullName = viewModel.FullName,
                    Email = viewModel.Email,
                    Specialty = viewModel.Specialty,
                    Age = viewModel.Age,
                    Address = viewModel.Address
                };
                if (result.Succeeded)
                {
                    //if create success then add trainer to dbcontext
                    await UserManager.AddToRoleAsync(user.Id, Roles.Trainner);
                    _context.trainers.Add(trainer);
                    _context.SaveChanges();
                    return RedirectToAction("IndexForTrainer", "Admin");
                }
                AddErrors(result);
            }
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult TrainerDetails(string id)
        {
            //get inform by PK TrainerId
            var TrainerInDb = _context.trainers.SingleOrDefault(t => t.TrainerId == id);

            if (TrainerInDb == null)
            {
                return HttpNotFound();
            }
            return View(TrainerInDb);
        }

        [HttpGet]
        public ActionResult EditTrainer(int id)
        {
            //SingleOrDefault return default value is 1 if the value is empty
            //get id of Trainers Models
            var TrainerinDb = _context.trainers.SingleOrDefault(s => s.Id == id);
            if (TrainerinDb == null)
            {
                return HttpNotFound();
            }
            return View(TrainerinDb);
        }

        //Get model from Form 
        [HttpPost]
        public ActionResult EditTrainer(Trainner trainner)
        {
            if (!ModelState.IsValid)
            {
                return View(trainner);
            }
            //get id to post
            var TrainerinDb = _context.trainers.SingleOrDefault(s => s.Id == trainner.Id);
            if (TrainerinDb == null)
            {
                return HttpNotFound();
            }
            TrainerinDb.FullName = trainner.FullName;
            TrainerinDb.Specialty = trainner.Specialty;
            TrainerinDb.Age = trainner.Age;
            TrainerinDb.Address = trainner.Address;

            _context.SaveChanges();
            return RedirectToAction("IndexForTrainer", "Admin");
        }

        [HttpGet]
        public ActionResult DeleteTrainer(int id)
        {   //Lấy userID của account đang login hiện tại
            var trainerInDb = _context.trainers.SingleOrDefault(d => d.Id == id);
            if (trainerInDb == null)
            {
                return HttpNotFound();
            }

            //if find out the ID then remove out of database
            _context.trainers.Remove(trainerInDb);
            //Save again
            _context.SaveChanges();
            return RedirectToAction("IndexForTrainer", "Admin");
        }

        [HttpGet]
        public ActionResult ChangePasswordForTrainer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePasswordForTrainer(ChangingPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

            }
            return View();
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