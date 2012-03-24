using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristGuide.Models;
using System.Web.Services;
using System.Text.RegularExpressions;

namespace TouristGuide.Controllers
{ 
    [WebService]
    public class WebServiceController : Controller
    {
        private TouristGuideDB db = new TouristGuideDB();

        // GET: /WebService/GetAttractions?place=Name
        [WebMethod]
        public JsonResult GetAttractions(string place, int start, int count)
        {
            var attractions =  db.Attraction.Include(c => c.Coordinates).Include(c => c.Country).Include(a => a.Address);

            if(place!=null)
            {
                attractions =  attractions.Where(p => p.Address.City == place || p.Address.Region == place);
            }

            var attrs = attractions.OrderByDescending(x=>x.AvgRating).Skip(start).Take(count).Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                AvgRating = x.AvgRating.HasValue ? Math.Round(x.AvgRating.Value,2) : 0.0
            });

            return Json(new { attractions = attrs.ToList() }, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetAttractionsByIds?ids=id1,id2
        [WebMethod]
        public JsonResult GetAttractionsByIds(string ids, int start, int count)
        {
            List<Attraction> attractions;
            string[] Ids = ids.Split(',');
            int[] Ids_int = new int[Ids.Count()];
            for (int i = 0; i < Ids.Length; ++i)
                Ids_int[i] = int.Parse(Ids[i]);
            attractions = db.Attraction.Include(c => c.Coordinates).Include(c => c.Country).Include(a => a.Address).
                Where(a => Ids_int.Contains(a.ID)).ToList();
            foreach (var attr in attractions)
                attr.Description = Regex.Replace(attr.Description, @"<.*?>", string.Empty);
            //return Json(attractions, JsonRequestBehavior.AllowGet);
            var attrs = attractions.OrderByDescending(x => x.AvgRating).Skip(start).Take(count).Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                AvgRating = x.AvgRating.HasValue ? Math.Round(x.AvgRating.Value, 2) : 0.0
            });

