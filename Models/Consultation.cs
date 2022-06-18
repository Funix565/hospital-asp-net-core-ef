using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab5AspNetCoreEfIndividual.Models
{
    // Many-to-many between Patient and Doctor.
    // Consultation functions as a many-to-many join table *with payload* in the database,
    // i.e. contains additional data besides foreign keys.
    public class Consultation
    {
        // It has own ID because one doctor can meet with one patient more than once
        public int ConsultationID { get; set; }

        // The ? indicates that the property is nullable
        // The ? means a date isn't known or hasn't been assigned yet
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true, NullDisplayText = "Not confirmed")]
        public DateTime? ConsultationDate { get; set; }
        public int RoomNumber { get; set; }

        // Consultation is done by a single doctor with a single patient.
        public int DoctorID { get; set; }
        public int PatientID { get; set; }

        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }
}
