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
                    return RedirectToAction("Index", "Admin");
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View(viewModel);
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