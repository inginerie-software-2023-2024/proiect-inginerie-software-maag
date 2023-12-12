using System.ComponentModel.DataAnnotations.Schema;

namespace Petbook.Models
{
    public class UserInChat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? UserId { get; set; }
        public int? ChatId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public virtual Chat? Chat { get; set; }
    }
}
