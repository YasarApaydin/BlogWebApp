using System.Diagnostics;
using BlogWebApp.Entity.DTOs.Emails;
using BlogWebApp.Service.Services.Abstractions;
using BlogWebApp.Service.Services.Concreates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Consts;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IArticleService articleService;
        private readonly IDashboardService dashboardService;
        private readonly IEmailSenderService emailSenderService;

        public HomeController(IEmailSenderService _emailSenderService, IDashboardService _dashboardService, IArticleService _articleService)
        {
            articleService = _articleService;
            dashboardService = _dashboardService;
            emailSenderService = _emailSenderService;
        }

        public async Task<IActionResult> Index()
        {
            var article = await articleService.GetAllArticlesWithCategoryNonDeletedAsync();
            return View(article);
        }
     
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactDto contactDto)
        {
            if (!ModelState.IsValid)
            {
                return View(contactDto);
            }

            var subject = $"Ýletiþim Formu";
            var body = $@"
     <p><strong>Ad Soyad:</strong> {contactDto.Ad} {contactDto.Soyad}</p>
        <p><strong>Email:</strong> {contactDto.Email}</p>
        <p><strong>Telefon:</strong> {contactDto.Telefon}</p>
        <p><strong>Mesaj:</strong><br>{contactDto.Mesaj}</p>";

            await emailSenderService.SendAsync("apaydinyasar0@gmail.com", subject, body);

            TempData["Message"] = "Mesajýnýz baþarýyla gönderildi.";
            return RedirectToAction("Contact");
        }


        [HttpGet]
         public async Task<IActionResult> TotalArticleCount()
        {
            var totalCount = await dashboardService.GetTotalArticleCounts();
            return Json(totalCount);

        }

     


        [HttpGet]
        public async Task<IActionResult> TotalUsersCount()
        {
            var totalCount = await dashboardService.GetTotalUsersCounts();
            return Json(totalCount);

        }

        [HttpGet]
        public async Task<IActionResult> TotalCommentsCount()
        {
            var totalCount = await dashboardService.GetTotalCommentsCounts();
            return Json(totalCount);

        }












    }
}
