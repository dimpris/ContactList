namespace ContactList.Models.Request
{
    public class SignUpRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
    }
}
