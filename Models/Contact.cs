namespace ContactList.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int OwnerId { get; set; }
        public User? Owner { get; set; }
        public virtual List<Phone>? Phones { get; set;}
    }
}
