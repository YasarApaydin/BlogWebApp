using AutoMapper;
using BlogWebApp.Entity.DTOs.Makales;
using BlogWebApp.Entity.Entities;
using BlogWebApp.Entity.Enums;
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
    public class ArticleController: Controller
    {
        private readonly IArticleService articleService;
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        private readonly IValidator<Makale> validator;
        private readonly IToastNotification toastNotification;
        public ArticleController(IArticleService _articleService, ICategoryService _categoryService, IMapper _mapper, IValidator<Makale> _validator, IToastNotification _toastNotification)
        {
            toastNotification = _toastNotification;
            validator = _validator;
            articleService = _articleService;
            categoryService = _categoryService;
            mapper = _mapper;

        }


        [HttpGet]
        [Authorize(Roles =$"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> Index()
        {

            var article = await articleService.GetAllArticlesWithCategoryNonDeletedAsync();
            return View(article);
        }
       

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> Add()
        {
            


            var categories = await categoryService.GetAllCategoriesNonDeleted();

            return View(new MakaleEkleDto {Kategories=categories });
        }



        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> Add(MakaleEkleDto makaleEkleDto)
        {
            makaleEkleDto.Durum = BlogDurumu.Yayinda;

            var map = mapper.Map<Makale>(makaleEkleDto);
            var reault = await validator.ValidateAsync(map);

            if (reault.IsValid)
            {
                  await articleService.CreateMakaleAsync(makaleEkleDto);
                toastNotification.AddSuccessToastMessage(Messages.Article.Add(makaleEkleDto.Baslik),new ToastrOptions() { Title="İşlem Başarılı..."});
           return RedirectToAction("Index", "Article", new { Area = "Admin" });

            }
            else
            {
            reault.AddToModelState(this.ModelState); 
                
            var categories = await categoryService.GetAllCategoriesNonDeleted();

            return View(new MakaleEkleDto { Kategories = categories });
            }



         
         
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> Update(Guid id)
        {

            var article = await articleService.GetArticleWithCategoryNonDeletedAsync(id);
            var categories = await categoryService.GetAllCategoriesNonDeleted();
            var map = mapper.Map<MakaleGuncelleDto>(article);
            map.Kategories = categories;

            return View(map);


          
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> Update(MakaleGuncelleDto makaleGuncelleDto)
        {

            var map = mapper.Map<Makale>(makaleGuncelleDto);
            var reault = await validator.ValidateAsync(map);

            if (reault.IsValid)
            {
               var baslik= await articleService.UpdateArticleAsync(makaleGuncelleDto);
                toastNotification.AddSuccessToastMessage(Messages.Article.Update(baslik),new ToastrOptions() { Title="İşlem Başarılı..."});
                return RedirectToAction("Index", "Article", new { Area = "Admin" });
            }
            else
            {
                reault.AddToModelState(this.ModelState);
            }






         
            var categories = await categoryService.GetAllCategoriesNonDeleted();
            makaleGuncelleDto.Kategories = categories;


            return View(makaleGuncelleDto);
        }









        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> Delete(Guid id)
        {

           var title= await articleService.SafeDeleteArticleAsync(id);
            toastNotification.AddSuccessToastMessage(Messages.Article.Delete(title),new ToastrOptions() { Title="İşlem Başarılı..."});
            return RedirectToAction("Index","Article",new {Area="Admin"});
        }



        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin},{RoleConsts.User}")]
        public async Task<IActionResult> DeletedArticle()
        {

            var article = await articleService.GetAllArticlesWithCategoryDeletedAsync();
            return View(article);
        }




        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> UndoDelete(Guid id)
        {

            var title = await articleService.UndoDeleteArticleAsync(id);
            toastNotification.AddSuccessToastMessage(Messages.Article.UndoDelete(title), new ToastrOptions() { Title = "İşlem Başarılı..." });
            return RedirectToAction("Index", "Article", new { Area = "Admin" });
        }

    }
}
