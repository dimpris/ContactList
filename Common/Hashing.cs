using ContactList.Models;
using System.Collections;
using System.Security.Cryptography;

namespace ContactList.Common
{
    public class Hashing
    {
        public static bool VerifyUserAndPasswordHash(User user, string password) 
        {
            // TODO Move this checking from here
            if (user == null)
            {
                throw new Exception("User not found");
            }
            
            if (user.IsActive == false)
            {
                throw new Exception("User not active");
            }

            string passHash = HashPassword(password, user.PasswordHashSalt);

            //BCrypt.Net.BCrypt.Verify();

            return user.PasswordHash == passHash;
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

            //byte[] salt = new byte[saltLengthInBytes];
            //using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            //{
            //    rngCryptoServiceProvider.GetBytes(salt);
            //}
            //return BitConverter.ToString(salt).Replace("-", "").ToLower();
        }
    }
}
