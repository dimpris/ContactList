using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ContactList.Controllers
{
    public class BaseContactListController : Controller
    {
        protected string GetCurrentUserName()
        {
            return HttpContext.User.Identity?.Name ?? "";
        }

        protected int GetCurrentUserId()
        {
            string? nameIdentifier = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (nameIdentifier != null)
            {
                int userId;
                int.TryParse(nameIdentifier, out userId);

                return userId;
            }

            return -1;
        }
    }
}
