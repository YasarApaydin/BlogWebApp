using AutoMapper;
using BlogWebApp.Data.UnitOfWorks;
using BlogWebApp.Entity.DTOs.Kategoris;
using BlogWebApp.Entity.Entities;
using BlogWebApp.Service.Extensions;
using BlogWebApp.Service.Services.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Web.Consts;
using Web.ResultMessages;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController:Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IValidator<Kategori> validator;
        private readonly IMapper mapper;
        private readonly IToastNotification toastNotification;
            


        public CategoryController(ICategoryService _categoryService, IValidator<Kategori> _validator, IMapper _mapper, IToastNotification _toastNotification)
        {
            mapper = _mapper;
            validator = _validator;
            categoryService = _categoryService;
            toastNotification = _toastNotification;
        }

        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> Index()
        {
            var category = await categoryService.GetAllCategoriesNonDeleted();
            return View(category);
        }

        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> DeletedCategory()
        {
            var category = await categoryService.GetAllCategoriesDeleted();
            return View(category);
        }


        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(KategoriEkleDto kategoriEkleDto)
        {
            var map = mapper.Map<Kategori>(kategoriEkleDto);

            var result = await validator.ValidateAsync(map);
            if (result.IsValid)
            {
               await categoryService.CreateCategoryAsync(kategoriEkleDto);
                toastNotification.AddSuccessToastMessage(Messages.Category.Add(kategoriEkleDto.Ad), new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("Index", "Category", new { Area = "Admin" });

            }
         
                result.AddToModelState(this.ModelState);
                return View();  

            
                

        }


        [HttpPost] 
        public async Task<IActionResult> AddWithAjax([FromBody] KategoriEkleDto kategoriEkleDto)
         {
            var map = mapper.Map<Kategori>(kategoriEkleDto);
            var result = await validator.ValidateAsync(map);
            if (result.IsValid)
            {
                await categoryService.CreateCategoryAsync(kategoriEkleDto);
                toastNotification.AddSuccessToastMessage(Messages.Category.Add(kategoriEkleDto.Ad), new ToastrOptions { Title = "İşlem Başarılı"});
                return Json(Messages.Category.Add(kategoriEkleDto.Ad));
            }
            else
            {
                toastNotification.AddErrorToastMessage(result.Errors.First().ErrorMessage, new ToastrOptions { Title = "İşlem Başarısız" });
                return Json(result.Errors.First().ErrorMessage);
            }
          


        }


        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> Update(Guid categoryid)
        {
            var category = await categoryService.GetCategoryByGuid(categoryid);
            var map = mapper.Map<Kategori, KategoriGuncelleDto>(category);
            return View(map);
        }
        

        [HttpPost] 
        public async Task<IActionResult> Update(KategoriGuncelleDto kategoriGuncelleDto)
        {
            var map = mapper.Map<Kategori>(kategoriGuncelleDto);

            var result = await validator.ValidateAsync(map);
            if (result.IsValid)
            {
                var name = await categoryService.UpdateCategoryAsync(kategoriGuncelleDto);
                toastNotification.AddSuccessToastMessage(Messages.Category.Update(name), new ToastrOptions { Title = "İşlem Başarılı." });
                return RedirectToAction("Index","Category",new {Area="Admin"});
            }

            result.AddToModelState(this.ModelState);
            return View();
        
        }
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> Delete(Guid categoryid)
        {
            var adi = await categoryService.SafeDeleteCategoryAsync(categoryid);
            toastNotification.AddSuccessToastMessage(Messages.Category.UndoDelete(adi), new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("Index", "Category", new { Area = "Admin" });
        
        }

        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> UndoDelete(Guid categoryid)
        {
            var adi = await categoryService.UndoDeleteCategoryAsync(categoryid);
            toastNotification.AddSuccessToastMessage(Messages.Category.Delete(adi), new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("Index", "Category", new { Area = "Admin" });

        }
    }
}
