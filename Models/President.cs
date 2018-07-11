using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectoApp.Models
{ 
    public enum Terms
    {
        First, Second, Third, Fourth, Fifth, Sixth, Lifetime, Dictator
    }
   
    public class President
    {
        [Key]
        public int PresidentId { get; set; }

        [Display(Name ="Full Name")]
        [Required]
        public string FullName { get; set; }

        [Display(Name = "Assumed Office Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public DateTime AssumedOffice { get; set; }

        [Display(Name = "Terms Served")]
        [Required]
        public Terms OfficeTerms { get; set; }

        [Display(Name = "Preceded By")]
        public string PrecededBy { get; set; }

        [Display(Name = "Photo of the President")]
        [Required]
        public byte[] Photo { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string PresCountry { get; set; }

        [Display(Name = "Capital City")]
        public string PresCapital { get; set; }

        [Required]
        [Display(Name = "Continent")]
        public string PresContinent { get; set; }

        [Display(Name = "Name of Political Party")]
        [Required]
        public string PoliticalParty { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string Publisher { get; set; }
    }
}
