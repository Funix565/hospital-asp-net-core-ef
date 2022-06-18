using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5AspNetCoreEfIndividual.Models
{
    public class Doctor
    {
        // Entity properties that are named ID or classnameID are recognized as PK properties.
        public int DoctorID { get; set; }

        [Display(Name = "Doctor Name")]
        public string DoctorName { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        // A Doctor can have many patients via consulations
        public ICollection<Consultation> Consultations { get; set; }

        // A Doctor can perform multiple treatments
        public ICollection<TreatmentAssignment> TreatmentAssignments { get; set; }
    }
}
