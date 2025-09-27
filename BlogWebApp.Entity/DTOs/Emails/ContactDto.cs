using System.ComponentModel.DataAnnotations;

namespace BlogWebApp.Entity.DTOs.Emails
{
    public class ContactDto
    {
        [Required]
        public string Ad { get; set; } = null!;

        [Required]
        public string Soyad { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        public string? Telefon { get; set; }

       

        [Required]
        public string Mesaj { get; set; } = null!;
    }
}
