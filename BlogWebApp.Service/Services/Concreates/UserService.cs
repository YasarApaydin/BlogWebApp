using AutoMapper;
using BlogWebApp.Data.UnitOfWorks;

using BlogWebApp.Entity.DTOs.Users;
using BlogWebApp.Entity.Entities;
using BlogWebApp.Entity.Enums;
using BlogWebApp.Service.Extensions;
using BlogWebApp.Service.Helpers.Images;
using BlogWebApp.Service.Services.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogWebApp.Service.Services.Concreates
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal principal;
       
        private readonly SignInManager<AppUser> signInManager;
        private readonly IImageHelper imageHelper;
        private readonly IValidator<AppUser> validator;
        public UserService(IUnitOfWork _unitOfWork, IHttpContextAccessor _httpContextAccessor, IValidator<AppUser> _validator, IMapper _mapper, UserManager<AppUser> _userManager, RoleManager<AppRole> _roleManager, SignInManager<AppUser> _signInManager, IImageHelper _imageHelper)
        {

            httpContextAccessor = _httpContextAccessor;
            principal = httpContextAccessor.HttpContext.User;
            unitOfWork = _unitOfWork;
            mapper = _mapper;
            userManager = _userManager;
            roleManager = _roleManager;
          
            signInManager = _signInManager;
            imageHelper = _imageHelper;
            validator = _validator;
                

        }

        public async Task<IdentityResult> CreateUserAsync(UserAddDto userAddDto)
        {
            var map = mapper.Map<AppUser>(userAddDto);

            map.UserName = userAddDto.Email;
            var result = await userManager.CreateAsync(map, string.IsNullOrEmpty(userAddDto.Password) ? "" : userAddDto.Password);

            if (result.Succeeded)
            {
                var findRole = await roleManager.FindByIdAsync(userAddDto.RoleId.ToString());
                await userManager.AddToRoleAsync(map, findRole!.ToString());
                return result;
            }
            else
            {
                return result;
            }
        }


        public async Task<IdentityResult> RegisterUserAsync(UserRegisterDto userRegisterDto)
        {
            var map = mapper.Map<AppUser>(userRegisterDto);
            map.UserName = userRegisterDto.Email;

            var result = await userManager.CreateAsync(map, userRegisterDto.Password ?? "");

            if (result.Succeeded)
            {
               
                var defaultRoleId = "96075b4b-b4c9-44b2-b1c1-7073aa6edb39";

                var findRole = await roleManager.FindByIdAsync(defaultRoleId);

                if (findRole != null)
                {
                    await userManager.AddToRoleAsync(map, "User");

                }

                return result;
            }

            return result;
        }


        public async Task<List<AppRole>> GetAllRolesAsync()
        {
            return await roleManager.Roles.ToListAsync();
        }

        public  async Task<AppUser> GetAppUserByIdAsync(Guid userId)
        {
            return await userManager.FindByIdAsync(userId.ToString());
        }

    
        public async Task<List<UserDto>> GetUsersWithRoleAsync()
        {
            var user = await userManager.Users.ToListAsync();
            var map = mapper.Map<List<UserDto>>(user);

            foreach (var item in map)
            {
                var findUser = await userManager.FindByIdAsync(item.Id.ToString());

                var role = string.Join("", await userManager.GetRolesAsync(findUser!));
                item.Role = role;
            }

            return map;
        }
         public async Task<string> GetUserRoleAsync(AppUser user)
        {
            return string.Join("", await userManager.GetRolesAsync(user));
        }

        public async Task<IdentityResult> UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            var user = await GetAppUserByIdAsync(userUpdateDto.Id);
            var userRole = await GetUserRoleAsync(user);
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await userManager.RemoveFromRoleAsync(user, userRole);
                var findRole = await roleManager.FindByIdAsync(userUpdateDto.RoleId.ToString());
                await userManager.AddToRoleAsync(user, findRole!.Name!);
                return result;
            }
            else
            {
                return result;
            }
        }

        public async Task<(IdentityResult identityResult, string? email)> DeleteUserAsync(Guid userId)
        {
            var user = await GetAppUserByIdAsync(userId);

            var result= await userManager.DeleteAsync(user!);
            if (result.Succeeded)
            {
                return (result,user.Email);
            }
            else
            {
                return (result,null);
            }
        }

        public async Task<UserProfileDto> GetUserProfileAsync()
        {

            var userId = principal.GetLoggedInUserId();
            var user = await GetAppUserByIdAsync(userId);
          
            var getImage = await unitOfWork.GetRepository<AppUser>().GetAsync(x => x.Id == userId, x => x.Resim);


            var map = mapper.Map<UserProfileDto>(user);
            var role = string.Join("", await userManager.GetRolesAsync(user));
           
            map.Role = role;
            map.Resim.DosyaYolu = getImage.Resim.DosyaYolu;

            return map;
        }

        public async Task<UserProfileEditDto> GetUserProfileEditAsync()
        {
            var userId = principal.GetLoggedInUserId();


            var getImage = await unitOfWork.GetRepository<AppUser>().GetAsync(x => x.Id == userId, x => x.Resim);
         

            var userProfileDto = mapper.Map<UserProfileEditDto>(getImage);
            userProfileDto.Resim.DosyaYolu = getImage.Resim.DosyaYolu;

       


            return userProfileDto;
        }

        public async Task<bool> UserProfileUpdateAsync(UserProfileEditDto userProfileEditDto)
        {
            var userId = principal.GetLoggedInUserId();
            var user = await GetAppUserByIdAsync(userId);

          

            var isVerified = await userManager.CheckPasswordAsync(user, userProfileEditDto.CurrentPassword);

            

            if (isVerified && userProfileEditDto.NewPassword != null )
            {
                var result = await userManager.ChangePasswordAsync(user, userProfileEditDto.CurrentPassword, userProfileEditDto.NewPassword);
          
                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);
                    await signInManager.SignOutAsync();
                    await signInManager.PasswordSignInAsync(user, userProfileEditDto.NewPassword, true, false);



                    //mapper.Map(githubUserDto.UserProfileEditDto, user);
                    user.FirstName = userProfileEditDto.FirstName;
                    user.LastName = userProfileEditDto.LastName;
                    user.PhoneNumber = userProfileEditDto.PhoneNumber;
                    
                 
                    if(userProfileEditDto.Photo != null)
                    {
                    user.ResimId = await UploadImage(userProfileEditDto);
                    }
                    
                    await userManager.UpdateAsync(user);

                    await unitOfWork.SaveAsync();

                    return true;
                }
                else
                {


                    return false;
                }
            }
            else if (isVerified )
            {

                await userManager.UpdateSecurityStampAsync(user);

              


                //mapper.Map(githubUserDto.UserProfileEditDto, user);

                user.FirstName = userProfileEditDto.FirstName;
                user.LastName = userProfileEditDto.LastName;
                user.PhoneNumber = userProfileEditDto.PhoneNumber;



               
                if (userProfileEditDto.Photo != null)
                {
                    user.ResimId = await UploadImage(userProfileEditDto);
                }

              
                await userManager.UpdateAsync(user);
                await unitOfWork.SaveAsync();

                return true;
            }
            else 
            {

             return false;
            }



        }


        private async Task<Guid>  UploadImage(UserProfileEditDto userProfileEditDto)
        {
            var userEmail = principal.GetLoggedInEmail();
            var imageUpload = await imageHelper.Upload($"{userProfileEditDto.FirstName}{userProfileEditDto.LastName}", userProfileEditDto.Photo, ImageType.User);
           
            Resim image = new(imageUpload.FullName, userEmail);
            await unitOfWork.GetRepository<Resim>().AddAsync(image);
            return image.Id;
        }

        public async Task<UserProfileEditUpdateDto> GetUserProfileUpdateAsync()
        {
            var userId = principal.GetLoggedInUserId();
            var user = await GetAppUserByIdAsync(userId);

            var getImage = await unitOfWork.GetRepository<AppUser>().GetAsync(x => x.Id == userId, x => x.Resim);


            var map = mapper.Map<UserProfileEditUpdateDto>(user);
            var role = string.Join("", await userManager.GetRolesAsync(user));

            map.Role = role;
            map.Resim.DosyaYolu = getImage.Resim.DosyaYolu;

            return map;
        }




       

        public async Task<bool> UserProfileEditUpdateAsync(UserProfileEditUpdateDto userProfileEditUpdateDto)
        {
            var userId = principal.GetLoggedInUserId();
            var user = await GetAppUserByIdAsync(userId);

         

            // Kullanıcı bilgilerini güncelle
            user.FirstName = userProfileEditUpdateDto.FirstName;
            user.LastName = userProfileEditUpdateDto.LastName;
            user.PhoneNumber = userProfileEditUpdateDto.PhoneNumber;
            user.Email = userProfileEditUpdateDto.Email;
            // Fotoğraf yüklendiyse güncelle
            if (userProfileEditUpdateDto.Photo != null)
            {
                user.ResimId = await UploadImage1(userProfileEditUpdateDto);
            }

            // Kullanıcıyı güncelle
            await userManager.UpdateAsync(user);
            await unitOfWork.SaveAsync();

            return true;
        }

        private async Task<Guid> UploadImage1(UserProfileEditUpdateDto userProfileEditUpdateDto)
        {
            var userEmail = principal.GetLoggedInEmail();
            var imageUpload = await imageHelper.Upload($"{userProfileEditUpdateDto.FirstName}{userProfileEditUpdateDto.LastName}", userProfileEditUpdateDto.Photo, ImageType.User);
          
            Resim image = new(imageUpload.FullName, userEmail);
            await unitOfWork.GetRepository<Resim>().AddAsync(image);
            return image.Id;
        }
        public async Task<bool> UserNewPasswordAsync(UserNewPasswordDto userNewPasswordDto)
        {
            var userId = principal.GetLoggedInUserId();
            var user = await GetAppUserByIdAsync(userId);

        
            var isVerified = await userManager.CheckPasswordAsync(user, userNewPasswordDto.MevcutSifre);



            if (isVerified && userNewPasswordDto.YeniSifre != null)
            {
                var result = await userManager.ChangePasswordAsync(user, userNewPasswordDto.MevcutSifre, userNewPasswordDto.YeniSifre);

                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);
                    await signInManager.SignOutAsync();
                    await signInManager.PasswordSignInAsync(user, userNewPasswordDto.YeniSifre, true, false);


                   

                 

            

                    await userManager.UpdateAsync(user);

                    await unitOfWork.SaveAsync();

                    return true;
                }
                else
                {


                    return false;
                }
            }
            else if (isVerified)
            {

                await userManager.UpdateSecurityStampAsync(user);

              

              




                await userManager.UpdateAsync(user);
                await unitOfWork.SaveAsync();

                return true;
            }
            else
            {

                return false;
            }

        }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await userManager.FindByEmailAsync(email) != null;
        }

   




    }
}
