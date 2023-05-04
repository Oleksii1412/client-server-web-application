using Microsoft.AspNetCore.Mvc;

namespace UniversityMgmtSystemServerApi.Controllers
{
	public class HomeController1 : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
