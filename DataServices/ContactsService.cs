using ContactList.Common.Contracts;
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

        public async Task<IEnumerable<object>> ApiList()
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

        public async Task<object> ApiDetails(int? id)
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

        public Contact Create(ContactRequest req)
        {
            Contact contact = new Contact();
            contact.Name = req.Name;
            contact.Email = req.Email;
            contact.OwnerId = UserId;
            contact.Phones = PreparePhoneNumbers(req.phone_number, req.phone_type);

            _context.Add(contact);
            _context.SaveChanges();

            return contact;
        }

        public async Task<Contact> Edit(int? id, Contact contact, ContactRequest req)
        {
            contact.Name = req.Name;
            contact.Email = req.Email;

            if (req.phone_number.Length > 1)
            {
                contact.Phones.AddRange(PreparePhoneNumbers(req.phone_number, req.phone_type));
            }

            await _context.SaveChangesAsync();

            return contact;
        }

        public async Task<bool> Remove(int? id)
        {
            var contact = await Details(id);
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return true;
        }

        public Phone GetPhone(int? id)
        {
            var phone = _context.Phones
                .Include(p => p.Contact)
                .FirstOrDefault(p => p.Id == id);

            if (phone.Contact.OwnerId != UserId)
            {
                throw new UnauthorizedAccessException("Forbidden");
            }

            return phone;
        }

        public async Task<bool> DeletePhone(int? id)
        {
            var phone = GetPhone(id);
            _context.Phones.Remove(phone);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Phone> EditPhone(int? id, EditPhoneRequest req)
        {
            var phone = GetPhone(id);
            phone.Number = req.Number;
            phone.Type = (PhoneType)Enum.Parse(typeof(PhoneType), req.Type.ToString());


            await _context.SaveChangesAsync();

            return phone;
        }
    }
}
