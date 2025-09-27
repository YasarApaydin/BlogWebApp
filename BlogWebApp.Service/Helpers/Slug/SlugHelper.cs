
using System.Text.RegularExpressions;

namespace BlogWebApp.Service.Helpers.Slug
{
    public static class SlugHelper 
    {
        public static string ToSlug(this string text)
        {
            text = text.ToLowerInvariant().Replace("ı","i").Replace("ş","s").Replace("ğ", "g")
            .Replace("ü", "u").Replace("ö", "o").Replace("ç", "c");

            text = Regex.Replace(text,@"[^a-z0-9\s-]", "");
            text = Regex.Replace(text, @"\s+", " ").Trim();
            if (text.Length > 80) text = text[..80].Trim();
            return Regex.Replace(text, @"\s", "-");
        }
    }
}
