using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristGuide.Models;
using TouristGuide.Helpers;

namespace TouristGuide.Controllers
{ 
    public class PlaceController : Controller
    {
        private TouristGuideDB db = new TouristGuideDB();

        //
        // GET: /Place/

        public ViewResult Index(int start = 0, int count = 20)
        {
            List<Place> places = FilterPlaces(null, null, start, count);
            return View(places);
        }

        private List<Place> FilterPlaces(string country, string place, int start, int count)
        {
            IQueryable<Place> places = db.Place.Include(c => c.Country).Include(c => c.Coordinates);

            if (country != null && country != "")
            {
                places = places.Where(a => a.Country != null && a.Country.Name.Contains(country));
            }
            if (place != null && place != "")
            {
                places = places.Where(a => a.Name.Contains(place));
            }

            return places.OrderBy(x => x.Name).Skip(start).Take(count).ToList();
        }

        public ViewResult GetPlaces(string country, string place, int start, int count)
        {
            List<Place> places = FilterPlaces(country, place, start, count);
            return View(places);
        }

        //
        // GET: /Place/Details/5

        public ViewResult Details(int id)
        {
            Place place = db.Place.Include(c => c.Country).Include(c => c.Coordinates).Where(p => p.ID == id).SingleOrDefault();
            return View(place);
        }

        //
        // GET: /Place/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.Countries = DbHelpers.GetCountriesToList();
            return View();
        } 

        //
        // POST: /Place/Create
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create(Place place)
        {
            place.Country = db.Country.Find(place.Country.ID);
            db.Place.Add(place);
            db.SaveChanges();
            return RedirectToAction("Index");  
        }
        
        //
        // GET: /Place/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            ViewBag.Countries = DbHelpers.GetCountriesToList();
            Place place = db.Place.Include(c => c.Country).Include(c => c.Coordinates).Where(p => p.ID == id).SingleOrDefault();
            return View(place);
        }

        //
        // POST: /Place/Edit/5
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit(Place place)//, int CountryId)
        {
            //place.Country = db.Country.Find(CountryId);
            ////place.Country.ID = CountryId;
            //if (ModelState.IsValid)
            //{
            //    UpdateModel(place);
            //    //db.Entry(place).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.Countries = DbHelpers.GetCountriesToList();
            //return View(place);
            var updatedPlace = db.Place.Include(c => c.Country).Include(c => c.Coordinates).Where(p => p.ID == place.ID).SingleOrDefault();
            updatedPlace.Coordinates.Latitude = place.Coordinates.Latitude;
            updatedPlace.Coordinates.Longitude = place.Coordinates.Longitude;
            updatedPlace.Country = db.Country.Find(place.Country.ID);
            updatedPlace.Description = place.Description;
            updatedPlace.Name = place.Name;
            db.Entry(updatedPlace).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Place/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            Place place = db.Place.Find(id);
            return View(place);
        }

        //
        // POST: /Place/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Place place = db.Place.Find(id);
            db.Place.Remove(place);
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