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
        public JsonResult GetAttractions(string place)
        {
            List<Attraction> attractions;
            if(place!=null)
                attractions = db.Attraction.Include(c => c.Coordinates).Include(c => c.Country).Include(a => a.Address).
                    Where(p => p.Address.City == place || p.Address.Region == place).ToList();
            else
                attractions = db.Attraction.Include(c => c.Coordinates).Include(c => c.Country).Include(a => a.Address).ToList();
            foreach(var attr in attractions)
                attr.Description = Regex.Replace(attr.Description, @"<.*?>", string.Empty);
            return Json(attractions, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetAttractionsByIds?ids=id1,id2
        [WebMethod]
        public JsonResult GetAttractionsByIds(string ids)
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
            return Json(attractions, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetAttractions/2
        [WebMethod]
        public JsonResult GetAttraction(int id)
        {
            var attraction = db.Attraction.Include(a => a.Address).Include(i => i.Images).Include(c => c.Coordinates).
                Where(x => x.ID == id).SingleOrDefault();
            attraction.Description = Regex.Replace(attraction.Description, @"<.*?>", string.Empty);
            return Json(attraction, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetAttractions?name=Name
        [WebMethod]
        public JsonResult GetAttractionByName(string name)
        {
            var attraction = db.Attraction.Include(c => c.Country).Include(a => a.Address).
                Where(x => x.Name == name).SingleOrDefault();
            attraction.Description = Regex.Replace(attraction.Description, @"<.*?>", string.Empty);
            return Json(attraction, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetAttractionsByPlaceId/3
        [WebMethod]
        public JsonResult GetAttractionsByPlace(string name)
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
            return Json(attractions, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetAttractionsByPlaceId/3
        [WebMethod]
        public JsonResult GetAttractionsByPlaceId(int id=-1)
        {
            List<AttractionViewModel> attractions;
            if (id != -1)
            {
                var place = db.Place.Find(id);
                attractions = db.Attraction.Where(p => p.Address.City == place.Name || p.Address.Region == place.Name).
                    Select(x => new AttractionViewModel() { ID = x.ID, Name = x.Name }).ToList();
            }
            else
            {
                attractions = db.Attraction.Select(x => new AttractionViewModel() { ID = x.ID, Name = x.Name }).ToList();
            }
            return Json(attractions, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetPlace/3
        [WebMethod]
        public JsonResult GetPlace(int id)
        {
            var place = db.Place.Find(id);
            place.Description = Regex.Replace(place.Description, @"<.*?>", string.Empty);
            return Json(place, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetPlaces?country=Name
        [WebMethod]
        public JsonResult GetPlaces(string country)
        {
            List<Place> places;
            if(country!=null)
                places = db.Place.Where(p => p.Country.Name == country).ToList();
            else
                places = db.Place.ToList();
            foreach (var p in places)
                p.Description = Regex.Replace(p.Description, @"<.*?>", string.Empty);
            return Json(places, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetCountry/3
        [WebMethod]
        public JsonResult GetCountry(int id)
        {
            var country = db.Country.Find(id);
            country.Description = Regex.Replace(country.Description, @"<.*?>", string.Empty);
            return Json(country, JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetCountries
        [WebMethod]
        public JsonResult GetCountries()
        {
            var countries = db.Country.ToList();
            foreach (var c in countries)
                c.Description = Regex.Replace(c.Description, @"<.*?>", string.Empty);
            return Json(countries, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}