using ContactList.DataContexts;
using ContactList.DataServices;
using ContactList.Models.Request;
using Microsoft.EntityFrameworkCore;
using Tests;

namespace TestProject1.Authentification
{
    public class ContactListTests : IDisposable
    {
        protected readonly ContactListAppContext Context;
        public ContactListTests()
        {
            Context  = ContactListContextFactory.Create();
        }
        public void Dispose()
        {
            ContactListContextFactory.Destroy(Context);
        }

        [Fact]
        public async Task SignUpAndSingInUserCommandHandler_Success()
        {
            // Arrange
            var handler = new UserService(Context);
            var userName = "Test";
            var password = "Test"; ;
            var email = "test@example.com";

            // Act
            var user = handler.CreateAndAdd(userName, password, userName, email);

            // Assert
            Assert.NotNull(handler.GetUser(user.Login));
        }

        [Fact]
        public async Task CreateContactCommandHandler_Success()
        {
            // Arrange
            var handler = new ContactsService(Context, ContactListContextFactory.UserId);
            var requestData = new ContactRequest()
            {
                Name = "Contact For Create",
                Email = "test@example.com",
                phone_number = new string[] { "", "(123) 456-7890", "(321) 456-7890" },
                phone_type = new string[] { "", "1", "2" }
            };

            // Act
            var contact = handler.Create(requestData);

            // Assert
            Assert.NotNull(
                await Context.Contacts.SingleOrDefaultAsync(c =>
                    c.Id == contact.Id && c.Email == requestData.Email &&
                    c.Name == requestData.Name
                ));
        }

        //[Fact]
        //public async Task EditContactCommandHandler_Success()
        //{
        //    // Arrange
        //    var handler = new ContactsService(Context, ContactListContextFactory.UserId);
        //    var requestData = new ContactRequest()
        //    {
        //        Name = "Contact For Create",
        //        Email = "test@example.com",
        //        phone_number = new string[] { "", "(123) 456-7890", "(321) 456-7890" },
        //        phone_type = new string[] { "", "1", "2" }
        //    };

        //    // Act
        //    var contact = handler.Create(requestData);

        //    // Assert
        //    Assert.NotNull(
        //        await Context.Contacts.SingleOrDefaultAsync(c =>
        //            c.Id == contact.Id && c.Email == requestData.Email &&
        //            c.Name == requestData.Name
        //        ));
        //}
    }
}
