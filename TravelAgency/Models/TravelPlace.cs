using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelAgency.Models
{
    public class TravelPlace
    {
        [Key]
        public int Id { get; set; }
        public string PlaceName { get; set; }
    }
}
