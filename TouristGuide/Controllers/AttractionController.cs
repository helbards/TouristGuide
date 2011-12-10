using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristGuide.Models;
using System.Web.Security;
using TouristGuide.Helpers;

namespace TouristGuide.Controllers
{ 
    public class AttractionController : Controller
    {
        private TouristGuideDB db = new TouristGuideDB();


        #region Attractions
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
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.AttractionTypes = DbHelpers.GetAttractionTypesToList();
            ViewBag.Countries = DbHelpers.GetCountriesToList();
            return View();
        }

        //
        // POST: /Attraction/Create
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            ViewBag.AttractionTypes = DbHelpers.GetAttractionTypesToList();
            ViewBag.Countries = DbHelpers.GetCountriesToList();
            Attraction attraction = db.Attraction.Find(id);
            return View(attraction);
        }

        //
        // POST: /Attraction/Edit/5
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            Attraction attraction = db.Attraction.Find(id);
            return View(attraction);
        }

        //
        // POST: /Attraction/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Attraction attraction = db.Attraction.Find(id);
            //remove reviews
            var deleteAttractionReviews =
                    from attractions in db.AttractionReview
                    where attractions.AttractionID == id
                    select attractions;

            foreach (var review in deleteAttractionReviews)
            {
                db.AttractionReview.Remove(review);
            }
            //---
            db.Attraction.Remove(attraction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion Attractions

        #region Reviews
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
        [Authorize]
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
        [Authorize]
        public ActionResult ReviewEdit(int id)
        {
            
            AttractionReview review = db.AttractionReview.Find(id);
            if (!review.Author.Equals(Membership.GetUser().UserName) && !Roles.IsUserInRole(Membership.GetUser().UserName, "admin"))
                return RedirectToAction("Details", new { id = review.AttractionID });
            ViewBag.Attraction = db.Attraction.Find(review.AttractionID);
            return View(review);
        }

        //
        // POST: /Attraction/ReviewEdit/5
        [Authorize]
        [HttpPost]
        public ActionResult ReviewEdit(AttractionReview review)
        {
            if (!review.Author.Equals(Membership.GetUser().UserName) && !Roles.IsUserInRole(Membership.GetUser().UserName, "admin"))
                return RedirectToAction("Details", new { id = review.AttractionID });
            db.Entry(review).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = review.AttractionID });
        }

        //
        // GET: /Attraction/ReviewDelete/5
        [Authorize]
        public ActionResult ReviewDelete(int id)
        {
            AttractionReview review = db.AttractionReview.Find(id);
            if (!review.Author.Equals(Membership.GetUser().UserName) && !Roles.IsUserInRole(Membership.GetUser().UserName, "admin"))
                return RedirectToAction("Details", new { id = review.AttractionID });
            ViewBag.Attraction = db.Attraction.Find(review.AttractionID);
            return View(review);
        }

        //
        // POST: /Attraction/ReviewDelete/5
        [Authorize]
        [HttpPost, ActionName("ReviewDelete")]
        public ActionResult ReviewDeleteConfirmed(int id)
        {
            AttractionReview review = db.AttractionReview.Find(id);
            if (!review.Author.Equals(Membership.GetUser().UserName) && !Roles.IsUserInRole(Membership.GetUser().UserName, "admin"))
                return RedirectToAction("Details", new { id = review.AttractionID });
            db.AttractionReview.Remove(review);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = review.AttractionID });
        }

        #endregion Reviews

        #region Helpers

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        #endregion Helpers
    }

}