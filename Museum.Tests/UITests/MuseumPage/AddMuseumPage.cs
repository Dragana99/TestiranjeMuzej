using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Tests.UITests.MuseumPage
{
    public class AddMuseumPage
    {
        private By buttonAddMuseums = By.Id("dodavanjeMuzeja");
        private By nameLabel = By.Id("name");
        private By addressLabel = By.Id("address");
        private By cityLabel = By.Id("city");
        private By phoneLabel = By.Id("phone");
        private By emailLabel = By.Id("email");
        private By buttonAdd = By.Id("btnDodaj");
        private By addMuseumPageHeader = By.XPath("//h1[@class='form-header']"); 

        private IWebDriver driver;
        WebDriverWait driverWait;

        public AddMuseumPage(IWebDriver driver)
        {
            this.driver = driver;
            driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        public IWebElement AddMuseumPageHeader
        {
            get
            {
                return driverWait.Until(driver => driver.FindElement(addMuseumPageHeader));
            }
        }

        public IWebElement NameLabel
        {
            get
            {
                return driverWait.Until(driver => driver.FindElement(nameLabel));

            }
        }

        public IWebElement AddressLabel
        {
            get
            {
                return driverWait.Until(driver => driver.FindElement(addressLabel));

            }
        }

        public IWebElement CityLabel
        {
            get
            {
                return driverWait.Until(driver => driver.FindElement(cityLabel));

            }
        }
        public IWebElement EmailLabel
        {
            get
            {
                return driverWait.Until(driver => driver.FindElement(emailLabel));

            }
        }
        public IWebElement PhoneLabel
        {
            get
            {
                return driverWait.Until(driver => driver.FindElement(phoneLabel));

            }
        }
        public IWebElement ButtonAdd
        {
            get
            {
                return driverWait.Until(driver => driver.FindElement(buttonAdd));

            }
        }

        public void Perform_AddNewMuseum(string n,string a, string c, string e, string p)
        {
            NameLabel.SendKeys(n);
            AddressLabel.SendKeys(a);
            CityLabel.SendKeys(c);
            EmailLabel.SendKeys(e);
            PhoneLabel.SendKeys(p);

            //btn click
            ButtonAdd.Click();
        }
    }
}
