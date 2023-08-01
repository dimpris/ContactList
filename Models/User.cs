using System.ComponentModel.DataAnnotations.Schema;

namespace ContactList.Models
{
    [Table("ContactListUser")]
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordHashSalt { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public virtual Role? Role { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [NotMapped]
        public bool IsActive
        {
            get
            {
                return VerifiedAt != null;
            }
        }
    }
}
