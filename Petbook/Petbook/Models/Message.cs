using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Petbook.Models
{
    public class Message
    {
        [Key]

        public int MessageId {  get; set; }
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public string? MessageText { get; set; }
        public DateTime SendDate { get; set; }
        public int? ChatId { get; set; }
        [JsonIgnore]
        public virtual Chat? Chat { get; set; }
     

    }
}