            return Json(new { attractions = attrs.ToList() }, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetAttractions/2
        [WebMethod]
        public JsonResult GetAttraction(int id)
        {
            var attraction = db.Attraction.Include(a => a.Address).Include(i => i.Images).Include(c => c.Coordinates).
                Where(x => x.ID == id).SingleOrDefault();
            attraction.Description = Regex.Replace(attraction.Description, @"<.*?>", string.Empty);
            //return Json(attraction, JsonRequestBehavior.AllowGet);
            return Json(new { attraction = attraction }, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetAttractions?name=Name
        [WebMethod]
        public JsonResult GetAttractionByName(string name)
        {
            var attraction = db.Attraction.Include(c => c.Country).Include(a => a.Address).
                Where(x => x.Name == name).SingleOrDefault();
            attraction.Description = Regex.Replace(attraction.Description, @"<.*?>", string.Empty);
            //return Json(attraction, JsonRequestBehavior.AllowGet);
            return Json(new { attraction = attraction }, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetAttractionsByType
        [WebMethod]
        public JsonResult GetAttractionsByType(string type, int start, int count)
        {
            List<Attraction> attractions;
            if (type != null)
            {
                attractions = db.Attraction.Include(c => c.Coordinates).Include(c => c.Country).Include(a => a.Address).
                    Where(p => p.AttractionType.Name == type).ToList();
            }
            else
                attractions = db.Attraction.Include(c => c.Coordinates).Include(c => c.Country).Include(a => a.Address).ToList();
            foreach (var attr in attractions)
                attr.Description = Regex.Replace(attr.Description, @"<.*?>", string.Empty);
            //return Json(attractions, JsonRequestBehavior.AllowGet);
            var attrs = attractions.OrderByDescending(x => x.AvgRating).Skip(start).Take(count).Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                AvgRating = x.AvgRating.HasValue ? Math.Round(x.AvgRating.Value, 2) : 0.0
            });

            return Json(new { attractions = attrs.ToList() }, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetAttractionsByPlace
        [WebMethod]
        public JsonResult GetAttractionsByPlace(string name, int start, int count)
        {
            List<Attraction> attractions;
            if (name!=null)
            {
                attractions = db.Attraction.Include(c => c.Coordinates).Include(c => c.Country).Include(a => a.Address).
                    Where(p => p.Address.City == name || p.Address.Region == name).ToList();
            }
            else
                attractions = db.Attraction.Include(c => c.Coordinates).Include(c => c.Country).Include(a => a.Address).ToList();
            foreach (var attr in attractions)
                attr.Description = Regex.Replace(attr.Description, @"<.*?>", string.Empty);
            //return Json(attractions, JsonRequestBehavior.AllowGet);
            var attrs = attractions.OrderByDescending(x => x.AvgRating).Skip(start).Take(count).Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                AvgRating = x.AvgRating.HasValue ? Math.Round(x.AvgRating.Value, 2) : 0.0
            });

            return Json(new { attractions = attrs.ToList() }, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetAttractionsByCountry
        [WebMethod]
        public JsonResult GetAttractionsByCountry(string country, int start, int count)
        {
            List<Attraction> attractions;
            if (country != null)
            {
                attractions = db.Attraction.Include(c => c.Coordinates).Include(c => c.Country).Include(a => a.Address).
                    Where(p => p.Country.Name == country).ToList();
            }
            else
                attractions = db.Attraction.Include(c => c.Coordinates).Include(c => c.Country).Include(a => a.Address).ToList();
            foreach (var attr in attractions)
                attr.Description = Regex.Replace(attr.Description, @"<.*?>", string.Empty);
            //return Json(attractions, JsonRequestBehavior.AllowGet);
            var attrs = attractions.OrderByDescending(x => x.AvgRating).Skip(start).Take(count).Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                AvgRating = x.AvgRating.HasValue ? Math.Round(x.AvgRating.Value, 2) : 0.0
            });

            return Json(new { attractions = attrs.ToList() }, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetAttractionsByPlaceId/3
        [WebMethod]
        public JsonResult GetAttractionsByPlaceId(int start, int count, int id=-1)
        {
            var attractions = db.Attraction.Include(x => x.Address);
            if (id != -1)
            {
                var place = db.Place.Find(id);
                attractions = attractions.Where(p => p.Address.City == place.Name || p.Address.Region == place.Name);
            }
            //return Json(attractions, JsonRequestBehavior.AllowGet);
            var attrs = attractions.OrderByDescending(x => x.AvgRating).Skip(start).Take(count).Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                AvgRating = x.AvgRating.HasValue ? Math.Round(x.AvgRating.Value, 2) : 0.0
            });

            return Json(new { attractions = attrs.ToList() }, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetPlace/3
        [WebMethod]
        public JsonResult GetPlace(int id)
        {
            var place = db.Place.Find(id);
            place.Description = Regex.Replace(place.Description, @"<.*?>", string.Empty);
            //return Json(place, JsonRequestBehavior.AllowGet);
            return Json(new { place = place }, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetPlaces?country=Name
        [WebMethod]
        public JsonResult GetPlaces(string country, int start, int count)
        {
            var places = db.Place.Include(x => x.Country);
            if(country!=null)
                places = places.Where(p => p.Country.Name == country);
            //return Json(places, JsonRequestBehavior.AllowGet);
            var placs = places.Skip(start).Take(count).Select(x => new
            {
                ID = x.ID,
                Name = x.Name
            });
            return Json(new { places = places.ToList() }, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetCountry/3
        [WebMethod]
        public JsonResult GetCountry(int id)
        {
            var country = db.Country.Find(id);
            country.Description = Regex.Replace(country.Description, @"<.*?>", string.Empty);
            //return Json(country, JsonRequestBehavior.AllowGet);
            return Json(new { country = country }, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetCountries
        [WebMethod]
        public JsonResult GetCountries(int start, int count)
        {
            var countries = db.Country.Skip(start).Take(count).Select(x => new
            {
                ID = x.ID,
                Name = x.Name
            });
            //return Json(countries, JsonRequestBehavior.AllowGet);
            return Json(new { countries = countries.ToList() }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}