﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnessGuru_Main.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data;
using System.Data.Entity;
using System.Net;

namespace FitnessGuru_Main.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class AdminController : Controller
    {
        ApplicationDbContext userscontext = new ApplicationDbContext();
        private FitnessGuruModelContainer db = new FitnessGuruModelContainer();
        private UserStore<ApplicationUser> userStore;
        private UserManager<ApplicationUser> userManager;
        private RoleStore<IdentityRole> roleStore;
        private RoleManager<IdentityRole> roleManager;


        public AdminController()
        {
            userStore = new UserStore<ApplicationUser>(userscontext);
            userManager = new UserManager<ApplicationUser>(userStore);
            roleStore = new RoleStore<IdentityRole>(userscontext);
            roleManager = new RoleManager<IdentityRole>(roleStore);
        }

        // GET: Admin
        public ActionResult Index()
        {
            var members = db.GymMembers;
            return View(members.ToList());
        }

        public ActionResult Details(int? id)
        {
            GymMember user = db.GymMembers.Find(id);
            return View(user);
        }


        public ActionResult AddAsTrainer(string UserId)
        {
            if (!roleManager.RoleExists(RoleName.Trainer))
            {
                roleManager.Create(new IdentityRole(RoleName.Trainer));
            }

            GymMember user = db.GymMembers.Where(c => c.UserId == UserId).Include(s => s.JoinedSessions).FirstOrDefault();
            if (!userManager.IsInRole(UserId, RoleName.Trainer))
            {
                userManager.AddToRole(UserId, RoleName.Trainer);
                
            }
            return RedirectToAction("Details", new { id = user.Id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}