using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using TouristGuide;
using TouristGuide.Controllers;
using TouristGuide.Models;

namespace TouristGuide.Tests
{
    
    [TestFixture]
    public class NewsControllerTest
    {
        NewsController controller;

        [TestFixtureSetUp]
        public void Setup()
        {
            controller = new NewsController();
        }

        [Test]
        public void CreateTest()
        {
            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Asserts
            Assert.IsNotNull(result);
            var model = result.Model;
            Assert.IsNotNull(model);
            Assert.IsInstanceOf<NewsTimeViewModel>(model);
            Assert.IsNotNull(((NewsTimeViewModel)model).News);
        }

        [Test]
        public void CreateTest1()
        {
            // Act
            NewsTimeViewModel newNews = new NewsTimeViewModel();
            try
            {
                ViewResult result = controller.Create(newNews) as ViewResult;
                Assert.Fail();
            }
            catch (NullReferenceException)
            {
                Assert.Pass();
            }
        }

        //[Test]
        //public void DeleteTest()
        //{
        //    NewsController target = new NewsController(); // TODO: Initialize to an appropriate value
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    FormCollection collection = null; // TODO: Initialize to an appropriate value
        //    RedirectToRouteResult expected = null; // TODO: Initialize to an appropriate value
        //    RedirectToRouteResult actual;
        //    actual = target.Delete(id, collection);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        //[Test]
        //public void DeleteTest1()
        //{
        //    NewsController target = new NewsController(); // TODO: Initialize to an appropriate value
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    ActionResult expected = null; // TODO: Initialize to an appropriate value
        //    ActionResult actual;
        //    actual = target.Delete(id);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        //[Test]
        //public void DetailsTest()
        //{
        //    NewsController target = new NewsController(); // TODO: Initialize to an appropriate value
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    ActionResult expected = null; // TODO: Initialize to an appropriate value
        //    ActionResult actual;
        //    actual = target.Details(id);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        //[Test]
        //public void EditTest()
        //{
        //    NewsController target = new NewsController(); // TODO: Initialize to an appropriate value
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    ActionResult expected = null; // TODO: Initialize to an appropriate value
        //    ActionResult actual;
        //    actual = target.Edit(id);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        //[Test]
        //public void EditTest1()
        //{
        //    NewsController target = new NewsController(); // TODO: Initialize to an appropriate value
        //    NewsTimeViewModel model = null; // TODO: Initialize to an appropriate value
        //    ActionResult expected = null; // TODO: Initialize to an appropriate value
        //    ActionResult actual;
        //    actual = target.Edit(model);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        //[Test]
        //public void IndexTest()
        //{
        //    NewsController target = new NewsController(); // TODO: Initialize to an appropriate value
        //    ActionResult expected = null; // TODO: Initialize to an appropriate value
        //    ActionResult actual;
        //    actual = target.Index();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}
    }
}
