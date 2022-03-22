using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using Microsoft.VisualBasic;

namespace Museum.Tests.UITests.Base
{
    public class BaseTest
    {
        public TestContext TestContext { get; set; }
        protected IWebDriver driver;

        [TestInitialize]
        public void SetUp() { 

            var options = new ChromeOptions();
            options.AddArgument("window-size=1920,1080");
            options.AddArgument("force-device-scale-factor=1");
            options.AddArgument("high-dpi-support=1");
            options.AddArguments("enable-automation");
            options.AddArgument("--browser.helperApps.neverAsk.saveToDisk");
            options.AddArguments("--no-sandbox");

            options.PageLoadStrategy = PageLoadStrategy.Eager;
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://localhost:3000/");
        }

        [TestCleanup]
        public void TearDownBrowser()
        {
            driver.Quit();
        }

    }
}
