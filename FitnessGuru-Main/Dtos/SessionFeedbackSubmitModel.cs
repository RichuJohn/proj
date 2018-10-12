using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessGuru_Main.Dtos
{
    public class SessionFeedbackSubmitModel
    {
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public String SessionFeedback { get; set; }
        public int Rating { get; set; }
    }
}