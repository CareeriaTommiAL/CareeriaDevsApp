﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CareeriaDevsApp.Databases
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CareeriaDevAppEntities : DbContext
    {
        public CareeriaDevAppEntities()
            : base("name=CareeriaDevAppEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Login> Login { get; set; }
        public virtual DbSet<OmaSisalto> OmaSisalto { get; set; }
        public virtual DbSet<Opiskelija> Opiskelija { get; set; }
        public virtual DbSet<PaaKayttaja> PaaKayttaja { get; set; }
        public virtual DbSet<Postitoimipaikka> Postitoimipaikka { get; set; }
        public virtual DbSet<PuhelinNumero> PuhelinNumero { get; set; }
        public virtual DbSet<PuhelinTyyppi> PuhelinTyyppi { get; set; }
        public virtual DbSet<Sahkoposti> Sahkoposti { get; set; }
        public virtual DbSet<Viesti> Viesti { get; set; }
        public virtual DbSet<Yritys> Yritys { get; set; }
    }
}