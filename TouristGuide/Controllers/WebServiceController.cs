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

        // GET: /WebService/GetAttractions
        [WebMethod]
        public JsonResult GetPlaces(string country)
        {
            var places = db.Place.Where(p => p.Country.Name == country).ToList();
            return Json(places, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        public JsonResult GetCountries()
        {
            var countries = db.Country.Select(c => c.Name).ToList();
            return Json(countries, JsonRequestBehavior.AllowGet);
        }

//
        // GET: /WebService/

        public ViewResult Index()
        {
            return View(db.Attraction.ToList());
        }

        //
        // GET: /WebService/Details/5

        public ViewResult Details(int id)
        {
            Attraction attraction = db.Attraction.Find(id);
            return View(attraction);
        }

        //
        // GET: /WebService/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /WebService/Create

        [HttpPost]
        public ActionResult Create(Attraction attraction)
        {
            if (ModelState.IsValid)
            {
                db.Attraction.Add(attraction);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(attraction);
        }
        
        //
        // GET: /WebService/Edit/5
 
        public ActionResult Edit(int id)
        {
            Attraction attraction = db.Attraction.Find(id);
            return View(attraction);
        }

        //
        // POST: /WebService/Edit/5

        [HttpPost]
        public ActionResult Edit(Attraction attraction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attraction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(attraction);
        }

        //
        // GET: /WebService/Delete/5
 
        public ActionResult Delete(int id)
        {
            Attraction attraction = db.Attraction.Find(id);
            return View(attraction);
        }

        //
        // POST: /WebService/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Attraction attraction = db.Attraction.Find(id);
            db.Attraction.Remove(attraction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}