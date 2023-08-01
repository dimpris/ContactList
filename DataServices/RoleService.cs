using ContactList.DataContexts;

namespace ContactList.DataServices
{
    public class RoleService
    {
        private readonly ContactListAppContext _data;

        public RoleService(ContactListAppContext context)
        {
            _data = context;
        }

        public int GetRoleIdByName(string roleName)
        {
            var role = _data.Roles.FirstOrDefault(r => r.Name == roleName);

            if (role == null)
            {
                role = _data.Roles.FirstOrDefault(r => r.Name == "user");
            }

            return role.Id;
        }
    }
}
