using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitnessGuru_Main.Models
{
    public class SessionViewModel
    {
    }

    public class SessionCreateViewModel
    {
        [Display(Name="Session Name")]
        public string SessionName { get; set; }

        [Display(Name="Session Scheduled At")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime SessionAt { get; set; }

        [Display(Name="Short Description")]
        public string Desc { get; set; }
    }

    public class SessionFeedbackViewModel
    {
        public Session session { get; set; }
        public SessionFeedback feedback { get; set; }
    }

}