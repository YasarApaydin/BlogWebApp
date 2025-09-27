using BlogWebApp.Data.UnitOfWorks;
using BlogWebApp.Entity.DTOs.Users;
using BlogWebApp.Entity.Entities;
using BlogWebApp.Service.Helpers.SendEmail;
using BlogWebApp.Service.Helpers.Token;
using BlogWebApp.Service.Services.Abstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace BlogWebApp.Service.Services.Concreates
{
    public class EmailSenderService : IEmailSenderService
    {

        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitOfWork;
        private readonly IHashHelper hashHelper;
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenHelper tokenHelper;
        private readonly IUserService userService;
        
        public EmailSenderService(IUserService _userService, ITokenHelper _tokenHelper,IConfiguration _configuration, IUnitOfWork _unitOfWork, IHashHelper _hashHelper, UserManager<AppUser> _userManager)
        {
            configuration = _configuration;
            unitOfWork = _unitOfWork;
            hashHelper = _hashHelper;
            userManager = _userManager;
            tokenHelper = _tokenHelper;
            userService = _userService;
          
        }



        public async Task<string> GenerateAndSendAsync(string email,string ad)
        {

            var code = new Random().Next(100000, 999999).ToString();
            var hash = hashHelper.Sha256(code);
            
            var expiration = DateTime.UtcNow.AddMinutes(5);


            var model = new VerifyCodeDto
            {

                Code = hash,
                Email = email,
                Expiration = expiration

            };


            var encryptedToken = tokenHelper.ProtectVerificationToken(model);

           
            string subject = "✔️ Email Doğrulama Kodunuz";
            string body = $@"
    <html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f4f4f4;
                color: #333;
                padding: 20px;
            }}
            .container {{
                background-color: #fff;
                border-radius: 8px;
                padding: 20px;
                max-width: 500px;
                margin: auto;
                box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
            }}
            .code {{
                font-size: 24px;
                font-weight: bold;
                color: #2d89ef;
                margin: 20px 0;
            }}
            .footer {{
                font-size: 12px;
                color: #888;
                margin-top: 30px;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <h2>Merhaba {ad},</h2>
            <p>Hesabınızı doğrulamak için aşağıdaki 6 haneli kodu kullanın:</p>
            <div class='code'>{code}</div>
            <p>Bu kod <strong>5 dakika</strong> boyunca geçerlidir. Lütfen doğrulama süresi içerisinde kullanınız.</p>
            <p>Bu işlem size ait değilse, bu e-postayı görmezden gelebilirsiniz.</p>
            <div class='footer'>
                Bu mesaj otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.
            </div>
        </div>
    </body>
    </html>
";
            await SendAsync(email, subject, body);
            return encryptedToken;
        }

        

        public async Task SendAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(configuration["Email:Address"]));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(configuration["Email:Address"], configuration["Email:AppPassword"]);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }

        public async Task<bool> VerifyCodeAsync(UserRegisterDto userRegisterDto,  string code, string protectedToken)
        {
            VerifyCodeDto tokenModel;
            try
            {
                tokenModel = tokenHelper.UnprotectVerificationToken(protectedToken);
            }
            catch
            {
                return false; //Token geçersiz veya bozuk

            }if(tokenModel.Email != userRegisterDto.Email)
            {
                return false;
            }
            if (tokenModel.Expiration < DateTime.UtcNow)
            {
                return false;

            }
            var codeHash = hashHelper.Sha256(code);
            if(codeHash != tokenModel.Code)
            {
                return false;
            }


            var result = await userService.RegisterUserAsync(userRegisterDto);


            if (!result.Succeeded)
            {

               
                return false;
            }

            var user1 = await userService.GetUserByEmailAsync(userRegisterDto.Email);

            var user = await userManager.FindByIdAsync(user1.Id.ToString());
            if(user != null)
            {
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);
            }
            return true;
        }
    }
}
