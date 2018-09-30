using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FitnessGuru_Main.Models;
using FitnessGuru_Main.utils;
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
            if (User.IsInRole(RoleName.Admin))
                return RedirectToLocal("/Admin/Index");

            if (User.IsInRole(RoleName.Trainer))
                return RedirectToLocal("/Trainers/Index");

            var userId = User.Identity.GetUserId();
            GymMember user = db.GymMembers.Where(c => c.UserId == userId).Include(s => s.JoinedSessions).FirstOrDefault();

            ViewBag.Name = user.FirstName;

            var sessions = db.Sessions
                                .Where(c => !c.isCancelled && (DateTime.Compare(c.SessionAt, DateTime.Now) > 0))
                                .Include(s => s.GymMember)
                                .ToList();

            var sessionsWithoutUser =
                //!User.IsInRole(RoleName.Trainer) ? 
                sessions
                    .Where(c => !c.GymMembers.Contains(user))
                    .OrderBy(c => c.SessionAt)
                    .ToList();
                //: sessions
                //                          .Where(c => c.TrainerId != user.Id && (DateTime.Compare(c.SessionAt, DateTime.Now) > 0))
                //                          .OrderBy(c => c.SessionAt)
                //                          .ToList();



            var joinedSessions =
                //!User.IsInRole(RoleName.Trainer) ? 
                user.JoinedSessions
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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: GymMembers/Details/5
        [Authorize(Roles = RoleName.Trainer + "," + RoleName.Admin)]
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

        //// GET: GymMembers/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: GymMembers/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,FirstName,LastName,DOB,AddressLine1,AddressLine2,ProfilePicPath,Desc,UserId,Gender")] GymMember gymMember)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.GymMembers.Add(gymMember);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(gymMember);
        //}

        //// GET: GymMembers/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    GymMember gymMember = db.GymMembers.Find(id);
        //    if (gymMember == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(gymMember);
        //}

        // POST: GymMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,DOB,AddressLine1,AddressLine2,ProfilePicPath,Desc,UserId,Gender")] GymMember gymMember)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(gymMember).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(gymMember);
        //}

        // GET: GymMembers/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    GymMember gymMember = db.GymMembers.Find(id);
        //    if (gymMember == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(gymMember);
        //}

        //// POST: GymMembers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    GymMember gymMember = db.GymMembers.Find(id);
        //    db.GymMembers.Remove(gymMember);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public ActionResult ListSessionsCreated()
        {
            var userId = User.Identity.GetUserId();
            var user = db.GymMembers.Where(c => c.UserId == userId).First();
            var sessions = db.Sessions.Where(c => c.TrainerId == user.Id && (DateTime.Compare(c.SessionAt, DateTime.Now) > 0)).Include(s => s.GymMember);
            return View(sessions.ToList());
        }

        [HttpPost]
        public ActionResult JoinSession(int id)
        {
            var userId = User.Identity.GetUserId();
            var user = db.GymMembers.Where(c => c.UserId == userId).First();
            var session = db.Sessions.Find(id);
            user.JoinedSessions.Add(session);

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            //send mail
            var salutation = user.Gender == "Male" ? "Mr." : "Miss.";
            EmailSender es = new EmailSender();
            es.Send(User.Identity.GetUserName(), salutation + user.LastName, "JoinedSession", session);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult OptOutFromSession(int id)
        {
            var userId = User.Identity.GetUserId();
            var user = db.GymMembers.Where(c => c.UserId == userId).First();
            Session session = db.Sessions.Find(id);
            user.JoinedSessions.Remove(session);

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            //send mail
            var salutation = user.Gender == "Male" ? "Mr." : "Miss.";
            EmailSender es = new EmailSender();
            es.Send(User.Identity.GetUserName(), salutation + user.LastName, "OptOutSession", session);

            return RedirectToAction("Index");
        }

        public ActionResult ShowHistory()
        {
            var userId = User.Identity.GetUserId();
            var user = db.GymMembers.Where(c => c.UserId == userId).First();

            var sessions = user.JoinedSessions.Where(c => !c.isCancelled && DateTime.Compare(c.SessionAt, DateTime.Now) <= 0);

            return View(sessions);
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
            }
            else
            {
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
