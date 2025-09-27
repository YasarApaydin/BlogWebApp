using BlogWebApp.Entity.DTOs.Users;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BlogWebApp.Service.Helpers.Token
{
    public  class TokenHelper: ITokenHelper
    {
      
        private readonly IDataProtector protector;
        public TokenHelper(IDataProtectionProvider _provider)
        {
            protector = _provider.CreateProtector("TokenProtectionPurpose");
        }

        public string Protect(string input)
        {
            return protector.Protect(input);
         
        }

        public string Unprotect(string protectedInput)
        {
            return protector.Unprotect(protectedInput);
        }


        public string ProtectVerificationToken(VerifyCodeDto model)
        {
            var json = JsonSerializer.Serialize(model);
            return Protect(json);

        }



      public string  ProtectRegisterUser(UserRegisterDto userRegisterDto)
        {
            var json =  JsonSerializer.Serialize(userRegisterDto);
             return  Protect(json);
        }


        public VerifyCodeDto UnprotectVerificationToken(string protectedToken)
        {
            var json = Unprotect(protectedToken);
            return JsonSerializer.Deserialize<VerifyCodeDto>(json)!;
        }

        public UserRegisterDto UnprotectUserRegisterToken(string protectedToken)
        {
            var json = Unprotect(protectedToken);
            return JsonSerializer.Deserialize<UserRegisterDto>(json)!;
        }
    }
}
