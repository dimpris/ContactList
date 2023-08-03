using ContactList.DataContexts;
using ContactList.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactList.DataServices
{
    public class SearchService
    {
        private readonly ContactListAppContext _context;
        private int UserId { get; set; }

        public SearchService(ContactListAppContext context, int userId)
        {
            _context = context;
            UserId = userId;
        }

        public List<Contact> SearchContacts(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            var contacts = _context.UserContacts(UserId)
                .Include(c => c.Phones)
                .Where(c => c.Name.ToLower().Contains(searchTerm) || c.Email.ToLower().Contains(searchTerm))
                .ToList();

            return contacts;
        }

        public List<Phone> SearchPhones(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            var phones = _context.Phones
                .Include(p => p.Contact)
                .Where(p => p.Contact.OwnerId == UserId && p.Number.ToLower().Contains(searchTerm))
                .ToList();

            return phones;
        }
    }
}
