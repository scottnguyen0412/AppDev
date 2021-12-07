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
    //[Authorize(Roles = Roles.Admin)]
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
                var staff = new TrainningStaff
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
        public ActionResult EditStaff(string id)
        {
            //SingleOrDefault return default value is 1 if the value is empty
            var StaffinDb = _context.trainningStaffs.SingleOrDefault(s => s.StaffId == id);
            if (StaffinDb == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        //Get model from Form 
        [HttpPost]
        public ActionResult EditStaff(TrainningStaff staff)
        {
            if(!ModelState.IsValid)
            {
                return View(staff);
            }
            var StaffinDb = _context.trainningStaffs.SingleOrDefault(s => s.StaffId == staff.StaffId);
            if(StaffinDb == null)
            {
                return HttpNotFound();
            }
            StaffinDb.FullName = staff.FullName;
            StaffinDb.Age = staff.Age;
            StaffinDb.Address = staff.Address;
            return RedirectToAction(" ","Admin");
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