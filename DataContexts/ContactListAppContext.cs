using ContactList.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactList.DataContexts
{
    public class ContactListAppContext : DbContext
    {
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<User> User { get; set; } = null!;
        public DbSet<Phone> Phone { get; set; } = null!;
        public DbSet<Contact> Contact { get; set; } = null!;
        public ContactListAppContext(DbContextOptions<ContactListAppContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
