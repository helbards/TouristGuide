using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using TouristGuide;
using TouristGuide.Controllers;
using TouristGuide.Models;

namespace TouristGuide.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        HomeController controller;

        [TestFixtureSetUp]
        public void Setup()
        {
            controller = new HomeController();
        }

        [Test]
        public void Index()
        {
            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Asserts
            Assert.IsNotNull(result);
            var model = result.Model;
            Assert.IsNotNull(model);
        }

        [Test]
        public void About()
        {
            // Act
            ViewResult result = controller.About() as ViewResult;

            // Asserts
            Assert.IsNotNull(result);
            var model = result.Model;
            Assert.IsNull(model);
        }
    }
}
