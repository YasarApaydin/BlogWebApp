using BlogWebApp.Entity.DTOs.Makales;
using BlogWebApp.Entity.Entities;

namespace BlogWebApp.Service.Services.Abstractions
{
    public interface IArticleService
    {

        Task<List<MakaleDto>> GetAllArticlesWithCategoryNonDeletedAsync();

        Task<List<MakaleDto>> GetCurrentUserArticlesWithCategoryAsync();


        Task<List<MakaleDto>> GetCurrentUserDraftsArticlesWithCategoryAsync();

        Task<List<MakaleDto>> GetAllArticlesWithCategoryDeletedAsync();


        Task<IList<Makale>> GetTop3MostViewedAsync();

        Task<MakaleListeDto> GetAllByPagingAsync(Guid? categoryId, int currentPage = 1, int pageSize = 3, bool isAscending = false);
        Task<MakaleListeDto> SearchAsync(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false);


        Task<MakaleDto> GetArticleWithCategoryNonDeletedAsync(Guid id);



        Task<MakaleDto> GetArticleWithCategoryNonDeletedSlugAsync(string slug);




        Task<string> PublishDraftsMakaleAsync(Guid makaleId);
        Task CreateMakaleAsync(MakaleEkleDto makaleEkleDto);


        Task<MakaleDto> GetBySlugAsync(string slug);

        Task PublishMakaleAsync(MakaleYayinlaDto makaleYayinlaDto);


        Task<string> SafeDeleteArticleAsync(Guid makaleId);
        Task<string> UpdateArticleAsync(MakaleGuncelleDto makaleGuncelleDto);
        Task<string> UndoDeleteArticleAsync(Guid makaleId);


        Task<MakaleDto> ArticleVisitorAddAsync(Guid id);



    }
}
