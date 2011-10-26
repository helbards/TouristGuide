using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace TouristGuide.Models
{
    public class TouristGuideDB : DbContext
    {
        public DbSet<News> News { get; set; }
        public DbSet<Attraction> Attraction { get; set; }
        public DbSet<AttractionReview> AttractionReview { get; set; }
        public DbSet<AttractionType> AttractionType { get; set; }
        public DbSet<Coordinates> Coordinates { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<LocationType> LocationType { get; set; }
    }
}