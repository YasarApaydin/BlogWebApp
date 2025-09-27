using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ErrorPageController:Controller
    {
        [Route("ErrorPage/HandleError")]
        public IActionResult HandleError(int code)
        {
            ViewBag.StatusCode = code;

         
            return View("Error");
        }
    }
}
