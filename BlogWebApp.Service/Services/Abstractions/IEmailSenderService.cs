using BlogWebApp.Entity.DTOs.Users;
using BlogWebApp.Entity.Entities;

namespace BlogWebApp.Service.Services.Abstractions
{
    public  interface IEmailSenderService
    {


        Task SendAsync(string toEmail, string subject, string bodyPlain);

        

        Task<string> GenerateAndSendAsync(string email,string ad);
        Task<bool> VerifyCodeAsync(UserRegisterDto userRegisterDto,  string code, string protectedToken);
    }
}
