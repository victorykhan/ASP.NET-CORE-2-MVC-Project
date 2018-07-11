using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectoApp.Models
{
    public class CapitalCity
    {
        [Key]
        public int CapitalCityId { get; set; }

        [Required]
        [Display(Name = "Capital City")]
        public string CapitalCityName { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string CapCountry { get; set; }
    }
}
