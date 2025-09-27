using AutoMapper;
using BlogWebApp.Data.UnitOfWorks;
using BlogWebApp.Entity.DTOs.Kategoris;
using BlogWebApp.Entity.Entities;
using BlogWebApp.Service.Extensions;
using BlogWebApp.Service.Helpers.Images;
using BlogWebApp.Service.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BlogWebApp.Service.Services.Concreates
{
    public class CategoryService : ICategoryService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal userClaim;
        public CategoryService(IUnitOfWork _unitOfWork, IMapper _mapper, IHttpContextAccessor _httpContextAccessor, IImageHelper _imageHelper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
            httpContextAccessor = _httpContextAccessor;
            userClaim = httpContextAccessor.HttpContext.User;
            
        }
        public async Task<List<KategoriDto>> GetAllCategoriesNonDeleted()
        {
           
            var categories = await unitOfWork.GetRepository<Kategori>().GetAllAsync(x=>!x.IsDeleted);
            var map = mapper.Map<List<KategoriDto>>(categories);
            return map;
        }


        public async Task<List<KategoriDto>> GetAllCategoriesNonDeletedTake()
        {
            var categories = await unitOfWork.GetRepository<Kategori>().GetAllAsync(x => !x.IsDeleted);
            var map = mapper.Map<List<KategoriDto>>(categories);


            return map.Take(24).ToList();
        }





        public async Task CreateCategoryAsync(KategoriEkleDto kategoriEkleDto)
        {
           
            var userEmail = userClaim.GetLoggedInEmail();
            Kategori category = new(kategoriEkleDto.Ad,userEmail);
           await unitOfWork.GetRepository<Kategori>().AddAsync(category);
            await unitOfWork.SaveAsync();
           
        }


        public async Task<Kategori> GetCategoryByGuid(Guid id)
        {
            var category = await unitOfWork.GetRepository<Kategori>().GetByGuidAsync(id);
            return category;
        }



        public async Task<string> UpdateCategoryAsync(KategoriGuncelleDto kategoriGuncelle)
        {
            var user = userClaim.GetLoggedInEmail();
            var category = await unitOfWork.GetRepository<Kategori>().GetAsync(x => !x.IsDeleted && x.Id == kategoriGuncelle.Id);
            category.Ad = kategoriGuncelle.Ad;
            category.DegistirenKullanici = user;
            category.DegistirilmeTarihi = DateTime.Now;
            await unitOfWork.GetRepository<Kategori>().UpdateAsync(category);
            await unitOfWork.SaveAsync();
            return category.Ad;


        }

        public async Task<string> SafeDeleteCategoryAsync(Guid categoryId)
        {
            var user = userClaim.GetLoggedInEmail();
            var category = await unitOfWork.GetRepository<Kategori>().GetByGuidAsync(categoryId);
            category.IsDeleted = true;
            category.SilinmeTarihi = DateTime.Now;
            category.SilenKullanici = user;
            await unitOfWork.GetRepository<Kategori>().UpdateAsync(category);
            await unitOfWork.SaveAsync();

            return category.Ad;
        }

        public async Task<List<KategoriDto>> GetAllCategoriesDeleted()
        {
            var categories = await unitOfWork.GetRepository<Kategori>().GetAllAsync(x => x.IsDeleted);
            var map = mapper.Map<List<KategoriDto>>(categories);
            return map;
        }

        public async Task<string> UndoDeleteCategoryAsync(Guid categoryId)
        {
        
            var category = await unitOfWork.GetRepository<Kategori>().GetByGuidAsync(categoryId);
            category.IsDeleted = false;
            category.SilinmeTarihi = null;
            category.SilenKullanici = null;
            await unitOfWork.GetRepository<Kategori>().UpdateAsync(category);
            await unitOfWork.SaveAsync();

            return category.Ad;
        }

    
    
    }
}
