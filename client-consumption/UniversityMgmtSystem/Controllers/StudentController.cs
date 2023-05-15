using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using UniversityMgmtSystemClientConsuming.Models.ControllerModel;
using UniversityMgmtSystemClientConsuming.ViewModels;

namespace UniversityMgmtSystemClientConsuming.Controllers
{
	
	public class StudentController : Controller
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

		[HttpGet]
		public async Task<IActionResult> GetProfile()
		{
			string StudentEmail=HttpContext.Session.GetString(UserNameSection);

			Student student = new Student();
			using (HttpClient httpClient = new HttpClient())
			{

				var response = await httpClient.GetAsync($"https://localhost:7003/api/Student/GetProfile/{StudentEmail}");
				if(response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();

					student = JsonConvert.DeserializeObject<Student>(content);
					return View(student);
				}


				ViewData["Error"] = response.ReasonPhrase;
				return View();
			}


			
		}
		[HttpPost]
		public async Task<IActionResult> UpdateProfile(Student student)
		{
			using(HttpClient httpClient = new HttpClient())
			{
				var response = await httpClient.PostAsJsonAsync("https://localhost:7003/api/Student/EditProfile", student);
				if(response.IsSuccessStatusCode)
				{
					return RedirectToAction("DashBoard");
				}
			}
		    

			return RedirectToAction("GetProfile");
			
		}


		[HttpGet]
		public async Task<IActionResult> GetCourse()
		{
			List<Course> courses = new List<Course>();
			using (HttpClient httpClient = new HttpClient())
			{

				var response = await httpClient.GetAsync("https://localhost:7003/api/Course/GetCourses");

				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();

					courses = JsonConvert.DeserializeObject<List<Course>>(content);
					return View(courses);
				}

			}
			return BadRequest();
			
		}
		[HttpGet]
		public async Task<IActionResult> EnrollCourse (int id)
		{
			string LoginEmail = HttpContext.Session.GetString(UserNameSection);
			if(LoginEmail==null)
			{
				ViewData["Error"] = "Login Again";
				return View();
			}
			EnrollCourse enrollCourse = new EnrollCourse()
			{
				CourseId = id,
				StudentEmail = LoginEmail
			};
			using(HttpClient httpClient = new HttpClient()) 
			{

				var response = await httpClient.PostAsJsonAsync("https://localhost:7003/api/Student/EnrollCourse",
					enrollCourse);
				if(!response.IsSuccessStatusCode)
				{
					ViewData["Error"] = response.ReasonPhrase;
					return View();
				}
				return RedirectToAction("GetEnrollCourse");
			
			}
		}
		[HttpGet]
		public  async Task<IActionResult> GetEnrollCourse()
		{
			using (HttpClient httpClient = new HttpClient())
			{
				string LoginEmail = HttpContext.Session.GetString(UserNameSection);
				if (LoginEmail == null)
				{
					ViewData["Error"] = "Login Again";
					return View();
				}
				List<Course> courses = new List<Course>();
				var response = await httpClient.GetAsync("https://localhost:7003/api/Student/GetEnrollCourse/"+
					LoginEmail);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					courses = JsonConvert.DeserializeObject<List<Course>>(content);
					return View(courses);
				};

				ViewData["Error"] = response.ReasonPhrase;
			return View();
			}
		}
		[HttpGet]
		public async Task<IActionResult> UnEnrollCourse(int id)
		{
			using (HttpClient httpClient = new HttpClient())
			{

				string LoginEmail = HttpContext.Session.GetString(UserNameSection);
				if (LoginEmail == null)
				{
					ViewData["Error"] = "Login Again";
					return View();
				}
				EnrollCourse enrollCourse = new EnrollCourse()
				{
					CourseId = id,
					StudentEmail = LoginEmail
				};

				var response = await httpClient.PostAsJsonAsync("https://localhost:7003/api/Student/UnEnrollCourse", enrollCourse);
				return RedirectToAction("GetEnrollCourse");
			}
		}
	}
}
