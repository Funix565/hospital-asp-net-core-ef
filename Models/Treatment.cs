using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab5AspNetCoreEfIndividual.Models
{
    public class Treatment
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Title")]
        [StringLength(250)]
        public string TreatmentTitle { get; set; }

        [Required]
        [StringLength(250)]
        public string Defenition { get; set; }

        [Display(Name = "Room Number")]
        public int RoomNumber { get; set; }

        // The Entity Framework doesn't require you to add a foreign key property to your data model
        // when you have a navigation property for a related entity.
        // But having the foreign key in the data model can make updates simpler and more efficient.
        public int DepartmentID { get; set; }

        // A doctor is assigned to one department
        public Department Department { get; set; }

        // A Treatment may be performed by multiple doctors
        public ICollection<TreatmentAssignment> TreatmentAssignments { get; set; }

        // A Treatment can only have at most one Contraindication (may be null if no contraindication)
        public TreatmentContraindication TreatmentContraindication { get; set; }
    }
}
