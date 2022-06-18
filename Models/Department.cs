using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5AspNetCoreEfIndividual.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Establishment Date")]
        public DateTime EstablishmentDate { get; set; }

        // A department may or may not have a chief, and a chief is always a doctor
        // ? -- the property marked nullable
        // Note: By convention, the EF enables cascade delete for non-nullable foreign keys and for many-to-many relationships
        public int? DoctorID { get; set; }

        public Doctor Chief { get; set; }

        // A department may have many treatments (kinds)
        public ICollection<Treatment> Treatments { get; set; }
    }
}
