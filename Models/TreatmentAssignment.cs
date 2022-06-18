using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5AspNetCoreEfIndividual.Models
{
    // A join table is required in the database for the Treatment-to-Doctor many-to-many relationship
    public class TreatmentAssignment
    {
        // It doesn't have own ID because one doctor can perform one treatment once
        // For example Doc1 breaks teeth. It's a fact. No duplicates

        // Since the foreign keys are not nullable and together uniquely identify each row of the table,
        // there's no need for a separate primary key.
        public int DoctorID { get; set; }
        public int TreatmentID { get; set; }

        // properties should function as a composite primary key
        // use the fluent API (it can't be done by using attributes)
        public Doctor Doctor { get; set; }
        public Treatment Treatment { get; set; }
    }
}
