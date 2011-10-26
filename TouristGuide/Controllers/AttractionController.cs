using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristGuide.Models;

namespace TouristGuide.Controllers
{ 
    public class AttractionController : Controller
    {
        private TouristGuideDB db = new TouristGuideDB();

        //
        // GET: /Attraction/

        public ViewResult Index()
        {
            return View(db.Attraction.ToList());
        }

        //
        // GET: /Attraction/Details/5

        public ViewResult Details(int id)
        {
            Attraction attraction = db.Attraction.Find(id);
            attraction.Reviews = db.AttractionReview.Where(a => a.AttractionID == id).ToList();
            return View(attraction);
        }

        //
        // GET: /Attraction/Create

        public ActionResult Create()
        {
            ViewBag.AttractionTypes = GetAttractionTypesToList();
            ViewBag.Countries = GetCountriesToList();
            return View();
        }

        //
        // POST: /Attraction/Create

        [HttpPost]
        public ActionResult Create(Attraction attraction)
        {
            attraction.AttractionType = db.AttractionType.Find(attraction.AttractionType.ID);
            attraction.Country = db.Country.Find(attraction.Country.ID);
            db.Attraction.Add(attraction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        //
        // GET: /Attraction/Edit/5
 
        public ActionResult Edit(int id)
        {
            ViewBag.AttractionTypes = GetAttractionTypesToList();
            ViewBag.Countries = GetCountriesToList();
            Attraction attraction = db.Attraction.Find(id);
            return View(attraction);
        }

        //
        // POST: /Attraction/Edit/5

        [HttpPost]
        public ActionResult Edit(Attraction attraction)
        {
            attraction.AttractionType = db.AttractionType.Find(attraction.AttractionType.ID);
            attraction.Country = db.Country.Find(attraction.Country.ID);
            db.Entry(attraction).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Attraction/Delete/5
 
        public ActionResult Delete(int id)
        {
            Attraction attraction = db.Attraction.Find(id);
            return View(attraction);
        }

        //
        // POST: /Attraction/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Attraction attraction = db.Attraction.Find(id);
            db.Attraction.Remove(attraction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        ////
        //// GET: /Attraction/

        //public ViewResult ReviewIndex()
        //{
        //    return View(db.Attraction.ToList());
        //}

        ////
        //// GET: /Attraction/Details/5

        //public ViewResult ReviewDetails(int id)
        //{
        //    Attraction attraction = db.Attraction.Find(id);
        //    attraction.Reviews = db.AttractionReview.Where(a => a.AttractionID == id).ToList();
        //    return View(attraction);
        //}

        //
        // POST: /Attraction/ReviewCreate

        [HttpPost]
        public ActionResult ReviewCreate(AttractionReview review)
        {
            review.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.AttractionReview.Add(review);
                db.SaveChanges();                
            }
            return RedirectToAction("Details", new { id = review.AttractionID });
        }

        //
        // GET: /Attraction/ReviewEdit/5

        public ActionResult ReviewEdit(int id)
        {
            AttractionReview review = db.AttractionReview.Find(id);
            ViewBag.Attraction = db.Attraction.Find(review.AttractionID);
            return View(review);
        }

        //
        // POST: /Attraction/ReviewEdit/5

        [HttpPost]
        public ActionResult ReviewEdit(AttractionReview review)
        {
            db.Entry(review).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = review.AttractionID });
        }

        //
        // GET: /Attraction/ReviewDelete/5

        public ActionResult ReviewDelete(int id)
        {
            Attraction attraction = db.Attraction.Find(id);
            return View(attraction);
        }

        //
        // POST: /Attraction/ReviewDelete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult ReviewDeleteConfirmed(int id)
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

        public List<SelectListItem> GetAttractionTypesToList()
        {
            List<SelectListItem> attractionTypes = new List<SelectListItem>();
            var allAttractionTypes = db.AttractionType;
            foreach (var item in allAttractionTypes)
            {
                attractionTypes.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.ID.ToString()
                });
            }
            return attractionTypes;
        }

        public List<SelectListItem> GetCountriesToList()
        {
            List<SelectListItem> countries = new List<SelectListItem>();
            var allCountries = db.Country;
            foreach (var item in allCountries)
            {
                countries.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.ID.ToString()
                });
            }
            return countries;
        }
    }

}