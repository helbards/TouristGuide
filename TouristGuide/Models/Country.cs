using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TouristGuide.Models
{
    public class Country
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}