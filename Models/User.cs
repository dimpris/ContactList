namespace ContactList.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Fullname { get; set; }
        public UserRole Role { get; set; }
    }
}
