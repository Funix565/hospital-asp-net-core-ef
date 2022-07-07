using Lab5AspNetCoreEfIndividual.Models;
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
        // 1) TPH
        public DbSet<Person> People { get; set; }

        // When the database is created, EF creates tables that have names the same as the DbSet property names.
        // Specifying singular table names.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Optimistic concurrency in Npgsql:
            // - https://www.npgsql.org/efcore/modeling/concurrency.html [Concurrency Tokens]
            // - https://stackoverflow.com/q/60801649
            // - https://github.com/npgsql/efcore.pg/issues/19#issue-148354668 [Usage of xmin column as concurrency token]

            // Probably, this modification is required. It didn't work with just a column in Department model
            modelBuilder.Entity<Department>()
                .UseXminAsConcurrencyToken();

            modelBuilder.Entity<Doctor>().ToTable("Person");
            modelBuilder.Entity<Consultation>().ToTable("Consultation");
            modelBuilder.Entity<Patient>().ToTable("Person");
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Treatment>().ToTable("Treatment");
            modelBuilder.Entity<TreatmentContraindication>().ToTable("TreatmentContraindication");
            modelBuilder.Entity<TreatmentAssignment>().ToTable("TreatmentAssignment");
            // 2) TPH
            // This is all that the Entity Framework needs in order to configure table-per-hierarchy inheritance
            modelBuilder.Entity<Person>().ToTable("Person");

            // Configure composite primary key.
            // Note: Fluent API overrides attributes.
            modelBuilder.Entity<TreatmentAssignment>()
                .HasKey(t => new { t.DoctorID, t.TreatmentID });
        }
    }
}
