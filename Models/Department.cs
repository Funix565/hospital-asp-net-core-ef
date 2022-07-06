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

        // There is no point in storig millions with 50 kopiykas
        // Comma is not accepted, but it is displayed - https://stackoverflow.com/q/71001434, https://stackoverflow.com/q/3504660
        [DisplayFormat(DataFormatString = "{0:c0}")]
        public long Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Establishment Date")]
        public DateTime EstablishmentDate { get; set; }

        // A department may or may not have a chief, and a chief is always a doctor
        // ? -- the property marked nullable
        // Note: By convention, the EF enables cascade delete for non-nullable foreign keys and for many-to-many relationships
        public int? DoctorID { get; set; }

        // The Timestamp attribute specifies that this column will be included
        // in the Where clause of Update and Delete commands sent to the database.
        //[Timestamp]
        //public byte[] RowVersion { get; set; }

        // PostgreSQL doesn't have such auto-updating columns

        // Optimistic concurrency.
        // Concurrency in PostgreSQL/Npgsql/EFCore: https://www.npgsql.org/efcore/modeling/concurrency.html
        // xmin - each update of a row creates a new row version for the same logical row: https://www.postgresql.org/docs/current/ddl-system-columns.html
        // Since this value automatically gets updated every time the row is changed, it is ideal for use as a concurrency token.
        // This approach works with the code in OnModelCreating.
        // Otherwise - InvalidCastException: Can't cast database type xid to Int64. https://github.com/npgsql/efcore.pg/issues/1646
        public uint xmin { get; set; }

        public Doctor Chief { get; set; }

        // A department may have many treatments (kinds)
        public ICollection<Treatment> Treatments { get; set; }
    }
}
