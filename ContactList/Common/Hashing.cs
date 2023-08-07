using ContactList.Common.Contracts;
using ContactList.Models;

namespace ContactList.Common
{
    public class Hashing
    {
        public static bool VerifyUserAndPasswordHash(User user, string password) 
        {
            string passHash = HashPassword(password, user.PasswordHashSalt);

            bool passwordValid = user.PasswordHash == passHash;

            if (!passwordValid)
            {
               throw new InvalidCredentialsException();
            }

            return passwordValid;
        }
        public static string HashPassword(PasswordHashData passwordData)
        { 
            return HashPassword(passwordData.Password, passwordData.Salt);
        }
        public static string HashPassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public static string GenerateSalt(int saltLengthInBytes = 16)
        {
            return BCrypt.Net.BCrypt.GenerateSalt(saltLengthInBytes);
        }
    }
}
