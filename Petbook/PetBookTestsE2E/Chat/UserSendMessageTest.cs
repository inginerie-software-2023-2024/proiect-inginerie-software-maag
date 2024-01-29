using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace PetBookTestsE2E.Chat
{
    public class UserSendMessageTest
    {
        private WebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
        }

        [Test]
        public void LogInAndSendMessage()
        {
            driver.Navigate().GoToUrl("https://localhost:7108");
            Login("user@test.com", "User1!");

            AssertHelloUserIsDisplayed();

            OpenSearchTab();

            SearchText("Mary");

            FollowUser();

            OpenChat("Mary");

            SendMessage("Hello");
        }

        private void Login(string email, string password)
        {
            IWebElement loginButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'Login')]")));
            loginButton.Click();

            IWebElement inputField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='Input_Email']")));
            inputField.SendKeys(email);

            IWebElement passwordField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='Input_Password']")));
            passwordField.SendKeys(password);

            IWebElement submitButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='login-submit']")));
            submitButton.Click();
        }

        private void AssertHelloUserIsDisplayed()
        {
            IWebElement helloUserElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(), 'Hello User!')]")));
            Assert.IsNotNull(helloUserElement, "Hello User text is not displayed on the page.");
        }

        private void OpenSearchTab()
        {
            IWebElement searchButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(), ' Search')]")));
            searchButton.Click();
        }

        private void SearchText(string searchText)
        {
            IWebElement searchInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("search")));
            searchInput.SendKeys(searchText);

            IWebElement searchButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@type='submit' and text()='Search']")));
            searchButton.Click();

            IWebElement maryElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a//*[text()='Mary'] | //li//*[text()='Mary']")));
            Assert.IsNotNull(maryElement, "Search was unsuccessful.");
            maryElement.Click();
        }

        private void FollowUser()
        {
            IWebElement followButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(), 'Follow user')]")));
            followButton.Click();
        }

        private void OpenChat(string user)
        {
            IWebElement messagesButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(), ' Messages')]")));
            messagesButton.Click();
            SearchText(user);
        }

        private void SendMessage(string message)
        {
            IWebElement messageInput = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='messageInput']")));
            messageInput.SendKeys(message);
            IWebElement sendButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[text()='Send']")));
            sendButton.Click();
            IWebElement messageElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//*[@id='chatBox']//*[contains(text(), '{message}')]")));
            Assert.IsNotNull(messageElement, "Message was not sent.");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Close();
            driver.Quit();
        }
    }
}