using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessGuru_Main.Models
{
    public class GymMemberViewModel
    {
    }

    public class GymMemberIndexViewModel
    {
        public ICollection<Session> JoinedSessions { get; set; }
        public ICollection<Session> CompleteSessions { get; set; }
    }

    public class TrainerIndexViewModel
    {
        public ICollection<Session> ManagingSessions { get; set; }
        public ICollection<Session> UpcomingSessions { get; set; }
    }
}