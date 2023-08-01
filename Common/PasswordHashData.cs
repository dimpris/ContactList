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
        //public string Hash { 
        //    get
        //    {
        //        return Hashing.HashPassword(this);
        //    }
        //}
        public string Salt { get; set; }
    }
}
