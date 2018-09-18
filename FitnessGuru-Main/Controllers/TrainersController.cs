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
    public class TrainersController : Controller
    {
        private FitnessGuruModelContainer db = new FitnessGuruModelContainer();

       
        public ActionResult History()
        {
            var TrainerId = User.Identity.GetUserId();
            var user = db.GymMembers.Where(c => c.UserId == TrainerId).FirstOrDefault();


            var sessions = user.Sessions.Where(c => !c.isCancelled && DateTime.Compare(c.SessionAt, DateTime.Now) <= 0);

            return View(sessions);
        }

        public ActionResult Feedback(int id)
        {
            var session = db.Sessions.Find(id);
            ViewBag.session = session;
            return View(session.SessionFeedbacks);
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
