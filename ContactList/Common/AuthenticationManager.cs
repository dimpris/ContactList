using ContactList.Common.Contracts;
using ContactList.Controllers;
using ContactList.DataContexts;
using ContactList.DataServices;
using ContactList.Models;
using ContactList.Models.Request;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactList.Common
{
    public class AuthenticationManager
    {
        public IResponseCookies Cookies
        { 
            get
            {
                if (Controller.HttpContext.Response.Cookies != null)
                {
                    return Controller.HttpContext.Response.Cookies;
                }
                else
                {
                    throw new NullReferenceException("Response Cookies not available");
                }
            }
        }
        private readonly BaseContactListController Controller;
        private readonly ContactListAppContext DataContext;
        private readonly IConfiguration appConfig;
        public AuthenticationManager(BaseContactListController controller, ContactListAppContext dataContext, IConfiguration appConfiguration)
        {
            Controller = controller;
            DataContext = dataContext;
            appConfig = appConfiguration;
        }

        public void SignOut()
        {
            Cookies.Delete("accessToken");
            Cookies.Delete("identityInfo");
        }

        public string SignIn(User user, string password, bool storeToken = true)
        {
            

            Hashing.VerifyUserAndPasswordHash(user, password);
            var claims = CollectUserClaims(user);
            var signingCredentials = GetSigningCredentials();
            var token = GenerateJwtToken(claims, signingCredentials);

            if (storeToken)
            {
                StoreToken(token);
                StoreSession(user);
            }

            return token;
        }
        public string? SignIn(string login, string password, bool storeToken = true)
        {
            var userService = new UserService(DataContext);
            var user = userService.GetUser(login);

            return SignIn(user, password, storeToken);
        }

        public User SignUp(SignUpRequest req)
        {
            if (req.PasswordRepeat != req.Password)
            {
                throw new InvalidCredentialsException();
            }
            
            var userService = new UserService(DataContext);

            return userService.CreateAndAdd(req.Login, req.Password, req.Fullname, req.Email);
        }
        private List<Claim> CollectUserClaims(User user)
        {
            return new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Fullname),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Login),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role?.Name ?? "user")
                };
        }
        private SigningCredentials GetSigningCredentials()
        {
            string secret = appConfig["AuthOptions:SecretKey"] ?? "";
            byte[] secretBytes = Encoding.UTF8.GetBytes(secret);
            var key = new SymmetricSecurityKey(secretBytes);
            return new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );
        }

        private string GenerateJwtToken(List<Claim> claims, SigningCredentials credentials)
        {
            double expireTimeout;
            double.TryParse(appConfig["AuthOptions:ExpireTimeout"], out expireTimeout);

            var jwt = new JwtSecurityToken(
                    issuer: appConfig["AuthOptions:Issuer"],
                    audience: appConfig["AuthOptions:Audience"],
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(expireTimeout)),
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        
        private void StoreSession(User u)
        {
            string userInfo = u.Id.ToString() + ":" + u.Fullname;

            var userInfoBytes = Encoding.UTF8.GetBytes(userInfo);
            var userInfoBase64 = Convert.ToBase64String(userInfoBytes);

            Cookies.Append("identityInfo", userInfoBase64);
        }

        private void StoreToken(string token) 
        {
            Cookies.Delete("accessToken");
            Cookies.Append("accessToken", token);
        }

        public string GenerateResetPasswordCode(string email)
        {
            var user = DataContext.Users
                .FirstOrDefault(u => u.Email == email && u.VerifiedAt != null);

            if (user == null)
            {
                throw new InvalidCredentialsException();
            }

            var passwordReset = new PasswordReset()
            {
                OwnerId = user.Id,
                ResetCode = Guid.NewGuid().ToString()
            };

            DataContext.PasswordResets.Add(passwordReset);
            DataContext.SaveChanges();

            // Here should e-mail sending

            return passwordReset.ResetCode;
        }
        public PasswordReset GetResetPasswordCode(string code)
        {
            var passwordResetCode = DataContext.PasswordResets
                .FirstOrDefault(r => r.ResetCode == code && r.ExpiresAt > DateTime.Now);

            return passwordResetCode;
        }
        public void ResetPassword(ConfirmResetPasswordRequest req)
        {
            var passwordResetCode = DataContext.PasswordResets
                .FirstOrDefault(r => r.ResetCode == req.Code && r.ExpiresAt > DateTime.Now);

            var user = DataContext.Users.FirstOrDefault(u => u.Id == passwordResetCode.OwnerId && u.VerifiedAt != null);

            PasswordHashData pass = new PasswordHashData(req.Password);
            string hash = Hashing.HashPassword(pass);

            user.PasswordHash = hash;
            user.PasswordHashSalt = pass.Salt;

            DataContext.PasswordResets.Remove(passwordResetCode);
            DataContext.SaveChanges();
        }
    }
}
