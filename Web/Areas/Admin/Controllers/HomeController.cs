using BlogWebApp.Entity.Entities;
using BlogWebApp.Service.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Consts;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController:Controller
    {
        private readonly IArticleService articleService;
        private readonly IDashboardService dashboardService;
     
        public HomeController(IArticleService _articleService, IDashboardService _dashboardService)
        {

            articleService = _articleService;
            dashboardService = _dashboardService;
          
        }
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> Index()
        {
            var article = await articleService.GetAllArticlesWithCategoryNonDeletedAsync();
            var result = await dashboardService.GetYearlyArticleCounts();


            return View(article);

        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> TotalArticleCount()
        {
            var totalCount = await dashboardService.GetTotalArticleCounts();
            return Json(totalCount);

        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> TotalCategoryCount()
        {
            var totalCount = await dashboardService.GetTotalCategoryCounts();
            return Json(totalCount);

        }


        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> TotalUsersCount()
        {
            var totalCount = await dashboardService.GetTotalUsersCounts();
            return Json(totalCount);

        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> TotalCommentsCount()
        {
            var totalCount = await dashboardService.GetTotalCommentsCounts();
            return Json(totalCount);

        }

       



    }
}
