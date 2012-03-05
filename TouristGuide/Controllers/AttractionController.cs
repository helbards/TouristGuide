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
using System.IO;

namespace TouristGuide.Controllers
{ 
    public class AttractionController : Controller
    {
        private TouristGuideDB db = new TouristGuideDB();


        #region Attractions
        //
        // GET: /Attraction/

        public ViewResult Index(string country, string place, int start = 0, int count = 20)
        {
            List<Attraction> attractions = FilterAttractions(country, place, start, count);
            return View(attractions);
        }

        private List<Attraction> FilterAttractions(string country, string place, int start, int count)
        {
            IQueryable<Attraction> attractions;

            if (country != null && place != null)
            {
                attractions = db.Attraction.Where(a => a.Country.Name.Contains(country)).Where(a => a.Address.City.Contains(place) || a.Address.Region.Contains(place));
            }
            else if (country != null && place == null)
            {
                attractions = db.Attraction.Where(a => a.Country.Name.Contains(country));
            }
            else if (country == null && place != null)
            {
                attractions = db.Attraction.Where(a => a.Address.City.Contains(place) || a.Address.Region.Contains(place));
            }
            else
            {
                attractions = db.Attraction;
            }
            
            return attractions.OrderBy(x=>x.ID).Skip(start).Take(count).ToList();
        }

        public ViewResult GetAttractions(string country, string place, int start, int count)
        {
            List<Attraction> attractions = FilterAttractions(country, place, start, count);
            return View(attractions);
        }

        //
        // GET: /Attraction/Details/5

        public ViewResult Details(int id)
        {
            //Attraction attraction = db.Attraction.Find(id);
            //attraction.Reviews = db.AttractionReview.Where(a => a.AttractionID == id).ToList();
            //attraction.Images = db.AttractionImage.Where(a => a.AttractionID == id).ToList();
            var attraction = db.Attraction.Include(r => r.Reviews).Include(i => i.Images).Include(a => a.Address).Include(c => c.Coordinates).Include(c => c.Country)
                .Where(a => a.ID == id).SingleOrDefault();
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

            //upload photos
            try
            {
                int i = 0;
                foreach (string upload in Request.Files)
                {
                    if (Request.Files[i].ContentLength == 0)
                        continue;

                    string path = AppDomain.CurrentDomain.BaseDirectory + "Content/AttractionImages/";
                    string ext = Request.Files[i].FileName.Substring(Request.Files[i].FileName.LastIndexOf('.'));
                    AttractionImage ai = new AttractionImage { AttractionID = attraction.ID, FileName = generateRandomString(32) + ext };
                    attraction.Images.Add(ai);
                    Request.Files[i].SaveAs(Path.Combine(path, ai.FileName));
                    i++;
                }
            }
            catch (NullReferenceException)
            {
                //no photo
            }
            //end of upload user photo

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
            Attraction attraction = db.Attraction.Include(c => c.Country).Include(t => t.AttractionType).Include(c => c.Coordinates).Include(a => a.Address).Where(a => a.ID == id).SingleOrDefault();
            return View(attraction);
        }

        //
        // POST: /Attraction/Edit/5
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit(Attraction attraction)
        {
            var updatedAttraction = db.Attraction.Include(a => a.AttractionType).Include(c => c.Country).Include(c => c.Coordinates).Include(a => a.Address).Where(x => x.ID == attraction.ID).SingleOrDefault();
            updatedAttraction.AttractionType = db.AttractionType.Find(attraction.AttractionType.ID);
            updatedAttraction.Country = db.Country.Find(attraction.Country.ID);
            updatedAttraction.Coordinates.Latitude = attraction.Coordinates.Latitude;
            updatedAttraction.Coordinates.Longitude = attraction.Coordinates.Longitude;
            updatedAttraction.Address = attraction.Address;
            db.Entry(updatedAttraction).State = EntityState.Modified;
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

            //remove images
            var deleteAttractionImages =
                    from attractions in db.AttractionImage
                    where attractions.AttractionID == id
                    select attractions;
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "Content/AttractionImages/";
                foreach (var img in deleteAttractionImages)
                {
                    FileInfo TheFile = new FileInfo(path + img.FileName);
                    if (TheFile.Exists)
                    {
                        TheFile.Delete();
                    }
                    db.AttractionImage.Remove(img);
                }
            }
            catch (NullReferenceException)
            {
                //no images
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

        public String generateRandomString(int length)
        {
            //Initiate objects & vars    
            Random random = new Random();
            String randomString = "";
            int randNumber;

            //Loop ‘length’ times to generate a random number or character
            for (int i = 0; i < length; i++)
            {
                if (random.Next(1, 3) == 1)
                    randNumber = random.Next(97, 123); //char {a-z}
                else
                    randNumber = random.Next(48, 58); //int {0-9}

                //append random char or digit to random string
                randomString = randomString + (char)randNumber;
            }
            //return the random string
            return randomString;
        }

        #endregion Helpers
    }

}