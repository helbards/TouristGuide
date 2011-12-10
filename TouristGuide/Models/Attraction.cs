using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TouristGuide.Models
{
    public class Attraction
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [MinLength(20)]
        [AllowHtml]
        public string Description { get; set; }

        public Coordinates Coordinates { get; set; }

        public Address Address { get; set; }

        [Required]
        public Country Country { get; set; }

        [Required]
        [Display(Name = "Attraction Type")]
        public AttractionType AttractionType { get; set; }
        
        public List<AttractionReview> Reviews { get; set; }
    }
}