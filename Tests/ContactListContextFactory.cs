using ContactList.DataContexts;
using ContactList.DataServices;
using ContactList.Models;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class ContactListContextFactory
    {
        public static int UserId = 1;

        public static int AdminRoleId = 1;
        public static int UserRoleId = 2;

        public static int ContactForEditId = 2;
        public static int ContactForDeleteId = 3;
        public static ContactListAppContext Create()
        {
            var options = new DbContextOptionsBuilder<ContactListAppContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ContactListAppContext(options);
            context.Database.EnsureCreated();

            Contact contactForEdit = new Contact();
            contactForEdit.Id = ContactForEditId;
            contactForEdit.Name = "Contact For Edit";
            contactForEdit.Email = "test@example.com";
            contactForEdit.OwnerId = UserId;
            contactForEdit.Phones = new List<Phone>()
            {
                new Phone()
                {
                    Type = PhoneType.Mobile,
                    Number = "(123) 456-7890"
                },
                new Phone()
                {
                    Type = PhoneType.Work,
                    Number = "(123) 456-7890"
                }
            };
            Contact contactForDelete = new Contact();
            contactForDelete.Id = ContactForDeleteId;
            contactForDelete.Name = "Contact For Delete";
            contactForDelete.Email = "test@example.com";
            contactForDelete.OwnerId = UserId;
            contactForDelete.Phones = new List<Phone>()
            {
                new Phone()
                {
                    Type = PhoneType.Mobile,
                    Number = "(123) 456-7890"
                },
                new Phone()
                {
                    Type = PhoneType.Work,
                    Number = "(123) 456-7890"
                }
            };
            context.Contacts.Add(contactForEdit);
            context.Contacts.Add(contactForDelete);

            context.SaveChanges();
            return context;
        }

        public static void Destroy(ContactListAppContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
