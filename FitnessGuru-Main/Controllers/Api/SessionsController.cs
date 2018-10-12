using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Services.Protocols;
using System.Web.UI;
using AutoMapper;
using FitnessGuru_Main.Dtos;
using FitnessGuru_Main.Models;
using FitnessGuru_Main.utils;

namespace FitnessGuru_Main.Controllers.Api
{
    public class SessionsController : ApiController
    {

        private FitnessGuruModelContainer db;
        private ApplicationDbContext AppDbContext;

        public SessionsController()
        {
            db = new FitnessGuruModelContainer();
            AppDbContext = new ApplicationDbContext();
        }

        [HttpGet]
        public IEnumerable<SessionCalendarDto> GetUpcomingSessionsForCalendar()
        {
            return db.Sessions
                .Where(c => !c.isCancelled && (DateTime.Compare(c.SessionAt, DateTime.Now) > 0))
                .ToList().Select(Mapper.Map<Session, SessionCalendarDto>);
        }


        [HttpGet]
        public IEnumerable<SessionCalendarDto> GetUpcomingSessionsForCalendar(int id)
        {
            return db.Sessions
                .Where(c => !c.isCancelled && (DateTime.Compare(c.SessionAt, DateTime.Now) > 0) && c.TrainerId == id)
                .ToList().Select(Mapper.Map<Session, SessionCalendarDto>);
        }

        [HttpGet]
        public IEnumerable<MemberSessionDto> GetUpcomingSessionsForMember(int id)
        {

            var user = db.GymMembers.Find(id);
            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);


            var joinedSessions = user.JoinedSessions
                .Where(c => !c.isCancelled && (DateTime.Compare(c.SessionAt, DateTime.Now) > 0));


            var joinedSessionDto = joinedSessions.Select(Mapper.Map<Session, JoinedSessionDto>)
                .Select(Mapper.Map<JoinedSessionDto, MemberSessionDto>);

            var sessions = db.Sessions.Include(c => c.GymMembers)
                .Where(c => !c.isCancelled && (DateTime.Compare(c.SessionAt, DateTime.Now) > 0))
                .ToList();

            List<Session> rlist = new List<Session>();
            foreach (Session session in sessions)
            {
                if (!session.GymMembers.Contains(user))
                    rlist.Add(session);
            }

            var upcomingSessionDto = rlist.Select(Mapper.Map<Session, UpcomingSessionDto>)
                .Select(Mapper.Map<UpcomingSessionDto,MemberSessionDto>);



            return joinedSessionDto.Concat(upcomingSessionDto);
        }


        [HttpGet]
        public IEnumerable<SessionDto> GetUpcomingSessionsForTable(String id)
        {

            var user = db.GymMembers.SingleOrDefault(c => c.UserId == id);
            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var sessions = db.Sessions.Include(c => c.GymMembers)
                .Where(c => !c.isCancelled && (DateTime.Compare(c.SessionAt, DateTime.Now) > 0))
                .ToList();

            List<Session> rlist = new List<Session>();
            foreach (Session session in sessions)
            {
                if (!session.GymMembers.Contains(user))
                    rlist.Add(session);
            }

            return rlist.Select(Mapper.Map<Session, SessionDto>);

        }

        [HttpGet]
        public IEnumerable<SessionDto> GetJoinedSessionsForTable(string id)
        {
            var user = db.GymMembers.SingleOrDefault(c => c.UserId == id);
            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);


            var sessions = user.JoinedSessions
                .Where(c => !c.isCancelled && (DateTime.Compare(c.SessionAt, DateTime.Now) > 0))
                .ToList().Select(Mapper.Map<Session, SessionDto>);
            return sessions;
        }


        //GET /api/sessions/id
        public IHttpActionResult GetSession(int id)
        {
            var session = db.Sessions.SingleOrDefault(c => c.Id == id);
            if (session == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return Ok(Mapper.Map<Session, SessionDto>(session));
        }

        [HttpPost]
        public IHttpActionResult SubmitFeedback(SessionFeedbackSubmitModel model)
        {

            Session session = db.Sessions.Find(model.SessionId);
            SessionFeedback sessionFeedback = new SessionFeedback()
            {
                GymMemberId = model.UserId,
                SessionId = model.SessionId,
                Desc = model.SessionFeedback,
                Rating = model.Rating,
            };

            // see if the user already provided a feedback
            var userFeedbackForSession = session.SessionFeedbacks.Where(c => c.GymMemberId == model.UserId).FirstOrDefault();

            if (userFeedbackForSession != null)
            {
                userFeedbackForSession.Desc = sessionFeedback.Desc;
                userFeedbackForSession.Rating = sessionFeedback.Rating;
            }
            else
            {
                session.SessionFeedbacks.Add(sessionFeedback);
            }

            db.Entry(session).State = EntityState.Modified;
            db.SaveChanges();


            return Ok();
        }


        //POST /api/createsession
        [HttpPost]
        public IHttpActionResult CreateSession(SessionCreateDto sessionDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var session = Mapper.Map<SessionCreateDto, Session>(sessionDto);
            db.Sessions.Add(session);
            db.SaveChanges();


            //send email to all members in gym
            var MembersInApp = db.GymMembers.ToList();

            EmailSender es = new EmailSender();
            foreach (var member in MembersInApp)
            {
                var salutation = member.Gender == "Male" ? "Mr." : "Miss.";
                es.Send(AppDbContext.Users.Find(member.UserId).Email, salutation + member.LastName, "SessionCreate", session);
            }

            return Ok();
        }


        [HttpPut]
        public void UpdateSession(SessionEditDto sessionDto)
        {
            var sessionInDb = db.Sessions.SingleOrDefault(c => c.Id == sessionDto.Id);

            ICollection<GymMember> joinedUsers = new List<GymMember>();
            foreach (var member in sessionInDb.GymMembers)
            {
                joinedUsers.Add(member);
            }

            if (sessionInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Mapper.Map(sessionDto, sessionInDb);

            db.Entry(sessionInDb).State = EntityState.Modified;
            db.SaveChanges();

            //send mail
            EmailSender es = new EmailSender();
            if (sessionInDb.isCancelled)
                foreach (var member in joinedUsers)
                {
                    var salutation = member.Gender == "Male" ? "Mr." : "Miss.";
                    es.Send(AppDbContext.Users.Find(member.UserId).Email, salutation + member.LastName, "SessionDelete", sessionInDb);
                }
            else
            {
                foreach (var member in joinedUsers)
                {
                    var salutation = member.Gender == "Male" ? "Mr." : "Miss.";
                    es.Send(AppDbContext.Users.Find(member.UserId).Email, salutation + member.LastName, "SessionEdit", sessionInDb);
                }
            }
        }

        public void DeleteSession(int id)
        {
            var sessionInDb = db.Sessions.SingleOrDefault(c => c.Id == id);

            if (sessionInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            db.Sessions.Remove(sessionInDb);
            db.Entry(sessionInDb).State = EntityState.Deleted;
            db.SaveChanges();

        }

    }
}
