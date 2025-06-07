using Microsoft.AspNetCore.Mvc;

namespace InternetShopAspNetCoreMvc.UI.Controllers
{
	public class PageNotFoundController : Controller
	{
        public IActionResult Index()
        {
            return View();
        }
    }
}
