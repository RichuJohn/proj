using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessGuru_Main.Dtos
{
    public class SessionEditDto
    {
        public int Id { get; set; }
        public string SessionName { get; set; }
        public DateTime SessionAt { get; set; }
        public bool isCancelled { get; set; }
        public string Desc { get; set; }
        public int TrainerId { get; set; }
    }
}