using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TouristGuide.Models
{
    public class Location
    {
        public int ID { get; set; }
        public Coordinates Coordinates { get; set; }

        [Required]
        public LocationType LocationType { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public int BuildingNumber { get; set; }
        public string Region { get; set; }
        public Country Country { get; set; }
    }
}