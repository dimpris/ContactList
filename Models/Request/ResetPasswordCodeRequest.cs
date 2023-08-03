using System.ComponentModel.DataAnnotations;

namespace ContactList.Models.Request
{
    public class ResetPasswordCodeRequest
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
