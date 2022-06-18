using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5AspNetCoreEfIndividual.Models.HospitalViewModels
{
    public class PatientDiagnosisGroup
    {
        [DataType(DataType.Text)]
        public string? Diagnosis { get; set; }
        public int PatientCount { get; set; }
    }
}
