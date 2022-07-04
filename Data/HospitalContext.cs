﻿using Lab5AspNetCoreEfIndividual.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5AspNetCoreEfIndividual.Data
{
    // The main class that coordinates EF functionality for a given data model
    public class HospitalContext : DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options) : base(options)
        {
        }

        // An entity set typically corresponds to a database table
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<TreatmentContraindication> TreatmentContraindications { get; set; }
        public DbSet<TreatmentAssignment> TreatmentAssignments { get; set; }

        // When the database is created, EF creates tables that have names the same as the DbSet property names.
        // Specifying singular table names.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO: Optimistic concurrency in Npgsql:
            // - https://www.npgsql.org/efcore/modeling/concurrency.html
            // - https://stackoverflow.com/q/60801649
            // - https://github.com/npgsql/efcore.pg/issues/19#issue-148354668
            
            // Probably, this modification is required. It didn't work with just a column in Department model
            modelBuilder.Entity<Department>()
                .UseXminAsConcurrencyToken();

            modelBuilder.Entity<Doctor>().ToTable("Doctor");
            modelBuilder.Entity<Consultation>().ToTable("Consultation");
            modelBuilder.Entity<Patient>().ToTable("Patient");
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Treatment>().ToTable("Treatment");
            modelBuilder.Entity<TreatmentContraindication>().ToTable("TreatmentContraindication");
            modelBuilder.Entity<TreatmentAssignment>().ToTable("TreatmentAssignment");

            // Configure composite primary key.
            // Note: Fluent API overrides attributes.
            modelBuilder.Entity<TreatmentAssignment>()
                .HasKey(t => new { t.DoctorID, t.TreatmentID });
        }
    }
}
