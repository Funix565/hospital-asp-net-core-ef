using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5AspNetCoreEfIndividual.Models
{
    public abstract class Person
    {
        public int ID { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "Name cannot be longer that 250 characters.")]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
