using ContactList.Common;
using ContactList.Common.Contracts;
using ContactList.DataContexts;
using ContactList.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace ContactList.DataServices
{
    public class UserService
    {
        private readonly ContactListAppContext _data;

        public UserService(ContactListAppContext context)
        {
            _data = context;
        }

        public User GetUser(string login)
        {
            var user = _data.Users
            .Include(u => u.Role)
                .FirstOrDefault(u => u.Login == login && u.VerifiedAt != null);

            if (user == null)
            {
                throw new InvalidCredentialsException();
            }

            return user;
        }

        public User CreateAndAdd(string login, string password, string fullname, string email, string role = "user")
        {
            var u = Create(login, password, fullname, email);
            
            var roleService = new RoleService(_data);
            int roleId = roleService.GetRoleIdByName(role);

            u.VerifiedAt = DateTime.Now;
            u.RoleId = roleId;

            Add(u);

            return u;
        }
        public static User Create(string login, string password, string fullname, string email)
        {
            PasswordHashData pass = new PasswordHashData(password);
            string hash = Hashing.HashPassword(pass);

            var u = new User
            {
                Fullname = fullname,
                Login = login,
                PasswordHashSalt = pass.Salt,
                PasswordHash = hash,
                Email = email
            };

            return u;
        }
        public User Add(User user)
        {
            _data.Users.Add(user);
            _data.SaveChanges();

            return user;
        }
        public User SetRole(User user, string role)
        {
            var rs = new RoleService(_data);
            int roleId = rs.GetRoleIdByName(role);

            user.RoleId = roleId;

            _data.Users.Add(user);
            _data.SaveChanges();

            return user;
        }
    }
}
