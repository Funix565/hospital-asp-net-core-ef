using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5AspNetCoreEfIndividual.Models.HospitalViewModels
{
    // TODO: Why do we need ViewModels at all?
    public class AssignedTreatmentData
    {
        public int TreatmentID { get; set; }
        public string Title { get; set; }
        public bool Assigned { get; set; }
    }
}
