using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Petbook.Data;
using Petbook.Models;

namespace Petbook.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PostsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // display all posts with their comments and likes,
        // for each article display the pet who posted it
        // for each comment display the user who posted it
        [Authorize(Roles = "User,Admin")]
        public IActionResult Index()
        {

            var user = db.Users.Include("Following").Where(u => u.Id == _userManager.GetUserId(User)).First();
            var posts = db.Posts.Include("Pet")
                                .Include("Pet.User")
                                .Include("PostLikes")
                                .Include("Comments")
                                .Include("Comments.User")
                                .Where(p => p.Pet.UserId != _userManager.GetUserId(User))
                                .Where(p => user.Following.Contains(p.Pet.User))
                                .OrderByDescending(p => p.PostDate)
                                .ToList();


            ViewBag.Posts = posts;
            ViewBag.LoggedUser = _userManager.GetUserId(User);
            ViewBag.IsAdmin = User.IsInRole("Admin");
            ViewBag.CurrentPage = "Feed";
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View();
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Explore()
        {
            var posts = db.Posts.Include("Pet")
                                .Include("Pet.User")
                                .Include("PostLikes")
                                .Include("Comments")
                                .Include("Comments.User")
                                .OrderByDescending(p => p.PostDate)
                                .Where(p => p.Pet.UserId != _userManager.GetUserId(User))
                                .ToList();

            ViewBag.ExplorePosts = posts;
            ViewBag.LoggedUser = _userManager.GetUserId(User);
            ViewBag.CurrentPage = "Explore";
            ViewBag.IsAdmin = User.IsInRole("Admin");

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View();
        }

 
        // displays a post with the given id
        // with its comments, likes, the pet who posted it
        // and the user who posted each comment
        [Authorize(Roles = "User,Admin")]
        public IActionResult Show(int id)
        {
            Post post = db.Posts.Include("Pet")
                                .Include("Pet.User")
                                .Include("PostLikes")
                                .Include("Comments")
                                .Include("Comments.User")
                                .Where(p => p.PostId == id)
                                .First();
          
            ViewBag.UserCurent = post.Pet.UserId;
            ViewBag.LoggedUser = _userManager.GetUserId(User); 

            return View(post);
        }

        // add a comment to one post from the db
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Show([FromForm] Comment comment)
        {
            comment.CommentDate = DateTime.Now;
            comment.UserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect("/Posts/Show/" + comment.PostId);
            }
            else
            {
                Post p = db.Posts.Include("Pet")
                                .Include("Pet.User")
                                .Include("PostLikes")
                                .Include("Comments")
                                .Include("Comments.User")
                                .Where(p => p.PostId == comment.PostId)
                                .First();
     
                return View(p);
            }
        }

        public IActionResult GetPopupContent(int id)
        {
            Post post = db.Posts.Include("Pet")
                                .Include("Pet.User")
                                .Include("PostLikes")
                                .Include("Comments")
                                .Include("Comments.User")
                                .Where(p => p.PostId == id)
                                .First();
            ViewBag.LoggedUser = _userManager.GetUserId(User);
            ViewBag.IsAdmin = User.IsInRole("Admin");

            return PartialView("PostInfo", post);
        }
       
        // form for adding a new post
        [Authorize(Roles = "User,Admin")]
        public IActionResult New()
        {
            Post post = new Post();

            post.Pets = GetPetsOfCurrentUser();

            return View(post);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult New(Post post)
        {
            post.PostDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                TempData["message"] = "The post was added";
                return RedirectToAction("Index");
            }
            else
            {
                var errors = ModelState.Values.Where(E => E.Errors.Count > 0)
                         .SelectMany(E => E.Errors)
                         .Select(E => E.ErrorMessage)
                         .ToList();
                foreach (var i in errors)
                {
                    Console.WriteLine(i);
                }

                post.Pets = GetPetsOfCurrentUser();
                return View(post);
            }
        }

        // edit the post from the db
        // it will be displayed in a form with the current values
        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id)
        {

            Post post = db.Posts.Include("Pet")
                                .Where(p => p.PostId == id)
                                .First();

            post.Pets = GetPetsOfCurrentUser();

            // edit only the posts that are posted on the pets profile of the current user
            if (post.Pet.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(post);
            }
            else
            {
                return Unauthorized("NOT_YOUR_OWN_POST");
            }

        }

        // add the modified post in the db
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id, Post requestPost)
        {

            Post post = db.Posts.Include("Pet")
                                .Where(p => p.PostId == id)
                                .First();

            if (ModelState.IsValid)
            {
                if (post.Pet.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {   
                    post.PostPhoto = requestPost.PostPhoto;
                    post.Description = requestPost.Description;
                    post.PostDate = requestPost.PostDate;
                    TempData["message"] = "The post has been modified";
                    db.SaveChanges();
                    return Redirect("/Pets/Show/" + post.Pet.PetId);
                }
                else
                {
                    post.Pets = GetPetsOfCurrentUser();
                    TempData["message"] = "Cannot edit the posts that aren't yours";
                    return Redirect("/Pets/Show/" + post.Pet.PetId);
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                return View(requestPost);
            }
        }

        // delete a post that is posted from the pets profiles of the current user
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Delete(int id)
        {
            Post post = db.Posts.Include("Pet")
                                .Where(p => p.PostId == id)
                                .First();
           
            if (post.Pet.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Posts.Remove(post);
                db.SaveChanges();
                TempData["message"] = "The post has been deleted";

                return Redirect("/Pets/Show/" + post.Pet.PetId);
            }
            else
            {
                TempData["message"] = "Cannot delete the posts that aren't yours";
                return Redirect("/Pets/Show/" + post.Pet.PetId);
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetPetsOfCurrentUser()
        {
            var selectList = new List<SelectListItem>();
            var pets = db.Pets.Where(p => p.UserId == _userManager.GetUserId(User)).ToList();

            foreach (var p in pets)
            {
                // we add the pets needed for the dropdown
                selectList.Add(new SelectListItem
                {
                    Value = p.PetId.ToString(),
                    Text = p.PetName
                });
            }

            return selectList;
        }
    }
}
