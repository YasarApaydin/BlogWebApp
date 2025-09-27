using BlogWebApp.Entity.Entities;
using Microsoft.AspNetCore.Http;

namespace BlogWebApp.Entity.DTOs.Users
{
    public class UserProfileEditDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
       

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
          public IFormFile? Photo { get; set; }
          public Resim? Resim { get; set; }
        public string CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
     

      

        public bool IsDeleted { get; set; }

    }
}
