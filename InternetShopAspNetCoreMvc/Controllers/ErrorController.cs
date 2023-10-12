using Microsoft.AspNetCore.Mvc;

namespace InternetShopAspNetCoreMvc.Controllers
{
	public class PageNotFoundController : Controller
	{
        public IActionResult Index()
        {
            return View();
        }
    }
}
