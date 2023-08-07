using System.ComponentModel.DataAnnotations;

namespace ContactList.Models.Request
{
    public class ConfirmResetPasswordRequest
    {
        public string Code { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string PasswordRepeat { get; set; }
    }
}
