using System.ComponentModel.DataAnnotations;

namespace ContactList.Models
{
    public class PasswordReset
    {
        public int Id { get; set; }
        public string ResetCode { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiresAt { get; set; } = DateTime.Now.AddDays(3);
        public int OwnerId { get; set; }
        public User? Owner { get; set; }
    }
}
