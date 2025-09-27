using AutoMapper;
using BlogWebApp.Data.UnitOfWorks;
using BlogWebApp.Entity.DTOs.Users;
using BlogWebApp.Entity.Entities;
using BlogWebApp.Service.Extensions;
using BlogWebApp.Service.Helpers.Token;
using BlogWebApp.Service.Services.Abstractions;
using BlogWebApp.Service.Services.Concreates;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NToastNotify;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Web.ResultMessages;

namespace Web.Controllers
{
    public class RegisterController:Controller
    {
        private readonly IMapper mapper;
        private readonly IValidator<AppUser> validator;
        private readonly IUserService userService;
        private readonly IToastNotification toastNotification;
        private readonly IEmailSenderService emailSender;
        private readonly IUnitOfWork unitOfWork;
        private readonly ITokenHelper tokenHelper;
        public RegisterController(ITokenHelper _tokenHelper, IUnitOfWork _unitOfWork, IMapper _mapper, IValidator<AppUser> _validator, IUserService _userService, IToastNotification _toastNotification, IEmailSenderService _emailSender)
        {
            userService = _userService;
            mapper = _mapper;
            toastNotification = _toastNotification;
            validator = _validator;
            emailSender = _emailSender;
            unitOfWork = _unitOfWork;
            tokenHelper = _tokenHelper;
        }

        public IActionResult Account()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Account(UserRegisterDto userRegisterDto)
        {


            if (!ModelState.IsValid)
            {
                return View(userRegisterDto);
            }
              


            var userExists = await userService.CheckEmailExistsAsync(userRegisterDto.Email);
            if (userExists)
            {
                ModelState.AddModelError("Email", "Bu e‑posta adresi zaten kayıtlı.");
                return View(userRegisterDto);
            }




            var map = mapper.Map<AppUser>(userRegisterDto);
            var validation = await validator.ValidateAsync(map);


            if (!validation.IsValid)
            {
                validation.AddToModelState(this.ModelState);
                return View();
            }


            var userToken =  tokenHelper.ProtectRegisterUser(userRegisterDto);
            TempData["UserToken"] = userToken;


            var token = await emailSender.GenerateAndSendAsync(userRegisterDto.Email,userRegisterDto.FirstName);
            toastNotification.AddSuccessToastMessage(Messages.User.Code(userRegisterDto.Email), new ToastrOptions() { Title = "Devam edelim..." });
            
            return Redirect($"/Register/VerifyCode?token={token}");

        }



        [HttpGet]
        public async Task<IActionResult> VerifyCode(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return BadRequest("Token eksik.");


            var userToken = TempData["UserToken"]?.ToString();
            TempData.Keep("UserToken");

            try
            {
                var tokenModel = tokenHelper.UnprotectVerificationToken(token);
                if (tokenModel.Expiration < DateTime.UtcNow)
                    return NotFound("Kodun süresi dolmuş.");



                var dto = new VerifyCodeDto
                {
                    
                     Email = tokenModel.Email,
                     Code = tokenModel.Code,
                    Expiration = tokenModel.Expiration


                };

                // UserId'i TempData'ya atıyoruz (POST'ta kullanacağız)
              
                ViewBag.Token = token;
                return View(dto);
            }
            catch
            {
                toastNotification.AddErrorToastMessage(Messages.User.Token(), new ToastrOptions() { Title = "İşlem Başarısız..." });
               return View("Account");
            }






        }


        [HttpPost]
        public async Task<IActionResult> VerifyCode(VerifyCodeDto dto, string token)
        {
           
            if (!ModelState.IsValid)
            {
                return View(dto);
            }


            var userToken = TempData["UserToken"]?.ToString();
            TempData.Keep("UserToken");

            var userRegisterDto = tokenHelper.UnprotectUserRegisterToken(userToken);

      

            var ok = await emailSender.VerifyCodeAsync(userRegisterDto, dto.Code,token);
            if (!ok)
            {
                ModelState.AddModelError("", " Kod hatalı veya süresi doldu.");

                return View(dto);
            }
          




            toastNotification.AddSuccessToastMessage(Messages.User.Verify(),new ToastrOptions { Title = "Başarılı!" });

            return RedirectToAction("Index", "Home");
        }



        [HttpPost]
        public async Task<IActionResult> ResendCode()
        {
            

            // Kod tekrar gönderme süresi kontrolü (30 saniye bekleme)
            if (TempData.TryGetValue("lastCodeSentTime", out var lastTimeObj) &&
                DateTime.TryParse(lastTimeObj.ToString(), out var lastSent) &&
                lastSent.AddSeconds(30) > DateTime.UtcNow)
            {
                toastNotification.AddWarningToastMessage("Yeni kod göndermek için biraz bekleyin.", new ToastrOptions { Title = "Bekleyin..." });
           
                TempData.Keep("lastCodeSentTime"); 
                return RedirectToAction("VerifyCode", "Register");
            }



            var userToken = TempData["UserToken"]?.ToString();
            TempData.Keep("UserToken");

            var userRegisterDto = tokenHelper.UnprotectUserRegisterToken(userToken);

            var token = await emailSender.GenerateAndSendAsync(userRegisterDto.Email,userRegisterDto.FirstName);
           
            TempData["lastCodeSentTime"] = DateTime.UtcNow.ToString();

            toastNotification.AddSuccessToastMessage(Messages.User.ResendCode(), new ToastrOptions { Title = "Başarılı!" });
            return RedirectToAction("VerifyCode", new { token });
        }


    }
}
