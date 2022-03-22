using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Tests.UITests.MuseumPage
{
    public class AllMuseumsPage
    {
        private By cardBodyDisplay = By.Id("sviMuzeji");
        private By deleteMuseumBtn = By.Id("obrisiMuzej");
        private By cardMuseumTitle = By.XPath("//*[@class='card - title h5']");

        private IWebDriver driver;
        WebDriverWait driverWait;

        public AllMuseumsPage(IWebDriver driver)
        {
            this.driver = driver;
            driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        public IWebElement CardBodyDisplay
        {
            get
            {
                return driverWait.Until(driver => driver.FindElement(cardBodyDisplay));
            }
        }

        public IWebElement DeleteMuseumButton
        {
            get
            {
                return driverWait.Until(driver => driver.FindElement(deleteMuseumBtn));
            }
        }

        public IList<IWebElement> CardMuseumTitle
        {
            get
            {
                return driverWait.Until(driver => driver.FindElements(cardMuseumTitle));
            }
            
         }

        
    }
}
