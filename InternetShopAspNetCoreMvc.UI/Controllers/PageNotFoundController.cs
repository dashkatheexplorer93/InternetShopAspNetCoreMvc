using Microsoft.AspNetCore.Mvc;

namespace InternetShopAspNetCoreMvc.UI.Controllers
{
    public class PageNotFoundController : Controller
    {
        [Route("404")]
        public IActionResult PageNotFound()
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
            return View("NotFound");
        }
    }
}
