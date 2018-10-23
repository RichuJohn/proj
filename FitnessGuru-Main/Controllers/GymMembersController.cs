using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http.Results;
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
            // redirect to admin index page
            if (User.IsInRole(RoleName.Admin))
                return RedirectToLocal("/Admin/Index");

            // redirect to trianer index page
            if (User.IsInRole(RoleName.Trainer))
                return RedirectToLocal("/Trainers/Index");

            var userId = User.Identity.GetUserId();
            GymMember user = db.GymMembers.Where(c => c.UserId == userId).Include(s => s.JoinedSessions).FirstOrDefault();
            ViewBag.UserId = user.Id;

            ViewBag.Name = user.FirstName;

            // get the list of upcoming sessions that have not been cancelled
            //            var currentTime = DateTime.Now.ToLocalTime();

            //            var currentTime = Util.ParseDateExactForTimeZone(DateTime.Now.ToString("yyyy-MM-dd HH:mm")).DateTime;
            var currentTime = Util.ParseDateExactForTimeZone(DateTime.UtcNow);
            ViewBag.CurrentTime = currentTime.ToString("yyyy-MM-dd HH:mm");

            var sessions = db.Sessions
                                .Where(c => !c.isCancelled && (DateTime.Compare(c.SessionAt, currentTime) > 0))
                                .Include(s => s.GymMember)
                                .ToList();

            // get the list of upcoming sessions where the user hasnt yet joined
            var sessionsWithoutUser =
                sessions
                    .Where(c => !c.GymMembers.Contains(user))
                    .OrderBy(c => c.SessionAt)
                    .ToList();


            // get the list of sessions the user has joined
            var joinedSessions = user.JoinedSessions
                    .Where(c => !c.isCancelled && (DateTime.Compare(c.SessionAt, currentTime) > 0))
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


        public ActionResult ListSessionsCreated()
        {
            var userId = User.Identity.GetUserId();
            //            var currentTime = DateTime.Now.ToLocalTime();
            //            var currentTime = Util.ParseDateExactForTimeZone(DateTime.Now.ToString("yyyy-MM-dd HH:mm")).DateTime;
            var currentTime = Util.ParseDateExactForTimeZone(DateTime.UtcNow);
            var user = db.GymMembers.Where(c => c.UserId == userId).First();
            var sessions = db.Sessions.Where(c => c.TrainerId == user.Id && (DateTime.Compare(c.SessionAt, currentTime) > 0)).Include(s => s.GymMember);
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
            //            var currentTime = DateTime.Now.ToLocalTime();
            //            var currentTime = Util.ParseDateExactForTimeZone(DateTime.Now.ToString("yyyy-MM-dd HH:mm")).DateTime;
            var currentTime = Util.ParseDateExactForTimeZone(DateTime.UtcNow);
            var sessions = user.JoinedSessions.Where(c => !c.isCancelled && DateTime.Compare(c.SessionAt, currentTime) <= 0);

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
            ViewBag.UserId = user.Id;

            // todo check if the user is indeed a user joined in that session

            SessionFeedback sessionFeedback = session.SessionFeedbacks.Where(c => c.GymMemberId == user.Id).FirstOrDefault();

            SessionFeedbackViewModel sessionFeedbackViewModel = new SessionFeedbackViewModel()
            {
                session = session,
                feedback = sessionFeedback,
            };

            if (sessionFeedbackViewModel.feedback == null)
            {
                ViewBag.SessionRating = 0;
            }
            else
            {
                ViewBag.SessionRating = sessionFeedbackViewModel.feedback.Rating;
            }

            return View(sessionFeedbackViewModel);

        }


        public ActionResult CalendarView()
        {
            var UserId = User.Identity.GetUserId();
            var user = db.GymMembers.Where(c => c.UserId == UserId).FirstOrDefault();
            ViewBag.UserId = user.Id;
            return View();
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
