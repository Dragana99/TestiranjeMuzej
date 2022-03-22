using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Tests.UITests.MuseumPage
{
    public class HomePage
    {
        private By museumDropdown = By.Id("collasible-nav-dropdown");
        private By buttonShowMuseums = By.Id("prikaz");
        private By buttonAddMuseums = By.Id("dodavanjeMuzeja");
        private By allMuseumsEditButton = By.Id("izmeniMuzej");
        private By allMuseumsDeleteButton = By.Id("obrisiMuzej");
        //private By editMuseumPageHeader = By.Xpath("//h1[@class="form-header"]"); - druga str

        private IWebDriver driver;
        WebDriverWait driverWait;

        public HomePage(IWebDriver driver)
        {
            this.driver = driver;
            driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        public IWebElement MuseumDropDown
        {
            get
            {
                return driverWait.Until(driver => driver.FindElement(museumDropdown));
            }
        }

        public IWebElement ShowMuseumsLink
        {
            get
            {
                return driverWait.Until(driver => driver.FindElement(buttonShowMuseums));

            }
        }

        public IWebElement AddMuseumsLink
        {
            get
            {
                return driverWait.Until(driver => driver.FindElement(buttonAddMuseums));

            }
        }

        public void PerformShowMuseumsClick()
        {
            MuseumDropDown.Click();
            ShowMuseumsLink.Click();
        }
        public void PerformAddNewMuseum()
        {
            MuseumDropDown.Click();
            AddMuseumsLink.Click();
        }
    }
}
