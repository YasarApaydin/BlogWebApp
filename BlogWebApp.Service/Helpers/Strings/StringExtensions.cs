namespace BlogWebApp.Service.Helpers.Strings
{
    public static class StringExtensions
    {
        public static string ToHtmlWithBreaks(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            return text.Replace("\n", "<br />");
        }
    }
}
