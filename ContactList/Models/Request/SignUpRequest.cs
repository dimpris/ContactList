using System.ComponentModel.DataAnnotations;

namespace ContactList.Models.Request
{
    public class SignUpRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string PasswordRepeat { get; set; }
        public string Fullname { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
