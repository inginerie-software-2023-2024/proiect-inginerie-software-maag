using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Petbook.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime? JoinDate { get; set; }

        public string? ProfilePhoto { get; set; }

        public string? PhoneNumber { get; set; }


        [JsonIgnore]
        public virtual IList<Pet>? Pets { get; set; }

        [JsonIgnore]
        public virtual ICollection<Comment>? Comments { get; set; }
        [JsonIgnore]
        public virtual ICollection<ApplicationUser>? Followers { get; set; }
        [JsonIgnore]
        public virtual ICollection<ApplicationUser>? Following { get; set; }
        [JsonIgnore]
        public virtual ICollection<BlogPost>? BlogPosts { get; set; }
        [JsonIgnore]
        public virtual ICollection<BlogPostLike>? BlogPostLikes { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserInChat>? UserInChats { get; set; }

        [JsonIgnore]
        [NotMapped]
        public IEnumerable<SelectListItem>? AllRoles { get; set; }



    }
}