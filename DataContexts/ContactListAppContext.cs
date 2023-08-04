using ContactList.DataServices;
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
        public DbSet<PasswordReset> PasswordResets { get; set; } = null!;
        public ContactListAppContext(DbContextOptions<ContactListAppContext> options)
            : base(options)
        {
             Database.EnsureCreated();
        }

        public IQueryable<Contact> UserContacts(int userId)
        {
            return Contacts.Where(c => c.OwnerId == userId);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            Role adminRole = new Role { Id = 1, Name = "administrator" };
            Role userRole = new Role { Id = 2, Name = "user" };
            modelBuilder.Entity<Role>().HasData(adminRole, userRole);

            var u = UserService.Create("john_doe", "john_doe", "John Doe", "john.doe@example.com");
            u.Id = 1;
            u.VerifiedAt = DateTime.Now;
            u.RoleId = userRole.Id;

            modelBuilder.Entity<User>().HasData(u);
        }
    }
}
