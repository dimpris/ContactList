namespace ContactList.Common
{
    public class PasswordHashData
    {
        public PasswordHashData(string password)
        {
            Password = password;
            Salt = Hashing.GenerateSalt();
        }

        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
