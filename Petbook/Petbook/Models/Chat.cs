using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Petbook.Models
{
    public class Chat
    {

        [Key]
        public int ChatId { get; set; }
        public virtual ICollection<UserInChat>? UserInChats { get; set; }
        public virtual ICollection<Message>? Messages { get; set; }
        public DateTime? LastMessageTime { get; set; }
    }
}
