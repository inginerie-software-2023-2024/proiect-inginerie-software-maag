using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Petbook.Models
{
    public class Chat
    {

        [Key]
        public int ChatId { get; set; }
        [JsonIgnore]
        public virtual IList<UserInChat>? UserInChats { get; set; }
 
        public virtual IList<Message>? Messages { get; set; }
        public string? LastMessage { get; set; }
        public DateTime? LastMessageTime { get; set; }
    }
}
