using BlogWebApp.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogWebApp.Entity.Entities
{
    public class AppUser:IdentityUser<Guid>,IEntityBase
    {

      
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Guid ResimId { get; set; } = Guid.Parse("84C3B69D-A397-4E09-874F-BD4407FCE6A0");

     
        public Resim Resim { get; set; }
       
        public ICollection<Yorum> Yorumlar { get; set; }
        public ICollection<Makale> Makales { get; set; }
    }
}
