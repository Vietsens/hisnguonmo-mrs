﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HTC.EFMODEL.DataModels
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HTCEntities : DbContext
    {
        public HTCEntities()
            : base("name=HTCEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<HTC_EXPENSE> HTC_EXPENSE { get; set; }
        public DbSet<HTC_EXPENSE_TYPE> HTC_EXPENSE_TYPE { get; set; }
        public DbSet<HTC_PERIOD> HTC_PERIOD { get; set; }
        public DbSet<HTC_PERIOD_DEPARTMENT> HTC_PERIOD_DEPARTMENT { get; set; }
        public DbSet<HTC_REPARTITION_RATIO> HTC_REPARTITION_RATIO { get; set; }
        public DbSet<HTC_REPARTITION_TYPE> HTC_REPARTITION_TYPE { get; set; }
        public DbSet<HTC_REVENUE> HTC_REVENUE { get; set; }
        public DbSet<V_HTC_EXPENSE> V_HTC_EXPENSE { get; set; }
        public DbSet<V_HTC_PERIOD_DEPARTMENT> V_HTC_PERIOD_DEPARTMENT { get; set; }
        public DbSet<V_HTC_REPARTITION_RATIO> V_HTC_REPARTITION_RATIO { get; set; }
    }
}
