﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FitnessGuruModelContainer : DbContext
    {
        public FitnessGuruModelContainer()
            : base("name=FitnessGuruModelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<GymMember> GymMembers { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<SessionFeedback> SessionFeedbacks { get; set; }
        public virtual DbSet<TrainerPersonalAppointmentSession> TrainerPersonalAppointmentSessions { get; set; }
        public virtual DbSet<PersonalAppointmentRequest> PersonalAppointmentRequests1 { get; set; }
        public virtual DbSet<PartnerGroup> PartnerGroups { get; set; }
        public virtual DbSet<GroupJoinRequest> GroupJoinRequests { get; set; }
        public virtual DbSet<GroupActivity> GroupActivities { get; set; }
    }
}