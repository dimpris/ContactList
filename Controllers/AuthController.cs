using ContactList.Common;
using ContactList.Common.Contracts;
using ContactList.DataContexts;
using ContactList.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace ContactList.Controllers
{
    public class AuthController : BaseContactListController
    {
        private readonly ContactListAppContext _data;
        private readonly AuthenticationManager authManager;

        public AuthController(ContactListAppContext context, IConfiguration appConfig)
        {
            _data = context;
            authManager = new AuthenticationManager(this, _data, appConfig);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            try
            {
                var token = authManager.SignIn(req.Login, req.Password);

                return Redirect("/Contacts");

            }
            catch (InvalidCredentialsException ex)
            {
                return Problem("Wrong credentials");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(SignUpRequest req)
        {
            try
            {

                var u = authManager.SignUp(req);
                var token = authManager.SignIn(u, req.Password);

                return Redirect("/Contacts");

            }
            catch (InvalidCredentialsException ex)
            {
                return Problem("Passwords doesn't match");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        public IActionResult SignOut()
        {
            authManager.SignOut();
            return RedirectToAction("Login");
        }
    }
}
