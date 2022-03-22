using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Museum.Tests.UITests.Base;
using Museum.Tests.UITests.MuseumPage;
using OpenQA.Selenium.Support.UI;

namespace Museum.Tests.UITests.Test
{
    [TestClass]
    public class MuseumTest : BaseTest
    {
        private HomePage homePage;
        private AddMuseumPage museumPage;
        private AllMuseumsPage allMuseumPage;

        WebDriverWait wait;

        [TestMethod]
        public void CheckSiteTitle_Test()
        {
            homePage = new HomePage(driver);

            driver.Navigate().GoToUrl("http://localhost:3000/");
            Assert.AreEqual(driver.Title,"Muzej");
           
        }

        [TestMethod]
        public void AddNewMuseum_Test()
        {
            homePage = new HomePage(driver);
            museumPage = new AddMuseumPage(driver);
            allMuseumPage = new AllMuseumsPage(driver);
            
            driver.Navigate().GoToUrl("http://localhost:3000/");
            Assert.AreEqual(driver.Title, "Muzej");

            homePage.PerformAddNewMuseum();
            Assert.AreEqual(driver.Url, "http://localhost:3000/addmuseum");
            Assert.AreEqual(museumPage.AddMuseumPageHeader.Text, "DODAJ NOVI MUZEJ");

            museumPage.Perform_AddNewMuseum("Draza Muzej", "Daleki Istok", "Beograd", "email@rks.rs", "+38165555874");
            Assert.IsNotNull(allMuseumPage.CardBodyDisplay);


        }

        [TestMethod]
        public void Check_DeleteButtonExists_Test()
        {
            homePage = new HomePage(driver);
            allMuseumPage = new AllMuseumsPage(driver);

            driver.Navigate().GoToUrl("http://localhost:3000/");
            Assert.AreEqual(driver.Title, "Muzej");
            
            homePage.PerformShowMuseumsClick();
            Assert.AreEqual(driver.Url, "http://localhost:3000/museums");

            var count = allMuseumPage.CardMuseumTitle.Count;
            
            Assert.AreEqual(allMuseumPage.DeleteMuseumButton.Text, "Obrisi muzej");
        }

    }
}
