using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FitnessGuru_Main.Models;
using FitnessGuru_Main.utils;
using Microsoft.AspNet.Identity;

namespace FitnessGuru_Main.Controllers
{
    [Authorize(Roles = RoleName.Trainer + ","+ RoleName.Admin)]
    public class SessionsController : Controller
    {
        private FitnessGuruModelContainer db = new FitnessGuruModelContainer();
        private ApplicationDbContext AppDbContext = new ApplicationDbContext();

        // GET: Sessions/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // GET: Sessions/Create
        public ActionResult Create()
        {
            var trainerId = User.Identity.GetUserId();
            var user = db.GymMembers.Where(c => c.UserId == trainerId).FirstOrDefault();

            ViewBag.Trainer = new SelectList(db.GymMembers, "Id", "FirstName");
            ViewBag.UserId = user.Id;
            return View();
        }


        // GET: Sessions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }

            IQueryable<ApplicationUser> Trainers = null;
            
            if (AppDbContext != null)
            {
                var roles = AppDbContext.Roles.Where(r => r.Name == RoleName.Trainer);
                if (roles.Any())
                {
                    var roleId = roles.First().Id;
                    Trainers = AppDbContext.Users.Where(c => c.Roles.Any(r => r.RoleId == roleId));
                }
            }

            var trainerIds = new ArrayList();

            foreach (ApplicationUser trainer in Trainers)
            {
                trainerIds.Add(trainer.Id);
            }

            var TrainersInApp = db.GymMembers.ToList().Where(c => trainerIds.Contains(c.UserId));

            
            var trainerId = User.Identity.GetUserId();
            var user = db.GymMembers.Where(c => c.UserId == trainerId).FirstOrDefault();
            ViewBag.UserId = user.Id;

            ViewBag.TrainerId = new SelectList(TrainersInApp, "Id", "FirstName", session.TrainerId);
            return View(session);
        }


        // GET: Sessions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Session session = db.Sessions.Include(c => c.GymMembers)
                .Include(c => c.GymMember)
                .Where(c => c.Id == id).FirstOrDefault();

            var sessionTemp = new Session()
            {
                GymMember = session.GymMember,
                SessionName = session.SessionName,
                SessionAt = session.SessionAt,
                TrainerId = session.TrainerId,
                Desc = session.Desc
            };

            ICollection<GymMember> joinedUsers = new List<GymMember>();
            foreach (var member in session.GymMembers)
            {
                joinedUsers.Add(member);
            }
            session.GymMembers.Clear();
            session.SessionFeedbacks.Clear();
            db.Entry(session).State = EntityState.Modified;
            db.SaveChanges();

            db.Sessions.Remove(session);
            db.SaveChanges();

            //send mail
            EmailSender es = new EmailSender();
            foreach (var member in joinedUsers)
            {
                var salutation = member.Gender == "Male" ? "Mr." : "Miss.";
                es.Send(AppDbContext.Users.Find(member.UserId).Email, salutation + member.LastName, "SessionDelete", sessionTemp);
            }

            return RedirectToAction("Index", "Trainers");
        }

        public ActionResult ListMembers(int id)
        {
            Session session = db.Sessions.Find(id);
            return View(session.GymMembers);
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
