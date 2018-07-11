using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectoApp.Models
{
    public enum Electoral
    {
        Presidential, Congressional, Senatorial, Parliamentary
    }
    public class Election
    {
        [Key]
        public int ElectionId { get; set; }

        [Required]
        [Display(Name = "Electoral System")]
        public Electoral ElectionType { get; set; }

        [Display(Name = "Previous Election Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PreviousElectionDate { get; set; }

        [Display(Name = "Upcoming Election Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ElectionDate { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string CountryName { get; set; }

        [Required]
        public string PublisherId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

    }
}
