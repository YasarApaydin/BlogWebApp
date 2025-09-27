using BlogWebApp.Entity.DTOs.Kategoris;
using BlogWebApp.Entity.Entities;

namespace BlogWebApp.Service.Services.Abstractions
{
    public interface ICategoryService
    {
          Task CreateCategoryAsync(KategoriEkleDto kategoriEkleDto);
         Task<List<KategoriDto>> GetAllCategoriesNonDeleted();
        Task<List<KategoriDto>> GetAllCategoriesNonDeletedTake();
        Task<List<KategoriDto>> GetAllCategoriesDeleted();
        Task<Kategori> GetCategoryByGuid(Guid id);
        Task<string> UpdateCategoryAsync(KategoriGuncelleDto kategoriGuncelle);
        Task<string> SafeDeleteCategoryAsync(Guid categoryId);
        Task<string> UndoDeleteCategoryAsync(Guid categoryId);
    }
}
