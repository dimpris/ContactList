using ContactList.Common;
using ContactList.DataContexts;
using ContactList.Models;

namespace ContactList.DataServices
{
    public class UserService
    {
        private readonly ContactListAppContext _data;

        public UserService(ContactListAppContext context)
        {
            _data = context;
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
