using Microsoft.AspNetCore.Mvc;

namespace UniversityMgmtSystemClientConsuming.Controllers
{
	public class AdminController : Controller
	{
        public const string UserNameSection = "UserName";
        public IActionResult DashBoard()
		{
            string LoginEmail = HttpContext.Session.GetString(UserNameSection);
            if (LoginEmail == null)
            {
                return RedirectToAction("Login","Account");
            }
            return View();
		}
	}
}
