using ContactList.Common;
using ContactList.DataContexts;
using ContactList.DataServices;
using ContactList.Models;
using ContactList.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace ContactList.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactListController : BaseContactListController
    {
        private readonly ContactListAppContext _context;
        private readonly IConfiguration appConfig;
        private readonly AuthenticationManager authManager;
        public ContactListController(ContactListAppContext context, IConfiguration appConfig)
        {
            _context = context;
            this.appConfig = appConfig;
            authManager = new AuthenticationManager(this, _context, appConfig);
        }

        [HttpGet]
        [Authorize]
        public async Task<object> Index()
        {
            try
            {
                var cs = new ContactsService(_context, GetCurrentUserId());

                return await cs.ListSelection();
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // GET api/<APIContactsController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<object> Get(int? id)
        {
            try
            {
                var cs = new ContactsService(_context, GetCurrentUserId());

                var contact = await cs.DetailsSelection(id);
                return contact;
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        public object Login(LoginRequest req)
        {
            try
            {
                var token = authManager.SignIn(req.Login, req.Password, false);

                return new { access_token = token };

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
    }
}
