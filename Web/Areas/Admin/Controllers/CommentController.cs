using AutoMapper;
using BlogWebApp.Entity.Entities;
using BlogWebApp.Service.Services.Abstractions;
using BlogWebApp.Service.Services.Concreates;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Web.Consts;
using Web.ResultMessages;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentController:Controller
    {

   
        
        private readonly IToastNotification toastNotification;
        private readonly ICommentService commentService;
        public CommentController(IToastNotification _toastNotification, ICommentService _commentService)
        {
            commentService = _commentService;
            toastNotification = _toastNotification;

        }


        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> Index()
        {

            var comments = await commentService.GetAllCommentsWithAsync();
            return View(comments);
        }


        [HttpGet]
        [Authorize(Roles =$"{RoleConsts.Admin},{RoleConsts.Superadmin}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var title = await commentService.SafeDeleteCommentAsync(id);
            toastNotification.AddSuccessToastMessage(Messages.Comment.Delete(title), new ToastrOptions() { Title = "İşlem Başarılı..." });
            return RedirectToAction("Index", "Comment", new { Area = "Admin" });
        }


        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]

        public async Task<IActionResult> DeletedComments()
        {
            var comment = await commentService.GetAllCommentDeletedAsync();
            return View(comment);
        }




        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> UndoDelete(Guid id)
        {
            var title = await commentService.UndoDeleteCommentAsync(id);
            toastNotification.AddSuccessToastMessage(Messages.Comment.UndoDelete(title), new ToastrOptions() { Title = "İşlem Başarılı..." });
            return RedirectToAction("Index", "Comment", new { Area = "Admin" });
        }



        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Superadmin}")]
        public async Task<IActionResult> HardDelete(Guid id)
        {
            var title = await commentService.HardDeleteCommentAsync(id);
            toastNotification.AddSuccessToastMessage(Messages.Comment.Hard(title), new ToastrOptions() { Title = "İşlem Başarılı..." });
            return RedirectToAction("DeletedComments", "Comment", new { Area = "Admin" });
        }

    }
}
