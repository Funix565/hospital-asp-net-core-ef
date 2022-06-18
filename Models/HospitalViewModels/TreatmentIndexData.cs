using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5AspNetCoreEfIndividual.Models.HospitalViewModels
{
    // The Treatments page shows data from three different tables.
    // - The list of treatments displays data from the TreatmentContraindication.
    // - When the user selects a treatment, related Doctor entities are displayed.
    // - When the user selects a doctor, related data from the Consultations entity set is displayed.
    public class TreatmentIndexData
    {
        public IEnumerable<Treatment> Treatments { get; set; }
        public IEnumerable<Doctor> Doctors { get; set; }
        public IEnumerable<Consultation> Consultations { get; set; }
    }
}
