using System.Security.Cryptography;
using System.Text;

namespace BlogWebApp.Service.Helpers.SendEmail
{
    public class HashHelper : IHashHelper
    {
        public string Sha256(string raw)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return Convert.ToHexString(bytes);  
        }
    }
}
