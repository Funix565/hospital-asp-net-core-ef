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

        // TODO: Where is precision and how to set it
        // Comma is not accepted, but it is displayed - https://stackoverflow.com/q/71001434, https://stackoverflow.com/q/3504660
        // Probably, some misunderstanding with default EF decimal and PSQL decimal
        // PSQL can have plain decimal, which means I can store any precision and scale up to the limit of the precision and scale mentioned above.
        // But looks like EF core has default (18,2) or (18,0)
        // Have to specify precision and scale manually
        // Still not working
        // Don't know why, but [543,123] - three digits after comma - works well :(
        // However, Index and Edit display it with two digits
        // OK. I figured it out. For some reason it works like that:

        // 1) Index page displays Budget as number with spaces to separate long digits
        // 2) It also has , (comma) as a decimal separator. And it dispalys ₴ sign
        // 3) I try to create new Department
        // 4) (d) -- passes
        // 5) I try to edit created row and just click save without changes
        // 6) The field displays number as (d,00) and there is a warning "The field Budget must be a number."
        // 7) If I use . (dot) instead of comma: "The value '1.00' is not valid for Budget."
        // 8) So, definately we have comma as decimal separator
        // 9) (1,000) - pass
        // 10) But Index displays (1,00)
        // 11) And also there is rounding. (1,129) -> becomes 1,13 in Index
        // 12) I can't use four and more digits after comma
        // 13) And I can't use four digits before comma
        // 14) 111,111 pass
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(19, 4)")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Establishment Date")]
        public DateTime EstablishmentDate { get; set; }

        // A department may or may not have a chief, and a chief is always a doctor
        // ? -- the property marked nullable
        // Note: By convention, the EF enables cascade delete for non-nullable foreign keys and for many-to-many relationships
        public int? DoctorID { get; set; }

        // PostgreSQL doesn't have such auto-updating columns

        // Optimistic concurrency.
        // The Timestamp attribute specifies that this column will be included
        // in the Where clause of Update and Delete commands sent to the database.
        //[Timestamp]
        //public byte[] RowVersion { get; set; }

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
