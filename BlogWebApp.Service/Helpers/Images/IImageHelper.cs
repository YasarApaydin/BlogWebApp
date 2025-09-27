using BlogWebApp.Entity.DTOs.Images;
using BlogWebApp.Entity.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebApp.Service.Helpers.Images
{
    public interface  IImageHelper
    {
        Task<ResimYuklemeDto> Upload(string name, IFormFile imageFile,ImageType imageType,string folderName = null);
        void Delete(string imageName);
    }
}
//asddasdasCmmeiladsasdas_85