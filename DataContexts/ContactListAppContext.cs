using ContactList.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactList.DataContexts
{
    public class ContactListAppContext : DbContext
    {
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Phone> Phones { get; set; } = null!;
        public DbSet<Contact> Contacts { get; set; } = null!;
        public ContactListAppContext(DbContextOptions<ContactListAppContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Role adminRole = new Role { Id = 1, Name = "administrator" };
            Role userRole = new Role { Id = 2, Name = "user" };
            modelBuilder.Entity<Role>().HasData(adminRole, userRole);
            modelBuilder.Entity<User>().HasData(
                    new User 
                    { 
                        Id = 1, 
                        Fullname = "John Doe", 
                        Login = "john_doe", 
                        PasswordHash = "john_doe", 
                        Email = "john.doe@example.com",
                        RoleId = 2,
                        VerifiedAt = DateTime.UtcNow,
                    }
            );
        }
    }
}
