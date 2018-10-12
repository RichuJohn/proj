using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessGuru_Main.Dtos
{
    public class MemberSessionDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public int TrainerId { get; set; }
        public string Desc { get; set; }
        public string TrainerName { get; set; }
        public bool Joined { get; set; }
    }
}