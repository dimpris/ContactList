using ContactList.Common;
using ContactList.DataContexts;
using ContactList.Models;
using ContactList.Models.Request;
using Microsoft.EntityFrameworkCore;

namespace ContactList.DataServices
{
    public class ContactsService
    {
        private readonly ContactListAppContext _context;
        private int UserId { get; set; }
        public ContactsService(ContactListAppContext context, int userId)
        {
            _context = context;
            UserId = userId;
        }

        public async Task<IEnumerable<object>> ListSelection()
        {
            var contacts = await ListQuery()
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    Phones = c.Phones.Select(p => p.Number)
                })
                .ToListAsync();

            return contacts;
        }
        public async Task<List<Contact>> List()
        { 
            return await ListQuery().ToListAsync();
        }
        private IQueryable<Contact> ListQuery()
        {
            if (NoContacts())
            {
                throw new NotFoundException();
            }

            var contacts = _context.UserContacts(UserId)
                    .Include(c => c.Phones);

            return contacts;
        }

        public async Task<object> DetailsSelection(int? id)
        {
            var contact = await ListQuery()
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Phones = c.Phones.Select(p => new
                    {
                        Id = p.Id,
                        Type = p.Type.ToString(),
                        Number = p.Number
                    })
                }).FirstOrDefaultAsync(m => m.Id == id);

            return contact;
        }
        public async Task<Contact> Details(int? id)
        {
            if (id == null || NoContacts())
            {
                throw new NotFoundException();
            }

            var contact = await _context.UserContacts(UserId)
                .Include(c => c.Phones)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contact == null)
            {
                throw new NotFoundException();
            }

            return contact;
        }

        public bool NoContacts()
        {
            return !HasContacts();
        }
        public bool HasContacts()
        {
            return _context.Contacts != null;
        }

        public List<Phone> PreparePhoneNumbers(string[]? phone_number, string[]? phone_type)
        {
            var phones = new List<Phone>();

            if (phone_number != null && phone_type != null)
            {
                for (int i = 1; i < phone_number.Length; i++)
                {
                    var phone = new Phone()
                    {
                        Number = phone_number[i],
                        Type = (PhoneType)Enum.Parse(typeof(PhoneType), phone_type[i])
                    };

                    phones.Add(phone);
                }
            }

            return phones;
        }

        public async Task<Contact> CreateAndSave(CreateContactRequest req)
        {
            Contact contact = Create(req);

            _context.Add(contact);
            await _context.SaveChangesAsync();

            return contact;
        }
        public Contact Create(CreateContactRequest req)
        {
            Contact contact = new Contact();
            contact.Name = req.Name;
            contact.Email = req.Email;
            contact.OwnerId = UserId;
            contact.Phones = PreparePhoneNumbers(req.phone_number, req.phone_type);

            return contact;
        }

        public async Task<bool> Remove(int? id)
        {
            var contact = await Details(id);
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
