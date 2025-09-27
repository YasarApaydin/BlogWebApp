namespace BlogWebApp.Service.Helpers.SendEmail
{
    public interface IHashHelper
    {
        string Sha256(string raw);
    }
}
