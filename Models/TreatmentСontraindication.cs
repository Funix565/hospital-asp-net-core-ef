using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab5AspNetCoreEfIndividual.Models
{
    // one-to-zero-or-one relationship between the Treatment and the Contraindication
    public class TreatmentContraindication
    {
        // its primary key is also its foreign key to the Treatment
        [Key]
        public int TreatmentID { get; set; }

        [StringLength(1024)]
        [Display(Name = "Contraindication Overview")]
        public string Overview { get; set; }

        public Treatment Treatment { get; set; }
    }
}
