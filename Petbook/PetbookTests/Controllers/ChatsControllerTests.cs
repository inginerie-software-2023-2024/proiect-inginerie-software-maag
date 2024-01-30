using EntityFrameworkCore.Testing.Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Petbook.Controllers;
using Petbook.Data;
using Petbook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Petbook.Controllers.Tests
{
    [TestClass()]
    public class ChatsControllerTests
    {
        private static ChatsController controller;
        private static ApplicationDbContext db;

        [TestInitialize]
        public void SetUp()
        {
            db = GetDbContext();
            controller = GetInstanceController();
        }


        public ChatsController GetInstanceController()
        {
            if (controller == null)
            {
                controller = GetController();
            }
            return controller;
        }


        public ApplicationDbContext GetDbContext()
        {
            if (db == null)
            {
                // create a mock db 
                db = Create.MockedDbContextFor<ApplicationDbContext>();
                db.PopulateDb(10);
            }
            return db;
        }

        private ChatsController GetController()
        {
            // create a mock admin user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, "admin"),
                new Claim(ClaimTypes.Name, "admin@admin.com"),
                new Claim(ClaimTypes.Role, "Admin")
            }, "TestAuthentication"));

            // create a mock controller with a session variable
            var httpContext = new DefaultHttpContext { User = user };
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["SessionVariable"] = "admin";
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var roleManager = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);
            var controller = new ChatsController(db, userManager.Object, roleManager.Object) { TempData = tempData };
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = httpContext;
            return controller;
        }


        [TestMethod()]
        public void AddMessageTest()
        {
            // Get the initial count of messages in the database
            var initialMessageCount = db.Messages.Count();

            var newMessage = new Message()
            {
                ChatId = 1,
                UserId = "8e445865-a24d-4543-a6c6-9443d048cdb0",
                SendDate = DateTime.Now,
                MessageText = "Test"
            };

            var result = controller.AddMessage(newMessage.MessageText, newMessage.UserId, newMessage.ChatId.ToString(), newMessage.SendDate.ToString("M/dd/yyyy hh:mm:ss tt")) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("message_sent", result.Value);

            // Check if the count of messages increased by 1
            var finalMessageCount = db.Messages.Count();
            Assert.AreEqual(initialMessageCount + 1, finalMessageCount);
        }

        [TestMethod()]
        public void GetMessagesTest()
        {
            // Get all chat IDs from the database
            var allChatIds = db.Chats.Select(c => c.ChatId).ToList();

            foreach (var chatId in allChatIds)
            {
                var result = controller.GetMessages(chatId) as OkObjectResult;

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result.Value, typeof(IEnumerable<Message>));

                var messages = result.Value as IEnumerable<Message>;

                Assert.IsTrue(messages.All(m => m.ChatId == chatId));
            }
        }

        [TestMethod()]
        public void GetCurrentDateTest()
        {
            var result = controller.GetCurrentDate() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(string));

            var currentDate = result.Value as string;

            Assert.IsFalse(string.IsNullOrEmpty(currentDate));
        }

    }
}