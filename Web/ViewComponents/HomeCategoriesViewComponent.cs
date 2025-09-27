using BlogWebApp.Service.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents
{
    public class HomeCategoriesViewComponent :ViewComponent
    {
        private readonly ICategoryService categoryService;
        public HomeCategoriesViewComponent(ICategoryService _categoryService)
        {
            categoryService = _categoryService;
            
        }
            
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cotegories = await categoryService.GetAllCategoriesNonDeletedTake();
            return View(cotegories);

        }


    }
}
