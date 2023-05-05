using Microsoft.AspNetCore.Mvc;

namespace UniversityMgmtSystemClientConsuming.Controllers
{
	public class AdminController : Controller
	{
		public IActionResult DashBoard()
		{
			return View();
		}
	}
}
