using BlogWebApp.Entity.DTOs.Images;
using BlogWebApp.Entity.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BlogWebApp.Service.Helpers.Images
{
    internal class ImageHelper : IImageHelper
    {
        private readonly string apiKey = "";
        private readonly IWebHostEnvironment env;
      
        public ImageHelper(IWebHostEnvironment _env)
        {
            env = _env;
         
            
        }
        private string ReplaceInvalidChars(string fileName)
        {
            return fileName.Replace("İ", "I")
                .Replace("ı", "i")
                .Replace("Ğ", "G")
                .Replace("ğ", "g")
                .Replace("Ü", "U")
                .Replace("ü", "u")
                .Replace("ş", "s")
                .Replace("Ş", "S")
                .Replace("Ö", "O")
                .Replace("ö", "o")
                .Replace("Ç", "C")
                .Replace("ç", "c")
                .Replace("é", "")
                .Replace("!", "")
                .Replace("'", "")
                .Replace("^", "")
                .Replace("+", "")
                .Replace("%", "")
                .Replace("/", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("=", "")
                .Replace("?", "")
                .Replace("_", "")
                .Replace("*", "")
                .Replace("æ", "")
                .Replace("ß", "")
                .Replace("@", "")
                .Replace("€", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("#", "")
                .Replace("$", "")
                .Replace("½", "")
                .Replace("{", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("}", "")
                .Replace(@"\", "")
                .Replace("|", "")
                .Replace("~", "")
                .Replace("¨", "")
                .Replace(",", "")
                .Replace(";", "")
                .Replace("`", "")
                .Replace(".", "")
                .Replace(":", "")
                .Replace(" ", "");



        }









        private async Task<byte[]> CompressImageAsync(IFormFile formFile, int quality = 70)
        {
            using var image = await Image.LoadAsync(formFile.OpenReadStream());
            using var ms = new MemoryStream();

            var encoder = new JpegEncoder { Quality = quality };
            await image.SaveAsJpegAsync(ms, encoder);

            return ms.ToArray();
        }

        public async Task<ResimYuklemeDto> Upload(string name, IFormFile imageFile, ImageType imageType, string folderName = null)
        {

            var compressedImage = await CompressImageAsync(imageFile);

            
            using var stream = new MemoryStream(compressedImage);

           
            using var client = new HttpClient();
            using var content = new MultipartFormDataContent();

            
            content.Add(new StreamContent(stream), "image", imageFile.FileName);

        
            var response = await client.PostAsync($"https://api.imgbb.com/1/upload?key={apiKey}", content);
            var json = await response.Content.ReadAsStringAsync();

           
            using var doc = JsonDocument.Parse(json);
            var url = doc.RootElement.GetProperty("data").GetProperty("url").GetString();

            return new ResimYuklemeDto
            {
                FullName = url
            };
        }


        public void Delete(string imageName)
        {
            // ImgBB için delete endpointi
            // Eğer silme özelliği 
        }




    }
}
