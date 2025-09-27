namespace BlogWebApp.Entity.DTOs.Users
{
    public class UserEmailVerificationCodeDto
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }  
        public string CodeHash { get; set; } = null!;
        public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;
        public bool IsUsed { get; set; } = false;
    }
}
