using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FitnessGuru_Main.Models;
using Microsoft.AspNet.Identity;


namespace FitnessGuru_Main.Controllers
{
    [Authorize]
    public class GymMembersController : Controller
    {
        private FitnessGuruModelContainer db = new FitnessGuruModelContainer();

        // GET: GymMembers
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            GymMember user = db.GymMembers.Where(c => c.UserId == userId).Include(s => s.JoinedSessions).FirstOrDefault();

            ViewBag.Name = user.FirstName;

            var sessions = db.Sessions
                                .Where(c => !c.isCancelled && (DateTime.Compare(c.SessionAt, DateTime.Now) > 0))
                                .Include(s => s.GymMember)
                                .ToList();

            var sessionsWithoutUser = sessions
                .Where(c => !c.GymMembers.Contains(user))
                .OrderBy(c=>c.SessionAt)
                .ToList();

            var joinedSessions = user.JoinedSessions
                .Where(c => !c.isCancelled && (DateTime.Compare(c.SessionAt, DateTime.Now) > 0))
                .OrderBy(c => c.SessionAt)
                .ToList();

            var sb = new GymMemberIndexViewModel()
            {
                CompleteSessions = sessionsWithoutUser,
                JoinedSessions = joinedSessions
            };
          
            return View(sb);
        }

        // GET: GymMembers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymMember gymMember = db.GymMembers.Find(id);
            if (gymMember == null)
            {
                return HttpNotFound();
            }
            return View(gymMember);
        }

        // GET: GymMembers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GymMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,DOB,AddressLine1,AddressLine2,ProfilePicPath,Desc,UserId,Gender")] GymMember gymMember)
        {
            if (ModelState.IsValid)
            {
                db.GymMembers.Add(gymMember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gymMember);
        }

        // GET: GymMembers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymMember gymMember = db.GymMembers.Find(id);
            if (gymMember == null)
            {
                return HttpNotFound();
            }
            return View(gymMember);
        }

        // POST: GymMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,DOB,AddressLine1,AddressLine2,ProfilePicPath,Desc,UserId,Gender")] GymMember gymMember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gymMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gymMember);
        }

        // GET: GymMembers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymMember gymMember = db.GymMembers.Find(id);
            if (gymMember == null)
            {
                return HttpNotFound();
            }
            return View(gymMember);
        }

        // POST: GymMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GymMember gymMember = db.GymMembers.Find(id);
            db.GymMembers.Remove(gymMember);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ListSessionsCreated()
        {
            var userId = User.Identity.GetUserId();
            var user = db.GymMembers.Where(c => c.UserId == userId).First();
            var sessions = db.Sessions.Where(c => c.TrainerId == user.Id).Include(s => s.GymMember);
            return View(sessions.ToList());
        }

        [HttpPost]
        public ActionResult JoinSession(int id)
        {
            var userId = User.Identity.GetUserId();
            var user = db.GymMembers.Where(c => c.UserId == userId).First();
            user.JoinedSessions.Add(db.Sessions.Find(id));

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult OptOutFromSession(int id)
        {
            var userId = User.Identity.GetUserId();
            var user = db.GymMembers.Where(c => c.UserId == userId).First();
            user.JoinedSessions.Remove(db.Sessions.Find(id));

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ShowHistory()
        {
            var userId = User.Identity.GetUserId();
            var user = db.GymMembers.Where(c => c.UserId == userId).First();

            var sessions = user.JoinedSessions.Where(c => !c.isCancelled && DateTime.Compare(c.SessionAt, DateTime.Now) <= 0);

            return View(sessions);
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
