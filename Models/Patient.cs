using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab5AspNetCoreEfIndividual.Models
{
    public class Patient
    {
        public int ID { get; set; }

        // Types that can't be null are automatically treated as required fields
        // requires the first character to be upper case and the remaining characters to be alphabetical, allows space [' ']
        [Required]
        [StringLength(250)]
        [RegularExpression(@"^[A-Z]+[a-z A-Z]*$")]
        [Column("patientname")]
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }

        [Display(Name = "Insurance #")]
        public int InsuranceId { get; set; }

        [Display(Name = "Diagnosis")]
        public string Diagnosis { get; set; }

        // A patient can visit many doctors via consultations.
        public ICollection<Consultation> Consultations { get; set; }
    }
}
