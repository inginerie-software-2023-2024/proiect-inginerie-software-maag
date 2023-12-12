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
        public IActionResult Index()
        {

            var user = db.Users.Where(u => u.Id == _userManager.GetUserId(User)).First();

            var existingChats = db.Chats.Include(c=>c.UserInChats).ThenInclude(uc=>uc.User).Include("Messages").Where(c =>( c.UserInChats.Where(uc => uc.UserId == user.Id).Count() >0))
                .Where(c=>c.Messages.Count>0).OrderByDescending(c=>c.LastMessageTime).ToList();

            ViewBag.ExistingChats = existingChats;
            ViewBag.CurrentUser = user;

            return View();
        }

    }
}
