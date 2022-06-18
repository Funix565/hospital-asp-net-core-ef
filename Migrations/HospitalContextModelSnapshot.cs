﻿// <auto-generated />
using System;
using Lab5AspNetCoreEfIndividual.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Lab5AspNetCoreEfIndividual.Migrations
{
    [DbContext(typeof(HospitalContext))]
    partial class HospitalContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Lab5AspNetCoreEfIndividual.Models.Consultation", b =>
                {
                    b.Property<int>("ConsultationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("ConsultationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("DoctorID")
                        .HasColumnType("integer");

                    b.Property<int>("PatientID")
                        .HasColumnType("integer");

                    b.Property<int>("RoomNumber")
                        .HasColumnType("integer");

                    b.HasKey("ConsultationID");

                    b.HasIndex("DoctorID");

                    b.HasIndex("PatientID");

                    b.ToTable("Consultation");
                });

            modelBuilder.Entity("Lab5AspNetCoreEfIndividual.Models.Department", b =>
                {
                    b.Property<int>("DepartmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("Budget")
                        .HasColumnType("decimal");

                    b.Property<int?>("DoctorID")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EstablishmentDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("DepartmentID");

                    b.HasIndex("DoctorID");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("Lab5AspNetCoreEfIndividual.Models.Doctor", b =>
                {
                    b.Property<int>("DoctorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("DoctorName")
                        .HasColumnType("text");

                    b.Property<string>("JobTitle")
                        .HasColumnType("text");

                    b.HasKey("DoctorID");

                    b.ToTable("Doctor");
                });

            modelBuilder.Entity("Lab5AspNetCoreEfIndividual.Models.Patient", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Diagnosis")
                        .HasColumnType("text");

                    b.Property<int>("InsuranceId")
                        .HasColumnType("integer");

                    b.Property<string>("PatientName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("patientname");

                    b.HasKey("ID");

                    b.ToTable("Patient");
                });

            modelBuilder.Entity("Lab5AspNetCoreEfIndividual.Models.Treatment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Defenition")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<int>("DepartmentID")
                        .HasColumnType("integer");

                    b.Property<int>("RoomNumber")
                        .HasColumnType("integer");

                    b.Property<string>("TreatmentTitle")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.HasKey("ID");

                    b.HasIndex("DepartmentID");

                    b.ToTable("Treatment");
                });

            modelBuilder.Entity("Lab5AspNetCoreEfIndividual.Models.TreatmentAssignment", b =>
                {
                    b.Property<int>("DoctorID")
                        .HasColumnType("integer");

                    b.Property<int>("TreatmentID")
                        .HasColumnType("integer");

                    b.HasKey("DoctorID", "TreatmentID");

                    b.HasIndex("TreatmentID");

                    b.ToTable("TreatmentAssignment");
                });

            modelBuilder.Entity("Lab5AspNetCoreEfIndividual.Models.TreatmentContraindication", b =>
                {
                    b.Property<int>("TreatmentID")
                        .HasColumnType("integer");

                    b.Property<string>("Overview")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.HasKey("TreatmentID");

                    b.ToTable("TreatmentContraindication");
                });

            modelBuilder.Entity("Lab5AspNetCoreEfIndividual.Models.Consultation", b =>
                {
                    b.HasOne("Lab5AspNetCoreEfIndividual.Models.Doctor", "Doctor")
                        .WithMany("Consultations")
                        .HasForeignKey("DoctorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lab5AspNetCoreEfIndividual.Models.Patient", "Patient")
                        .WithMany("Consultations")
                        .HasForeignKey("PatientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("Lab5AspNetCoreEfIndividual.Models.Department", b =>
                {
                    b.HasOne("Lab5AspNetCoreEfIndividual.Models.Doctor", "Chief")
                        .WithMany()
                        .HasForeignKey("DoctorID");

                    b.Navigation("Chief");
                });

            modelBuilder.Entity("Lab5AspNetCoreEfIndividual.Models.Treatment", b =>
                {
                    b.HasOne("Lab5AspNetCoreEfIndividual.Models.Department", "Department")
                        .WithMany("Treatments")
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Lab5AspNetCoreEfIndividual.Models.TreatmentAssignment", b =>
                {
                    b.HasOne("Lab5AspNetCoreEfIndividual.Models.Doctor", "Doctor")
                        .WithMany("TreatmentAssignments")
                        .HasForeignKey("DoctorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lab5AspNetCoreEfIndividual.Models.Treatment", "Treatment")
                        .WithMany("TreatmentAssignments")
                        .HasForeignKey("TreatmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Treatment");
                });

            modelBuilder.Entity("Lab5AspNetCoreEfIndividual.Models.TreatmentContraindication", b =>
                {
                    b.HasOne("Lab5AspNetCoreEfIndividual.Models.Treatment", "Treatment")
                        .WithOne("TreatmentContraindication")
                        .HasForeignKey("Lab5AspNetCoreEfIndividual.Models.TreatmentContraindication", "TreatmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Treatment");
                });

            modelBuilder.Entity("Lab5AspNetCoreEfIndividual.Models.Department", b =>
                {
                    b.Navigation("Treatments");
                });

            modelBuilder.Entity("Lab5AspNetCoreEfIndividual.Models.Doctor", b =>
                {
                    b.Navigation("Consultations");

                    b.Navigation("TreatmentAssignments");
                });

            modelBuilder.Entity("Lab5AspNetCoreEfIndividual.Models.Patient", b =>
                {
                    b.Navigation("Consultations");
                });

            modelBuilder.Entity("Lab5AspNetCoreEfIndividual.Models.Treatment", b =>
                {
                    b.Navigation("TreatmentAssignments");

                    b.Navigation("TreatmentContraindication");
                });
#pragma warning restore 612, 618
        }
    }
}