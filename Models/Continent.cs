using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectoApp.Models
{
    public class Continent
    {
        [Key]
        public int ContinentId { get; set; }

        [Required]
        [Display(Name = "Continent")]
        public string ContinentName { get; set; }
    }
}
