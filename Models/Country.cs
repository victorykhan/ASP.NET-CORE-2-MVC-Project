using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectoApp.Models
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }
        [Required]
        [Display(Name = "Country")]
        public string CountryName { get; set; }
        [Required]
        [Display(Name = "Continent")]
        public string CounContinent { get; set; }
    }
}
