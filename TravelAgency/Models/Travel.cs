using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TravelAgency.Models
{
    public class Travel
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; }    
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        public int PersonNumber { get; set; }
        [DataType(DataType.ImageUrl)]      
        public string ImageUrl { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public int TravelPlaceId { get; set; }
        [ForeignKey("TravelPlaceId")]
        public TravelPlace TravelPlace { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
