using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristGuide.Models;
using System.Web.Services;

namespace TouristGuide.Controllers
{ 
    public class WebServiceController : Controller
    {
        private TouristGuideDB db = new TouristGuideDB();

        // GET: /WebService/GetAttractions
        [WebMethod]
        public JsonResult GetAttractions(string place)
        {
            List<Attraction> attractions;
            if(place!=null)
                attractions = db.Attraction.Where(p => p.Address.City == place || p.Address.Region == place).ToList();
            else
                attractions = db.Attraction.ToList();
            return Json(attractions, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        public JsonResult GetAttraction(int id)
        {
            return Json(db.Attraction.Find(id), JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetAttractions
        [WebMethod]
        public JsonResult GetAttractionsByPlaceId(int id=-1)
        {
            List<Attraction> attractions;
            if (id != -1)
            {
                var place = db.Place.Find(id);
                attractions = db.Attraction.Where(p => p.Address.City == place.Name || p.Address.Region == place.Name).ToList();
            }
            else
                attractions = db.Attraction.ToList();
            return Json(attractions, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        public JsonResult GetPlace(int id)
        {
            return Json(db.Place.Find(id), JsonRequestBehavior.AllowGet);
        }

        // GET: /WebService/GetAttractions
        [WebMethod]
        public JsonResult GetPlaces(string country)
        {
            List<Place> places;
            if(country!=null)
                places = db.Place.Where(p => p.Country.Name == country).ToList();
            else
                places = db.Place.ToList();
            return Json(places, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        public JsonResult GetCountry(int id)
        {
            return Json(db.Country.Find(id), JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        public JsonResult GetCountries(string country)
        {
            var countries = db.Country.ToList();
            return Json(countries, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}