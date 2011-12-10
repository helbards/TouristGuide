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
            return View(db.Place.ToList());
        }

        //
        // GET: /Place/Details/5

        public ViewResult Details(int id)
        {
            Place place = db.Place.Find(id);
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
            Place place = db.Place.Find(id);
            return View(place);
        }

        //
        // POST: /Place/Edit/5
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit(Place place)
        {
            if (ModelState.IsValid)
            {
                db.Entry(place).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(place);
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