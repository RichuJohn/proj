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
    public class SessionsController : Controller
    {
        private FitnessGuruModelContainer db = new FitnessGuruModelContainer();

        // GET: Sessions
        public ActionResult Index()
        {
            var sessions = db.Sessions.Include(s => s.GymMember);
            return View(sessions.ToList());
        }

        // GET: Sessions/Details/5
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
            ViewBag.TrainerId = new SelectList(db.GymMembers, "Id", "FirstName");
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SessionCreateViewModel model)
        {
            var trainerId = User.Identity.GetUserId();
            var user = db.GymMembers.Where(c => c.UserId == trainerId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var session = new Session()
                {
                    SessionName = model.SessionName,
                    SessionAt = model.SessionAt,
                    Desc = model.Desc,
                    TrainerId = user.Id,
                };
                db.Sessions.Add(session);
                db.SaveChanges();
                return RedirectToAction("ListSessionsCreated", "GymMembers");
            }

            ViewBag.TrainerId = new SelectList(db.GymMembers, "Id", "FirstName", trainerId);
            return View(model);
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
            ViewBag.TrainerId = new SelectList(db.GymMembers, "Id", "FirstName", session.TrainerId);
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SessionName,SessionAt,isCancelled,Desc,TrainerId")] Session session)
        {
            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListSessionsCreated", "GymMembers");
            }
            ViewBag.TrainerId = new SelectList(db.GymMembers, "Id", "FirstName", session.TrainerId);
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
            Session session = db.Sessions.Find(id);
            db.Sessions.Remove(session);
            db.SaveChanges();
            return RedirectToAction("ListSessionsCreated", "GymMembers");
        }

        public ActionResult Feedback(int? id)
        {
            ViewBag.FeedbackStatus = "Please tell us how you felt";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }

            var gymMemberId = User.Identity.GetUserId();
            var user = db.GymMembers.Where(c => c.UserId == gymMemberId).FirstOrDefault();

            // todo check if the user is indeed a user joined in that session

            SessionFeedback sessionFeedback = session.SessionFeedbacks.Where(c => c.GymMemberId == user.Id).FirstOrDefault();

            SessionFeedbackViewModel sessionFeedbackViewModel = new SessionFeedbackViewModel()
            {
                session = session,
                feedback = sessionFeedback,
            };

            return View(sessionFeedbackViewModel);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Feedback(SessionFeedbackViewModel model)
        {

            ViewBag.FeedbackStatus = "Thankyou.. Your feedback has been duely noted";

            var gymMemberId = User.Identity.GetUserId();
            var user = db.GymMembers.Where(c => c.UserId == gymMemberId).FirstOrDefault();

            Session session = db.Sessions.Find(model.session.Id);
            SessionFeedback sessionFeedback = new SessionFeedback()
            {
                GymMemberId = user.Id,
                SessionId = model.session.Id,
                Desc = model.feedback.Desc,
                Rating = model.feedback.Rating,
            };

            // see if the user already provided a feedback
            var userFeedbackForSession = session.SessionFeedbacks.Where(c => c.GymMemberId == user.Id).FirstOrDefault();

            if (userFeedbackForSession != null)
            {
                //session.SessionFeedbacks.Remove(userFeedbackForSession);
                //db.Entry(session).State = EntityState.Modified;
                //db.SaveChanges();
                //session.SessionFeedbacks.Add(sessionFeedback);
               
                userFeedbackForSession.Desc = sessionFeedback.Desc;
                userFeedbackForSession.Rating = sessionFeedback.Rating;
            } else {
                session.SessionFeedbacks.Add(sessionFeedback);
            }

            db.Entry(session).State = EntityState.Modified;
            db.SaveChanges();

            SessionFeedbackViewModel sessionFeedbackViewModel = new SessionFeedbackViewModel()
            {
                session = session,
                feedback = session.SessionFeedbacks.Where(c => c.GymMemberId == user.Id).FirstOrDefault(),
            };
            return View(sessionFeedbackViewModel);
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
