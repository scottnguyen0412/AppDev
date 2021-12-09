using AppDev.Models;
using AppDev.Utils;
using AppDev.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
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
        public ActionResult IndexForTrainingStaff(string SearchString)
        {
            var indexTrainee = _context.trainees
                                .ToList();
            if (!string.IsNullOrEmpty(SearchString))
            {
                //age is int. Thus, using tostring() to convert int to string
                indexTrainee = indexTrainee.Where(t => t.FullName.ToLower()
                                                        .Contains(SearchString.ToLower()) ||
                                                        t.Age.ToString().Contains(SearchString.ToLower()))
                                                        .ToList();
            };
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

        [HttpGet]
        public ActionResult DeleteTrainee(int id)
        {   //Lấy userID của account đang login hiện tại
            var TraineeInDb = _context.trainees.SingleOrDefault(d => d.Id == id);
            if (TraineeInDb == null)
            {
                return HttpNotFound();
            }

            //if find out the ID then remove out of database
            _context.trainees.Remove(TraineeInDb);
            //Save again
            _context.SaveChanges();
            return RedirectToAction("IndexForTrainingStaff", "TrainningStaff");
        }

        //For Course Category
        [HttpGet]
        public ActionResult IndexForCourseCategory(string SearchString)
        {
            var CourseCategoryInDB = _context.courseCategories.ToList();
            if (!string.IsNullOrEmpty(SearchString))
            {
                CourseCategoryInDB = CourseCategoryInDB.Where(t => t.Name.ToLower().Contains(SearchString.ToLower())|| 
                                                              t.Description.ToLower().Contains(SearchString.ToLower()))
                                                             .ToList();

            };
            return View(CourseCategoryInDB);
        }

        [HttpGet]
        public ActionResult CreateCourseCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCourseCategory(CourseCategory courseCategory)
        {
            //Người dùng không nhập thì hiển thị thông báo
            if (!ModelState.IsValid)
            {
                return View(courseCategory);
            }

            var Coursecategory = new CourseCategory()
            {
                Name = courseCategory.Name,
                Description = courseCategory.Description
            };
            //sử dụng try catch để hiển thị lỗi khi tạo 2 tên trùng nhau
            try
            {
                _context.courseCategories.Add(Coursecategory);
                _context.SaveChanges();
            }
            catch (System.Exception)
            {
                ModelState.AddModelError("duplicate", "Course Category already existed");
                return View(courseCategory);
            }
            return RedirectToAction("IndexForCourseCategory","TrainningStaff");
        }

        [HttpGet]
        public ActionResult UpdateForCourseCategory(int id)
        {
            var CourseCategoryInDb = _context.courseCategories.SingleOrDefault(s => s.Id == id);
            if (CourseCategoryInDb == null)
            {
                return HttpNotFound();
            }
            return View(CourseCategoryInDb);
        }

        [HttpPost]
        public ActionResult UpdateForCourseCategory(CourseCategory courseCategory)
        {
            //nếu người dùng không nhập gì thì trả về thông báo
            if (!ModelState.IsValid)
            {
                return View(courseCategory);
            }
            //get id to post
            var CourseCategoryInDb = _context.courseCategories.SingleOrDefault(s => s.Id == courseCategory.Id);
            if (CourseCategoryInDb == null)
            {
                return HttpNotFound();
            }
            //Name in DB will same with Name of Models
            CourseCategoryInDb.Name = courseCategory.Name;
            CourseCategoryInDb.Description = courseCategory.Description;
            //save again
            _context.SaveChanges();
            return RedirectToAction("IndexForCourseCategory", "TrainningStaff");
        }

        [HttpGet]
        public ActionResult DeleteForCourseCategory(int id)
        {   
            var CourseCategoryInDb = _context.courseCategories.SingleOrDefault(d => d.Id == id);
            if (CourseCategoryInDb == null)
            {
                return HttpNotFound();
            }

            //if find out the ID then remove out of database
            _context.courseCategories.Remove(CourseCategoryInDb);
            //Save again
            _context.SaveChanges();
            return RedirectToAction("IndexForCourseCategory", "TrainningStaff");
        }

        //For Course
        [HttpGet]
        public ActionResult IndexForCourse()
        {
            var courseInDb = _context.courses.Include(c => c.CourseCategory);
            return View(courseInDb);
        }

        [HttpGet]
        public ActionResult CreateForCourse()
        {
            var CourseviewModel = new CourseViewModel
            {
                //courseCategories of viewmodel will equal courseCategories in Database
                courseCategories = _context.courseCategories.ToList()
            };
            return View(CourseviewModel);
        }

        //through viewModel get form course
        [HttpPost]
        public ActionResult CreateForCourse(CourseViewModel viewModel)
        {
            //if modelState là không có sẵn thì return view
            if (!ModelState.IsValid)
            {
                var model = new CourseViewModel
                {
                    courseCategories = _context.courseCategories.ToList(),
                    course = viewModel.course
                };
                return View(model);
            }
            var newCourse = new Course()
            {
                CourseName = viewModel.course.CourseName,
                CategoryId = viewModel.course.CategoryId,
                CourseDescription = viewModel.course.CourseDescription,
                CourseCategory = viewModel.course.CourseCategory
            };

            //Add lại vào trong database
            _context.courses.Add(newCourse);

            //Save lại 
            _context.SaveChanges();
            return RedirectToAction("IndexForCourse","TrainningStaff");
        }

        [HttpGet]
        public ActionResult UpdateForCourse(int id)
        {
            var courseInDb = _context.courses.SingleOrDefault(s => s.Id == id);
            if (courseInDb == null)
            {
                return HttpNotFound();
            }
            var newCourseViewModel = new CourseViewModel
            { 
                course = courseInDb,
                courseCategories = _context.courseCategories.ToList()
            };
            return View(newCourseViewModel);
        }

        [HttpPost]
        public ActionResult UpdateForCourse(CourseViewModel courseViewModel)
        {
            if (!ModelState.IsValid)
            {
                var NewCourseViewModel = new CourseViewModel
                {
                    course = courseViewModel.course,
                    courseCategories = _context.courseCategories.ToList()
                };
            }
            //get data in dbcontext by id
            var dataInDB = _context.courses.SingleOrDefault(d => d.Id == courseViewModel.course.Id);
            if (dataInDB == null)
            {
                return HttpNotFound();
            }

            //if find out id 
            dataInDB.CourseName = courseViewModel.course.CourseName;
            dataInDB.CourseDescription = courseViewModel.course.CourseDescription;
            dataInDB.CourseCategory = courseViewModel.course.CourseCategory;
            dataInDB.CategoryId = courseViewModel.course.CategoryId;

            _context.SaveChanges();
            return RedirectToAction("IndexForCourse", "TrainningStaff");
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