
using BlogWebApp.Entity.DTOs.Users;
using BlogWebApp.Entity.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogWebApp.Service.Services.Abstractions
{
    public interface IUserService
    {



      
        Task<List<UserDto>> GetUsersWithRoleAsync();
        Task<List<AppRole>> GetAllRolesAsync();




        Task<IdentityResult> CreateUserAsync(UserAddDto userAddDto);

        Task<IdentityResult> RegisterUserAsync(UserRegisterDto userRegisterDto);

        Task<IdentityResult> UpdateUserAsync(UserUpdateDto userUpdateDto);


        Task<bool> UserProfileUpdateAsync(UserProfileEditDto userProfileEditDto);

        Task<bool> UserProfileEditUpdateAsync(UserProfileEditUpdateDto userProfileEditUpdateDto);




        Task<AppUser> GetUserByEmailAsync(string email);

        Task<bool> CheckEmailExistsAsync(string email);




        Task<(IdentityResult identityResult,string? email)> DeleteUserAsync(Guid userId);
        Task<AppUser> GetAppUserByIdAsync(Guid userId);
        Task<string> GetUserRoleAsync(AppUser user);

        Task<UserProfileEditUpdateDto> GetUserProfileUpdateAsync();
        Task<UserProfileDto> GetUserProfileAsync();
        Task<UserProfileEditDto> GetUserProfileEditAsync();




        Task<bool> UserNewPasswordAsync(UserNewPasswordDto userNewPasswordDto);




       


    }
}
