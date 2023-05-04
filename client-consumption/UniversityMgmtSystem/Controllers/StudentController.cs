using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UniversityMgmtSystemClientConsuming.Models.ControllerModel;
using UniversityMgmtSystemClientConsuming.ViewModels;

namespace UniversityMgmtSystemClientConsuming.Controllers
{
	public class StudentController : Controller
	{
		LoginVM LoginUser;
		public IActionResult DashBoard(LoginVM user)
		{
			LoginUser = user;
			return View(user);
		}

		[HttpGet]
		public async Task<IActionResult> GetProfile(string StudentEmail)
		{
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
		[HttpPost]
		public async Task<IActionResult> EnrollCourse (int CourseId)
		{
			if(LoginUser.Email==null)
			{
				ViewData["Error"] = "Login Again";
				return View();
			}
			EnrollCourse enrollCourse = new EnrollCourse()
			{
				CourseId = CourseId,
				StudentEmail = LoginUser.Email
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
				return View("GetEnrollCourse");
			
			}
		}
		[HttpGet]
		public  async Task<IActionResult> GetEnrollCourse(string StudentEmail)
		{
			using (HttpClient httpClient = new HttpClient())
			{

				List<Course> courses = new List<Course>();
				var response = await httpClient.GetAsync("https://localhost:7003/api/Student/GetEnrollCourse/"+
					StudentEmail);
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
	}
}
