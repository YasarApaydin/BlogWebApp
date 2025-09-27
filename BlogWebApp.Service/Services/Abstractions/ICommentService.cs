using BlogWebApp.Entity.DTOs.YorumMakales;
using BlogWebApp.Entity.DTOs.Yorums;

namespace BlogWebApp.Service.Services.Abstractions
{
    public interface ICommentService
    {


        public Task CreateCommentsAsync(YorumMakaleDto yorumMakaleDto);

        Task<List<YorumsDto>> GetAllCommentsWithAsync();



        Task<List<YorumsDto>> GetCurrentUserCommentsWithAsync();



        Task<string> SafeDeleteCommentAsync(Guid Id);



        Task<string> HardDeleteCommentAsync(Guid Id);

        Task<string> UndoDeleteCommentAsync(Guid Id);
        
        Task<List<YorumsDto>> GetAllCommentDeletedAsync();
    }
}
