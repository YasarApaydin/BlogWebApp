using BlogWebApp.Entity.DTOs.Users;

namespace BlogWebApp.Service.Helpers.Token
{
    public  interface ITokenHelper
    {

        string Protect(string input);
       string Unprotect(string protectedInput);
        string ProtectRegisterUser(UserRegisterDto userRegisterDto);
        string ProtectVerificationToken(VerifyCodeDto model);
        VerifyCodeDto UnprotectVerificationToken(string protectedToken);


        UserRegisterDto UnprotectUserRegisterToken(string protectedToken);
    }
}
