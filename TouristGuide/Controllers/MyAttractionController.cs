using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristGuide.Models;
using TouristGuide.Providers.Database;

namespace TouristGuide.Controllers
{ 
    public class MyAttractionController : Controller
    {
        private TouristGuideDB db = new TouristGuideDB();
        private IDataProvider users = new SqlProvider();

        //
        // GET: /MyAttraction/

        public ViewResult Index()
        {
            var id = users.GetUserByLogin(HttpContext.User.Identity.Name).UserId;
            var myAttractions = db.MyAttractions.Where(x => x.UserId == id);
            return View();
        }

        //
        // POST: /MyAttraction/Create

        [HttpPost]
        public ActionResult Create(MyAttraction myattraction)
        {
            if (ModelState.IsValid)
            {
                db.MyAttractions.Add(myattraction);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(myattraction);
        }

        //
        // POST: /MyAttraction/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            MyAttraction myattraction = db.MyAttractions.Find(id);
            db.MyAttractions.Remove(myattraction);
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