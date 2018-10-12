using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessGuru_Main.Dtos
{
    public class SessionCreateDto
    {
        public int Id { get; set; }
        public string SessionName { get; set; }
        public System.DateTime SessionAt { get; set; }
        public string Desc { get; set; }
        public int TrainerId { get; set; }
    }
}