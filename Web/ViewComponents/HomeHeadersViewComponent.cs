using AutoMapper;
using BlogWebApp.Entity.DTOs.Users;
using BlogWebApp.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents
{
    public class HomeHeadersViewComponent:ViewComponent
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;

        public HomeHeadersViewComponent(UserManager<AppUser> _userManager, IMapper _mapper)
        {
            userManager = _userManager;
            mapper = _mapper;

            
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var loggedInUser = await userManager.GetUserAsync(HttpContext.User);
            var map = mapper.Map<UserDto>(loggedInUser);

            if(loggedInUser != null)
            {
  var role = string.Join("", await userManager.GetRolesAsync(loggedInUser));
            map.Role = role;
            }
          
            return View(map);
        }


    }
}
