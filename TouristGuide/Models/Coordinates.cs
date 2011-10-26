﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TouristGuide.Models
{
    public class Coordinates
    {
        public int ID { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}