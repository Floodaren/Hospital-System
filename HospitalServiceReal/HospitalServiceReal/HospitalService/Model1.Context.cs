﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HospitalService
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HospitaldbEntities : DbContext
    {
        public HospitaldbEntities()
            : base("name=HospitaldbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Doctor> Doctor { get; set; }
        public virtual DbSet<DoctorLogIn> DoctorLogIn { get; set; }
        public virtual DbSet<DoctorXPatient> DoctorXPatient { get; set; }
        public virtual DbSet<HospitalPost> HospitalPost { get; set; }
        public virtual DbSet<Journal> Journal { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<PatientLogIn> PatientLogIn { get; set; }
    }
}