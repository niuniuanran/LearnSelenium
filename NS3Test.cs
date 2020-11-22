using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System.Threading;
using System;

namespace seleniumtestns3
{
    public class NS3Test
    {
        public static void Main(string[] args)
        {
            IWebDriver driver;
            WebDriverWait wait;

            using (driver = new ChromeDriver())
            {
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                driver.Navigate().GoToUrl("https://ns3-notes.netlify.app/");
                wait.Until<bool>(((document) => document.Title.Contains("NS3")));

                var logInLink = driver.FindElement(By.LinkText("Login"));
                logInLink.Click();
                wait.Until<bool>((document) => document.Url.Contains("login"));

                var emailInput = driver.FindElement(By.Id("email"));
                emailInput.SendKeys("non-existing@test.com");
                var passwordInput = driver.FindElement(By.Id("password"));
                passwordInput.SendKeys("wrong-password");

                var loginButton = driver.FindElement(By.ClassName("LoaderButton"));
                loginButton.Submit();

                wait.Until(ExpectedConditions.AlertIsPresent());
                var alert = driver.SwitchTo().Alert();

                Assert.AreEqual(alert.Text, "User does not exist.");

                alert.Accept();

                emailInput.Clear();
                emailInput.SendKeys("admin@test.com");
                passwordInput.Clear();
                passwordInput.SendKeys("Passw0rd!");
                loginButton = driver.FindElement(By.ClassName("LoaderButton"));
                loginButton.Submit();

                wait.Until<bool>(document => !document.Url.Contains("login"));

                Thread.Sleep(120000);
            }
        }
    }
}