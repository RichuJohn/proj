using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitnessGuru_Main.Dtos
{
    public class SessionDto
    {

        public int Id { get; set; }
        public string SessionName { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime SessionAt { get; set; }
        public bool isCancelled { get; set; }
        public string Desc { get; set; }
        public int TrainerId { get; set; }

        public GymMemberDto GymMember { get; set; }
    }
}