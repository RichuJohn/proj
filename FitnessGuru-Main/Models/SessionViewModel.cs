using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessGuru_Main.Models
{
    public class SessionViewModel
    {
    }

    public class SessionCreateViewModel
    {
        public string SessionName { get; set; }
        public DateTime SessionAt { get; set; }
        public string Desc { get; set; }
    }
}