using AutoMapper;
using BlogWebApp.Entity.DTOs.Makales;
using BlogWebApp.Entity.DTOs.Users;
using BlogWebApp.Entity.Entities;
using BlogWebApp.Service.Extensions;
using BlogWebApp.Service.Services.Abstractions;
using BlogWebApp.Service.Services.Concreates;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.ComponentModel.DataAnnotations;
using Web.Consts;
using Web.ResultMessages;
using static Web.ResultMessages.Messages;

namespace Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IToastNotification toastNotification;
        private readonly IValidator<UserNewPasswordDto> validator1;
        private readonly IValidator<AppUser> validator;
        private readonly UserManager<AppUser> userManager;
        private readonly IArticleService articleService;
        private readonly ICategoryService categoryService;
        private readonly IValidator<Makale> validatorMakale;
        private readonly ICommentService commentService;


        public ProfileController(IUserService _userService, IMapper _mapper, IToastNotification _toastNotification, IValidator<AppUser> _validator, IValidator<UserNewPasswordDto> _validator1, UserManager<AppUser> _userManager, IArticleService _articleService, ICategoryService _categoryService, IValidator<Makale> _validatorMakale, ICommentService _commentService)
        {
            userService = _userService;
            mapper = _mapper;
            toastNotification = _toastNotification;
            validator = _validator;
            validator1 = _validator1;
            userManager = _userManager;
            articleService = _articleService;
            categoryService = _categoryService;
            validatorMakale = _validatorMakale;
            commentService = _commentService;

        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.User},{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> ProfileDetail()
        {
            var profile = await userService.GetUserProfileUpdateAsync();
            return View(profile);
        }


        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.User},{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> ProfileDetail(UserProfileEditUpdateDto userProfileEditUpdateDto)
        {
            var map = mapper.Map<AppUser>(userProfileEditUpdateDto);

            if (ModelState.IsValid)
            {
                var result = await userService.UserProfileEditUpdateAsync(userProfileEditUpdateDto);

                if (result)
                {
                    toastNotification.AddSuccessToastMessage("Bilgileriniz başarıyla güncellendi", new ToastrOptions { Title = "İşlem Başarılı." });
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var model = await userService.GetUserProfileUpdateAsync();
                    toastNotification.AddErrorToastMessage("Bilgileriniz güncellenirken bir hata oluştu.", new ToastrOptions { Title = "İşlem Tamamlanamadı." });
                    return View(model);
                }
            }
            else
            {
                toastNotification.AddErrorToastMessage("Bilgileriniz güncellenirken bir hata oluştu.", new ToastrOptions { Title = "İşlem Başarısız." });
                return View();
            }





        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.User},{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public IActionResult ChangePassword()
        {
            
            ModelState.Clear(); 
            return View(new UserNewPasswordDto());
        }



        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.User},{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> ChangePassword(UserNewPasswordDto userNewPasswordDto)
        {
            ModelState.Clear();
            var validationResult = await validator1.ValidateAsync(userNewPasswordDto);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);
                toastNotification.AddErrorToastMessage("Şifreniz ile alakalı yerleri yanlış doldurdunuz.", new ToastrOptions { Title = "İşlem Tamamlanamadı." });
               

              
                return View(userNewPasswordDto);
            }

            if (userNewPasswordDto.MevcutSifre == userNewPasswordDto.YeniSifre)
            {
                ModelState.AddModelError("YeniSifre", "Yeni şifre mevcut şifre ile aynı olamaz.");
                toastNotification.AddErrorToastMessage("Yeni şifre mevcut şifre ile aynı olamaz.", new ToastrOptions { Title = "İşlem Tamamlanamadı." });
     

               
                return View(userNewPasswordDto);
            }

          
            var user = await userManager.GetUserAsync(User);
            var passwordCheck = await userManager.CheckPasswordAsync(user, userNewPasswordDto.MevcutSifre);
            if (!passwordCheck)
            {
                ModelState.AddModelError("MevcutSifre", "Mevcut şifreniz yanlış.");
                toastNotification.AddErrorToastMessage("Mevcut şifreniz yanlış.", new ToastrOptions { Title = "İşlem Tamamlanamadı." });
                

              
                return View(userNewPasswordDto);
            }

            var result = await userService.UserNewPasswordAsync(userNewPasswordDto);
            if (result)
            {
                toastNotification.AddSuccessToastMessage("Şifreniz başarıyla güncellendi", new ToastrOptions { Title = "İşlem Başarılı." });
                return RedirectToAction("Index", "Home");
            }
            else
            {
                toastNotification.AddErrorToastMessage("Şifreniz güncellenirken bir hata oluştu.", new ToastrOptions { Title = "İşlem Tamamlanamadı." });
        

              
                return View(userNewPasswordDto);
            }
        }



        [HttpGet]
        [Authorize(Roles =$"{RoleConsts.Admin},{RoleConsts.User},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> ArticleView()
        {
          

            var article = await articleService.GetCurrentUserArticlesWithCategoryAsync();
            return View(article);
        }


        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.User},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> ArticleDrafts()
        {


            var article = await articleService.GetCurrentUserDraftsArticlesWithCategoryAsync();
            return View(article);
        }






        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.User},{RoleConsts.Superadmin}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var title = await articleService.SafeDeleteArticleAsync(id);
            toastNotification.AddSuccessToastMessage(Messages.Comment.Delete(title), new ToastrOptions() { Title = "İşlem Başarılı..." });
            return RedirectToAction("ProfileDetail", "Profile");
        }




        [HttpGet("/Profile/ArticleUpdate/{slug}")]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.User},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> ArticleUpdate(string slug)
        {


            var article = await articleService.GetArticleWithCategoryNonDeletedSlugAsync(slug);
            var categories = await categoryService.GetAllCategoriesNonDeleted();
            var map = mapper.Map<MakaleGuncelleDto>(article);
            map.Kategories = categories;

            return View(map);
        }






        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.User},{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> ArticleUpdate(MakaleGuncelleDto makaleGuncelleDto)
        {

            var map = mapper.Map<Makale>(makaleGuncelleDto);
            var reault = await validatorMakale.ValidateAsync(map);

            if (reault.IsValid)
            {
                var baslik = await articleService.UpdateArticleAsync(makaleGuncelleDto);
                toastNotification.AddSuccessToastMessage(Messages.Article.Update(baslik), new ToastrOptions() { Title = "İşlem Başarılı..." });
                return RedirectToAction("ArticleView", "Profile");
            }
            else
            {
                reault.AddToModelState(this.ModelState);
            }







            var categories = await categoryService.GetAllCategoriesNonDeleted();
            makaleGuncelleDto.Kategories = categories;


            return View(makaleGuncelleDto);
        }



        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.User},{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> CommentView()
        {
           
            var comment = await commentService.GetCurrentUserCommentsWithAsync();
            return View(comment);



        }

        [HttpGet]
        [Authorize(Roles =$"{RoleConsts.Admin},{RoleConsts.Superadmin},{RoleConsts.User}")]
        public  IActionResult CreateCv()
        {
            return View();

        }



        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin},{RoleConsts.User}")]

        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var title = await commentService.SafeDeleteCommentAsync(id);
            toastNotification.AddSuccessToastMessage(Messages.Comment.Delete(title), new ToastrOptions() { Title = "İşlem Başarılı..." });
            return RedirectToAction("CommentView", "Profile");
        }


        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin},{RoleConsts.User}")]

        public async Task<IActionResult> Publish(Guid id)
        {
            var title = await articleService.PublishDraftsMakaleAsync(id);
            toastNotification.AddSuccessToastMessage(Messages.Article.Add(title), new ToastrOptions() { Title = "İşlem Başarılı..." });
            return RedirectToAction("ArticleDrafts", "Profile");
        }




    }
}
