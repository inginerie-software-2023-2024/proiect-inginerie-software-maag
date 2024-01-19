using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Petbook.Data;
using Petbook.Models;

namespace Petbook.Controllers
{
    public class ChatsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ChatsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Index(string search)
        {

            var user = db.Users.Where(u => u.Id == _userManager.GetUserId(User)).First();


            ViewBag.SearchString = search;

            Console.WriteLine(search);

            List<Chat> displayedChats;

            if (string.IsNullOrEmpty(search))
            {
                 displayedChats = db.Chats.Include(c => c.UserInChats).ThenInclude(uc => uc.User).Include("Messages").Where(c => (c.UserInChats.Where(uc => uc.UserId == user.Id).Count() > 0))
                    .Where(c => c.Messages.Count > 0).OrderByDescending(c => c.LastMessageTime).ToList();
            }
            else
            {
                var displayedUsers = db.Users.Include("Following").Where(u => u.Id == user.Id).First().Following;
                displayedUsers = displayedUsers.Where(u => u.UserName.ToLower().Contains(search.ToLower())).ToList();

                displayedChats = db.Chats.Include(c => c.UserInChats).ThenInclude(uc => uc.User).Include("Messages")
                    .Where(c => (c.UserInChats.Where(uc => uc.UserId == user.Id).Count() > 0)).
                    Where(c => (c.UserInChats.Where(uc => uc.User.UserName.ToLower().Contains(search.ToLower())).Count() > 0)).ToList();

                HashSet<string> existingUserIDs = new HashSet<string>();

                foreach (var existingChat in displayedChats)
                {
                    if (existingChat.UserInChats.First().UserId != user.Id)
                        existingUserIDs.Add(existingChat.UserInChats.First().UserId);
                    else
                        existingUserIDs.Add(existingChat.UserInChats.Last().UserId);
                }

                

                foreach (ApplicationUser searchedUser in displayedUsers)
                {
                    if (!existingUserIDs.Contains(searchedUser.Id))
                    {
                        Chat newChat = new Chat();
                        newChat.UserInChats = new List<UserInChat>() ;
                        newChat.UserInChats.Add(new UserInChat {ChatId = newChat.ChatId, UserId = searchedUser.Id, User = searchedUser});
                        newChat.UserInChats.Add(new UserInChat { ChatId = newChat.ChatId, UserId = user.Id , User = user});
                        displayedChats.Add(newChat);
                        db.Chats.Add(newChat);
                        db.SaveChanges();
                    }
                }
            }

            ViewBag.DisplayedChats = displayedChats;
            ViewBag.CurrentUser = user;

            return View();
        }

        [HttpPost("/Chats/AddMessage/")]
        public IActionResult AddMessage([FromForm]string messageText,
                                        [FromForm] string senderId,
                                        [FromForm] string chatId,
                                        [FromForm] string date)
        {
            Message newMessage = new Message{
                ChatId = Int32.Parse(chatId),
                MessageText = messageText,
                UserId = senderId,
                SendDate = DateTime.ParseExact(date, "M/dd/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture) 
            };
            db.Messages.Add(newMessage);
            db.SaveChanges();

            return Ok("message_sent");
        }

        [HttpGet("/Chats/GetMessages/{chatId}")]
        public IActionResult GetMessages([FromRoute] int chatId)
        {
           var messages = db.Chats.Include("Messages").Where(c => c.ChatId == chatId).First().Messages;

           return Ok(messages);

        }

        [HttpGet("/Chats/GetCurrentDate/")]
        public IActionResult GetCurrentDate()
        {
            return Ok(DateTime.Now.ToString());
        }

    }
}
