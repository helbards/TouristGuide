using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristGuide.Models;

namespace TouristGuide.Controllers
{
    public class HomeController : Controller
    {
        TouristGuideDB db = new TouristGuideDB();

        public ActionResult Index()
        {
            var news = db.News.Where(n => n.Date <= DateTime.Now).OrderByDescending(n => n.Date);

            return View(news);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Mobile()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}
