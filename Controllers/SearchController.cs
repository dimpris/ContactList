using ContactList.DataContexts;
using ContactList.DataServices;
using ContactList.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace ContactList.Controllers
{
    public class SearchController : BaseContactListController
    {
        private readonly ContactListAppContext _context;

        public SearchController(ContactListAppContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index(string searchTerm)
        {
            var ss = new SearchService(_context, GetCurrentUserId());
            var result = new SearchResult()
            {
                Contacts = ss.SearchContacts(searchTerm),
                Phones = ss.SearchPhones(searchTerm)
            };

            return View(result);
        }
    }
}
