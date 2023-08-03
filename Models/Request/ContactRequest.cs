namespace ContactList.Models.Request
{
    public class ContactRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string[]? phone_type { get; set; }
        public string[]? phone_number { get; set; }
    }
}
