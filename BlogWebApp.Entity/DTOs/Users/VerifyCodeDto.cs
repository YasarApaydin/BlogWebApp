using BlogWebApp.Entity.Entities;
using System.ComponentModel.DataAnnotations;

namespace BlogWebApp.Entity.DTOs.Users
{
    public class VerifyCodeDto
    {

        
        
      
        public string Email { get; set; }
     
        public DateTime Expiration { get; set; }
        public string Code { get; set; }
    }
}
