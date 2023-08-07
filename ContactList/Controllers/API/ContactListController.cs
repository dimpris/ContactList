using ContactList.Common;
using ContactList.Common.Contracts;
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

                return await cs.ApiList();
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

                var contact = await cs.ApiDetails(id);
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

        [Authorize]
        [HttpPost]
        [Route("DeletePhone/{id}")]
        public async Task<object> DeletePhone(int? id)
        {
            try
            {
                var cc = new ContactsService(_context, GetCurrentUserId());
                await cc.DeletePhone(id);

                return true;

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("EditPhone/{id}")]
        public async Task<object> EditPhone(int? id, EditPhoneRequest req)
        {
            try
            {
                var cc = new ContactsService(_context, GetCurrentUserId());
                await cc.EditPhone(id, req);

                return true;

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
