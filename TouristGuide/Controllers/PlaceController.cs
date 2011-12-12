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

        public ViewResult Index()
        {
            return View(db.Place.Include(c => c.Country).Include(c => c.Coordinates).ToList());
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
            //place.Country = db.Country.Find(place.Country.ID);
            ////place.Country = db.Country.Find(CountryId);
            //if (ModelState.IsValid)
            //{
            //    db.Entry(place).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.Countries = DbHelpers.GetCountriesToList();
            //return View(place);
            var updatedPlace = db.Place.Include(c => c.Country).Include(c => c.Coordinates).Where(p => p.ID == place.ID).SingleOrDefault();
            updatedPlace.Coordinates.Latitude = double.Parse(Request.Form["Coordinates.Latitude"]); // place.Coordinates.Latitude;
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