using BlogWebApp.Entity.Entities;
using Microsoft.AspNetCore.Http;

namespace BlogWebApp.Entity.DTOs.Users
{
    public class UserProfileEditUpdateDto
    {




        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }


        public string Role { get; set; }


        public IFormFile? Photo { get; set; }

        public Resim Resim { get; set; }

        public bool IsDeleted { get; set; }
    }
}
