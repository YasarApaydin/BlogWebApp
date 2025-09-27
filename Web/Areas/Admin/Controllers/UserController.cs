using AutoMapper;
using BlogWebApp.Data.Context;
using BlogWebApp.Data.UnitOfWorks;

using BlogWebApp.Entity.DTOs.Users;
using BlogWebApp.Entity.Entities;
using BlogWebApp.Entity.Enums;
using BlogWebApp.Service.Extensions;
using BlogWebApp.Service.Helpers.Images;
using BlogWebApp.Service.Services.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Web.ResultMessages;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController:Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        private readonly RoleManager<AppRole> roleManager;
        private readonly IValidator<AppUser> validator;
        private readonly IToastNotification toastNotification;
        private readonly AppDbContext dbContext;
        private readonly SignInManager<AppUser> signInManager;
    
        private readonly IImageHelper imageHelper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserService userService;
     
        public UserController(UserManager<AppUser> _userManager, IMapper _mapper, IUserService _userService, IUnitOfWork _unitOfWork, IImageHelper _imageHelper, RoleManager<AppRole> _roleManager, IValidator<AppUser> _validator, IToastNotification _toastNotification, AppDbContext _dbContext, SignInManager<AppUser> _signInManager)
        {
            dbContext = _dbContext;
            userManager = _userManager;
            roleManager = _roleManager;     
            validator = _validator;
            toastNotification = _toastNotification;
            mapper = _mapper;
            signInManager = _signInManager;
          
            imageHelper = _imageHelper;
            unitOfWork = _unitOfWork;
            userService = _userService;
          
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await userService.GetUsersWithRoleAsync();
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var roles = await userService.GetAllRolesAsync();
            return View(new UserAddDto { Roles=roles});
        }


        [HttpPost]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {  
            var roles = await userService.GetAllRolesAsync();
            var map = mapper.Map<AppUser>(userAddDto);
            var validation = await validator.ValidateAsync(map);

            if (ModelState.IsValid)
            {
                var result = await userService.CreateUserAsync(userAddDto);
                

                if (result.Succeeded)
                {
                  
                    toastNotification.AddSuccessToastMessage(Messages.User.Add(userAddDto.Email), new ToastrOptions() { Title = "İşlem Başarılı..." });
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                }
                else
                {
                 

                    result.AddToIdentityModelState(this.ModelState);
                    validation.AddToModelState(this.ModelState);
                    return View(new UserAddDto { Roles = roles });


                }
            }
            return View(new UserAddDto { Roles = roles });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid userId)
        {
            var roles = await userService.GetAllRolesAsync();
            var findId = await userService.GetAppUserByIdAsync(userId);

            var map = mapper.Map<UserUpdateDto>(findId);
            map.Roles = roles;
            return View(map);
        }



        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto updateDto)
        {
            var user = await userService.GetAppUserByIdAsync(updateDto.Id);
            if(user != null)
            {
        
                var roles = await userService.GetAllRolesAsync();
                if (ModelState.IsValid)
                {
                   var map= mapper.Map(updateDto, user);
                    var validation = await validator.ValidateAsync(map);

                    if (validation.IsValid)
                    {
                            user.UserName = updateDto.Email;
                           user.SecurityStamp = Guid.NewGuid().ToString();

                    var result = await userService.UpdateUserAsync(updateDto);
                    if (result.Succeeded)
                    {
                        
                        toastNotification.AddSuccessToastMessage(Messages.User.Update(updateDto.Email), new ToastrOptions { Title = "İşlem Başarılı." });
                        return RedirectToAction("Index", "User", new { Area = "Admin" });


                    }
                    else
                    {
                            result.AddToIdentityModelState(this.ModelState);

                            return View(new UserUpdateDto { Roles = roles });

                    }
                    }
                    else
                    {
                        validation.AddToModelState(this.ModelState);
                        return View(new UserUpdateDto { Roles = roles });
                    }
                
                }


            }
            return NotFound();



        }





        public async Task<IActionResult> Delete(Guid userId)
        {
          

            var result = await userService.DeleteUserAsync(userId);
            if (result.identityResult.Succeeded)
            {
                toastNotification.AddSuccessToastMessage(Messages.User.Delete(result.email!), new ToastrOptions { Title = "İşlem Başarılı." });
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }
            else
            {
                result.identityResult.AddToIdentityModelState(this.ModelState);
            }
            return NotFound();
        }

        [HttpGet]
       public async Task<IActionResult> Profile()
        {

            var profile = await userService.GetUserProfileAsync();
            return View(profile);
        }

  
        [HttpGet]
        public async Task<IActionResult> ProfileEdit()
        {
            var model = await userService.GetUserProfileEditAsync();

            return View(model);

        }



        [HttpPost]
        public async Task<IActionResult> ProfileEdit(UserProfileEditDto userProfileEditDto)
        {
           

       

            
            
            if (ModelState.IsValid)
            {
                var result = await userService.UserProfileUpdateAsync(userProfileEditDto);
             
                if (result)
                {
                    toastNotification.AddSuccessToastMessage("Bilgileriniz başarıyla güncellendi",new ToastrOptions { Title="İşlem Başarılı."});
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                }
                else
                {
                    var model = await userService.GetUserProfileEditAsync();
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


    }
}
