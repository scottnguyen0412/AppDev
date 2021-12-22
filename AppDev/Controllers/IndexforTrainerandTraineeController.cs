﻿using System;
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
        [HttpPost]
        [Authorize(Roles = Roles.Trainner)]
        public ActionResult UpdateProfileOfTrainer(Trainner trainner)
        {
            if (!ModelState.IsValid)
            {
                return View(trainner);
            }
            var trainerInDb = _context.trainers.SingleOrDefault(t => t.Id == trainner.Id);
            if (trainerInDb == null)
            {
                return HttpNotFound();
            }
            trainerInDb.FullName = trainner.FullName;
            trainerInDb.Age = trainner.Age;
            trainerInDb.Address = trainner.Address;
            trainerInDb.Specialty = trainner.Specialty;

            _context.SaveChanges();
            return RedirectToAction("HomeOfTrainer", "");
        }
        [HttpGet]
        [Authorize(Roles = Roles.Trainner)]
        public ActionResult ViewCourse()
        {
            //get courseCategories
            var courseCategoryInDB = _context.courseCategories.ToList();

            var userId = User.Identity.GetUserId();
            var courseInDB = _context.assignTrainerToCourses
                                .Where(t => t.Trainer.TrainerId == userId)
                                .Select(c => c.Course).ToList();
            return View(courseInDB);

        }
    }
    }