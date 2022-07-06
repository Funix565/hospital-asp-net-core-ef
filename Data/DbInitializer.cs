using Lab5AspNetCoreEfIndividual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5AspNetCoreEfIndividual.Data
{
    public static class DbInitializer
    {
        public static void Initialize(HospitalContext context)
        {
            // context.Database.EnsureCreated();

            if (context.Patients.Any())
            {
                // data exists, no need to add
                return;
            }

            var patients = new Patient[]
            {
                new Patient { PatientName = "Alla Tkach", Diagnosis = "Cancer", InsuranceId = 12345 },
                new Patient { PatientName = "Ivan Hanov", Diagnosis = "Headache", InsuranceId = 54321 },
                new Patient { PatientName = "Stepan Franko", Diagnosis = "Diabet", InsuranceId = 48195 },
                new Patient { PatientName = "Adolf Pumakov", Diagnosis = "Arrhythmia", InsuranceId = 19045 },
                new Patient { PatientName = "Kate Lobanova", Diagnosis = "Arthritis", InsuranceId = 100000 }
            };

            foreach (Patient p in patients)
            {
                context.Patients.Add(p);
            }

            // Apply changes to DB!
            context.SaveChanges();

            var docotrs = new Doctor[]
            {
                new Doctor{ DoctorName = "Taras Dub", JobTitle = "Intern" },
                new Doctor{ DoctorName = "Orest Lypa", JobTitle = "Dentist" },
                new Doctor{ DoctorName = "Max Pain", JobTitle = "Massage therapist" },
                new Doctor{ DoctorName = "Kyrylo Bereza", JobTitle = "Acupuncturist" },
                new Doctor{ DoctorName = "Iren Red", JobTitle = "Paramedic" },
                new Doctor{ DoctorName = "Chris Green", JobTitle = "Mental Health Support Worker" }
            };

            foreach (Doctor d in docotrs)
            {
                context.Doctors.Add(d);
            }

            context.SaveChanges();

            var departments = new Department[]
            {
                new Department {
                    Name = "Brain Tumor Program",
                    Budget = 1000000,
                    EstablishmentDate = DateTime.Parse("2021-06-14"),
                    DoctorID = docotrs.Single(d => d.DoctorName == "Taras Dub").DoctorID
                },
                new Department {
                    Name = "Dermatology",
                    Budget = 300000,
                    EstablishmentDate = DateTime.Parse("2005-10-23"),
                    DoctorID = docotrs.Single(d => d.DoctorName == "Orest Lypa").DoctorID
                },
                new Department {
                    Name = "Cardiology",
                    Budget = 123000,
                    EstablishmentDate = DateTime.Parse("2002-10-14"),
                    DoctorID = docotrs.Single(d => d.DoctorName == "Orest Lypa").DoctorID
                }
            };

            foreach (Department d in departments)
            {
                context.Departments.Add(d);
            }

            context.SaveChanges();

            var treatments = new Treatment[]
            {
                new Treatment {
                    TreatmentTitle = "Blood test",
                    Definition = "A laboratory analysis performed on a blood sample that is usually extracted from a vein in the arm using a hypodermic needle, or via fingerprick",
                    RoomNumber = 2020,
                    DepartmentID = departments.Single(s => s.Name == "Dermatology").DepartmentID
                },
                new Treatment {
                    TreatmentTitle = "Chemotherapy",
                    Definition = "A type of cancer treatment that uses one or more anti-cancer drugs as part of a standardized chemotherapy regimen",
                    RoomNumber = 2090,
                    DepartmentID = departments.Single(s => s.Name == "Brain Tumor Program").DepartmentID
                },
                new Treatment {
                    TreatmentTitle = "Radiation therapy",
                    Definition = "A therapy using ionizing radiation, generally provided as part of cancer treatment to control or kill malignant cells and normally delivered by a linear accelerator",
                    RoomNumber = 2030,
                    DepartmentID = departments.Single(s => s.Name == "Brain Tumor Program").DepartmentID
                },
                new Treatment {
                    TreatmentTitle = "Palliative care",
                    Definition = "An interdisciplinary medical caregiving approach aimed at optimizing quality of life and mitigating suffering among people with serious, complex, and often terminal illnesses",
                    RoomNumber = 2040,
                    DepartmentID = departments.Single(s => s.Name == "Brain Tumor Program").DepartmentID
                },
                new Treatment {
                    TreatmentTitle = "Facial rejuvenation",
                    Definition = "A cosmetic treatment, which aims to restore a youthful appearance to the human face",
                    RoomNumber = 2050,
                    DepartmentID = departments.Single(s => s.Name == "Dermatology").DepartmentID
                },
                new Treatment {
                    TreatmentTitle = "Lobotomy",
                    Definition = "A form of neurosurgical treatment for psychiatric disorder or neurological disorder that involves severing connections in the brain's prefrontal cortex",
                    RoomNumber = 2060,
                    DepartmentID = departments.Single(s => s.Name == "Brain Tumor Program").DepartmentID
                },
                new Treatment {
                    TreatmentTitle = "Nicotine replacement therapy",
                    Definition = "A medically approved way to treat people with tobacco use disorder by taking nicotine by means other than tobacco",
                    RoomNumber = 2070,
                    DepartmentID = departments.Single(s => s.Name == "Brain Tumor Program").DepartmentID
                },
                new Treatment {
                    TreatmentTitle = "Biopsy",
                    Definition = "The process involves extraction of sample cells or tissues for examination to determine the presence or extent of a disease",
                    RoomNumber = 2080,
                    DepartmentID = departments.Single(s => s.Name == "Dermatology").DepartmentID
                }
            };

            foreach (Treatment t in treatments)
            {
                context.Treatments.Add(t);
            }

            context.SaveChanges();

            var treatmentContraindications = new TreatmentContraindication[]
            {
                new TreatmentContraindication {
                    TreatmentID = treatments.Single(t => t.TreatmentTitle == "Chemotherapy").ID,
                    Overview = "Pregnancy or lactation. Congenital long QT syndrome"
                },
                new TreatmentContraindication {
                    TreatmentID = treatments.Single(t => t.TreatmentTitle == "Blood test").ID,
                    Overview = "Conditions such as cellulitis or an abscess can increase the risk of bacteria directly infiltrating the blood"
                },
                new TreatmentContraindication {
                    TreatmentID = treatments.Single(t => t.TreatmentTitle == "Biopsy").ID,
                    Overview = "Uncorrectable bleeding diathesis. Skin infection at biopsy site"
                },
                new TreatmentContraindication {
                    TreatmentID = treatments.Single(t => t.TreatmentTitle == "Facial rejuvenation").ID,
                    Overview = "Viruses such as colds, FLU, COVID19, fever, cold sores, warts, Bacterial infections such as impetigo"
                }
            };

            foreach (TreatmentContraindication tc in treatmentContraindications)
            {
                context.TreatmentContraindications.Add(tc);
            }

            context.SaveChanges();

            var treatmentDoctors = new TreatmentAssignment[]
            {
                new TreatmentAssignment {
                    DoctorID = docotrs.Single(d => d.DoctorName == "Taras Dub").DoctorID,
                    TreatmentID = treatments.Single(t => t.TreatmentTitle == "Chemotherapy").ID
                },
                new TreatmentAssignment {
                    DoctorID = docotrs.Single(d => d.DoctorName == "Orest Lypa").DoctorID,
                    TreatmentID = treatments.Single(t => t.TreatmentTitle == "Blood test").ID
                },
                                new TreatmentAssignment {
                    DoctorID = docotrs.Single(d => d.DoctorName == "Iren Red").DoctorID,
                    TreatmentID = treatments.Single(t => t.TreatmentTitle == "Facial rejuvenation").ID
                },
                new TreatmentAssignment {
                    DoctorID = docotrs.Single(d => d.DoctorName == "Chris Green").DoctorID,
                    TreatmentID = treatments.Single(t => t.TreatmentTitle == "Blood test").ID
                },
                new TreatmentAssignment {
                    DoctorID = docotrs.Single(d => d.DoctorName == "Max Pain").DoctorID,
                    TreatmentID = treatments.Single(t => t.TreatmentTitle == "Lobotomy").ID
                }
            };

            foreach (TreatmentAssignment ta in treatmentDoctors)
            {
                context.TreatmentAssignments.Add(ta);
            }

            context.SaveChanges();

            var consultations = new Consultation[]
            {
                new Consultation {
                    PatientID = patients.Single(p => p.PatientName == "Alla Tkach").ID,
                    DoctorID = docotrs.Single(d => d.DoctorName == "Taras Dub").DoctorID,
                    ConsultationDate = DateTime.Parse("2022-06-06 14:30:00"),
                    RoomNumber = 222
                },
                new Consultation {
                    PatientID = patients.Single(p => p.PatientName == "Ivan Hanov").ID,
                    DoctorID = docotrs.Single(d => d.DoctorName == "Orest Lypa").DoctorID,
                    ConsultationDate = DateTime.Parse("2022-06-09 18:20:00"),
                    RoomNumber = 111
                }
            };

            // I don't understand this loop and checks
            foreach (Consultation c in consultations)
            {
                var consultationInDataBase = context.Consultations.Where(
                    s => s.Patient.ID == c.PatientID &&
                    s.Doctor.DoctorID == c.DoctorID).SingleOrDefault();

                if (consultationInDataBase == null)
                {
                    context.Consultations.Add(c);
                }
            }

            context.SaveChanges();
        }
    }
}
