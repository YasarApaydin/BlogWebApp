using AutoMapper;
using BlogWebApp.Data.UnitOfWorks;
using BlogWebApp.Entity.DTOs.YorumMakales;
using BlogWebApp.Entity.DTOs.Yorums;
using BlogWebApp.Entity.Entities;
using BlogWebApp.Service.Extensions;
using BlogWebApp.Service.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BlogWebApp.Service.Services.Concreates
{
    public class CommentService : ICommentService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal user;
        
        

        public CommentService(IUnitOfWork _unitOfWork, IMapper _mapper, IHttpContextAccessor _httpContextAccessor)
        {
            mapper = _mapper;

            httpContextAccessor = _httpContextAccessor;
            unitOfWork = _unitOfWork;

            user = httpContextAccessor.HttpContext.User;

        }

        public async Task CreateCommentsAsync(YorumMakaleDto yorumMakaleDto)
        {

          
            var yorum = new Yorum(yorumMakaleDto.MakaleYorumEkleDto.MakaleId, yorumMakaleDto.MakaleYorumEkleDto.Icerik, yorumMakaleDto.MakaleYorumEkleDto.UserId, yorumMakaleDto.MakaleYorumEkleDto.OlusturanKullanici);

            await unitOfWork.GetRepository<Yorum>().AddAsync(yorum);
            await unitOfWork.SaveAsync();

        }

        public async Task<List<YorumsDto>> GetAllCommentsWithAsync()
        {
            var userId =  user.GetLoggedInUserId();
            var comments = await unitOfWork.GetRepository<Yorum>().GetAllAsync(x => !x.IsDeleted ,y => y.User, y => y.Makale,c => c.User.Resim);
            var map = mapper.Map<List<YorumsDto>>(comments);
            return map;
        }

        public async Task<string> SafeDeleteCommentAsync(Guid Id)
        {
            var userEmail = user.GetLoggedInEmail();
            var comment = await unitOfWork.GetRepository<Yorum>().GetByGuidAsync(Id);
            comment.IsDeleted = true;
            comment.SilinmeTarihi = DateTime.Now;
            comment.SilenKullanici = userEmail;
            await unitOfWork.GetRepository<Yorum>().UpdateAsync(comment);
            await unitOfWork.SaveAsync();
            return comment.Icerik?.Length > 10
                ? comment.Icerik.Substring(0, 10)
                : comment.Icerik;

        }

        public async Task<string> HardDeleteCommentAsync(Guid Id)
        {
            var comment = await unitOfWork.GetRepository<Yorum>().GetByGuidAsync(Id);
           if(comment is null)
            {
                return "Silinecek Yorum Bulunamadı.";
            }
            await unitOfWork.GetRepository<Yorum>().DeleteAsync(comment);
            await unitOfWork.SaveAsync();
            return "Yorum kalıcı olarak silindi.";
        }

        public async Task<List<YorumsDto>> GetAllCommentDeletedAsync()
        {
            var comment = await unitOfWork.GetRepository<Yorum>().GetAllAsync(x => x.IsDeleted, y => y.User, y => y.Makale, c => c.User.Resim);
            var map = mapper.Map<List<YorumsDto>>(comment);
            return map;
        }

        public async Task<string> UndoDeleteCommentAsync(Guid Id)
        {
            var comment = await unitOfWork.GetRepository<Yorum>().GetByGuidAsync(Id);
            comment.IsDeleted = false;
            comment.SilinmeTarihi = null;
            comment.SilenKullanici = null;
            await unitOfWork.GetRepository<Yorum>().UpdateAsync(comment);
            await unitOfWork.SaveAsync();
            return comment.Icerik?.Length > 10
                ? comment.Icerik.Substring(0, 10)
                : comment.Icerik;
        }

        public async Task<List<YorumsDto>> GetCurrentUserCommentsWithAsync()
        {
            var userId = user.GetLoggedInUserId();
            var comments = await unitOfWork.GetRepository<Yorum>().GetAllAsync(x => !x.IsDeleted && x.UserId== userId, y => y.User, y => y.Makale, c => c.User.Resim);
            var map = mapper.Map<List<YorumsDto>>(comments);
            return map;
        }
    }
}
