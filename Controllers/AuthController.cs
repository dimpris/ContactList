using ContactList.Common;
using ContactList.DataContexts;
using ContactList.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace ContactList.Controllers
{
    public class AuthController : Controller
    {
        private readonly ContactListAppContext _data;

        public AuthController(ContactListAppContext context)
        {
            _data = context;
        }

        public IActionResult Index()
        {
            var roles = _data.Roles.ToList();
            return View(roles);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginRequest req)
        {
            var user = _data.Users.FirstOrDefault(u => u.Login == req.Login);

            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        public IActionResult SignOut()
        {
            // TODO implement Sign Out
            return RedirectToAction("Login");
        }
    }
}
