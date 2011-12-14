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
        public ITouristGuideDB db;

        public HomeController()
        {
            db = new TouristGuideDB();
        }

        public HomeController(ITouristGuideDB dbContext)
        {
            db = dbContext;
        }

        public ActionResult Index(int start=1)
        {
            var news = db.News.Where(n => n.Date <= DateTime.Now).OrderByDescending(n => n.Date).ToList();

            int newsCount = news.Count();
            if (start > newsCount)
                start = newsCount;
            if (start < 1)
                start = 1;

            ViewBag.PagesCount = newsCount / 10 + (newsCount % 10 > 0 ? 1 : 0);
            ViewBag.CurrentPage = ((start - 1) / 10) + 1;

            news = news.Skip(start - 1).Take(10).ToList();

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
