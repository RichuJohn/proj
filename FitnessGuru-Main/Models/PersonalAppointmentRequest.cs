//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FitnessGuru_Main.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PersonalAppointmentRequest
    {
        public int Id { get; set; }
        public string Desc { get; set; }
        public bool IsApproved { get; set; }
        public string Feedback { get; set; }
        public int GymMemberId { get; set; }
        public Nullable<int> Rating { get; set; }
    
        public virtual GymMember GymMember { get; set; }
        public virtual TrainerPersonalAppointmentSession TrainerPersonalAppointmentSession { get; set; }
    }
}