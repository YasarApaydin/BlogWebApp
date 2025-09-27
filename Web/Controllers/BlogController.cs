using BlogWebApp.Entity.DTOs.Makales;
using BlogWebApp.Service.Services.Abstractions;
using BlogWebApp.Service.Services.Concreates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Web.Consts;
using AutoMapper;
using BlogWebApp.Entity.Entities;
using NToastNotify;
using System.ComponentModel.DataAnnotations;
using Web.ResultMessages;
using FluentValidation;
using BlogWebApp.Service.Extensions;
using BlogWebApp.Entity.DTOs.Yorums;
using BlogWebApp.Entity.DTOs.YorumMakales;
using System.Security.Claims;
using BlogWebApp.Entity.Enums;
namespace Web.Controllers
{
    public class BlogController:Controller
    {
        private readonly IArticleService articleService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        private readonly IValidator<Makale> validator;
        private readonly IValidator<Yorum> validatorComment;
        private readonly IToastNotification toastNotification;
        private readonly ICommentService commentService;



       
        private readonly ClaimsPrincipal user;

        public BlogController(IArticleService _articleService, IHttpContextAccessor _httpContextAccessor, ICategoryService _categoryService,IMapper _mapper, IValidator<Makale> _validator, IToastNotification _toastNotification, ICommentService _commentService, IValidator<Yorum> _validatorComment)
        {
            articleService = _articleService;
            httpContextAccessor = _httpContextAccessor;
            categoryService = _categoryService;
            mapper = _mapper;
            validator = _validator;
            toastNotification = _toastNotification;
            commentService = _commentService;
            validatorComment = _validatorComment;
            user = httpContextAccessor.HttpContext.User;


        }


        [HttpGet]
        public async Task<IActionResult> Index(Guid? categoryId,int currentPage=1,int pageSize=3,bool isAscending = false)
        {
            var article = await articleService.GetAllByPagingAsync(categoryId, currentPage, pageSize, isAscending);
          
            return View(article);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            var article = await articleService.SearchAsync(keyword, currentPage, pageSize, isAscending);
            return View(article);
        }


       
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await categoryService.GetAllCategoriesNonDeleted();

            return View(new MakaleYayinlaDto { Kategories = categories });
        }




 [HttpGet("/Blog/Detail/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {


            var article = await articleService.GetBySlugAsync(slug);
            if (article == null)
            {
           return NotFound();
            }
               

            await articleService.ArticleVisitorAddAsync(article.Id);




            var yorumMakaleDto = new YorumMakaleDto
            {
                MakaleDto = article,
                MakaleYorumEkleDto = new MakaleYorumEkleDto
                {
                    MakaleId = article.Id
                }
            };

            return View(yorumMakaleDto);

        }



        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.User},{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> Detail(YorumMakaleDto yorumMakaleDto)
        {

            yorumMakaleDto.MakaleYorumEkleDto.UserId = user.GetLoggedInUserId();
            yorumMakaleDto.MakaleYorumEkleDto.OlusturanKullanici = user.GetLoggedInEmail();

            var map = mapper.Map<Yorum>(yorumMakaleDto.MakaleYorumEkleDto);
            var reault = await validatorComment.ValidateAsync(map);

            if (reault.IsValid)
            {
                await commentService.CreateCommentsAsync(yorumMakaleDto);
                toastNotification.AddSuccessToastMessage(Messages.Comment.Add(yorumMakaleDto.MakaleYorumEkleDto.OlusturanKullanici), new ToastrOptions() { Title = "İşlem Başarılı..." });
                var makale = await articleService.GetArticleWithCategoryNonDeletedAsync(yorumMakaleDto.MakaleYorumEkleDto.MakaleId);
               
                return RedirectToAction("Detail", new { slug = makale.Slug });


            }
            else
            {
                reault.AddToModelState(this.ModelState);

         
                var makaleDto = await articleService.ArticleVisitorAddAsync(yorumMakaleDto.MakaleYorumEkleDto.MakaleId);
                var model = new YorumMakaleDto
                {
                    MakaleDto = makaleDto,
                    MakaleYorumEkleDto = yorumMakaleDto.MakaleYorumEkleDto
                };

                return View(model);

            }



        }




        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.User},{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> Create(MakaleYayinlaDto makaleYayinlaDto,string durum)
        {

            if(Enum.TryParse<BlogDurumu>(durum, out var parsedDurum))
            {
                makaleYayinlaDto.Durum = parsedDurum;
            }
            else
            {
                makaleYayinlaDto.Durum = BlogDurumu.Taslak; 
            }


            var map = mapper.Map<Makale>(makaleYayinlaDto);
            var reault = await validator.ValidateAsync(map);

            if (reault.IsValid)
            {
                await articleService.PublishMakaleAsync(makaleYayinlaDto);
                toastNotification.AddSuccessToastMessage(Messages.Article.Add(makaleYayinlaDto.Baslik), new ToastrOptions() { Title = "İşlem Başarılı..." });
                return RedirectToAction("Index", "Home");
            }
            else
            {
                reault.AddToModelState(this.ModelState);

                var categories = await categoryService.GetAllCategoriesNonDeleted();
                makaleYayinlaDto.Kategories = categories;

                return View(makaleYayinlaDto); 
            }
        }


       


    }
}

