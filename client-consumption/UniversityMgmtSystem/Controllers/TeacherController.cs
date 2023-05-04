using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UniversityMgmtSystemClientConsuming.Models;

namespace UniversityMgmtSystemClientConsuming.Controllers
{
    public class TeacherController : Controller
    {
        private readonly static HttpClient _httpClient = new HttpClient();

        public async Task<IActionResult> Index()
        {
            List<Teacher> teachers = new List<Teacher>();
            using (HttpClient httpClient = new HttpClient())
            {

                var response = await httpClient.GetAsync("https://localhost:7003/api/Teacher/GetTeacher");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    teachers = JsonConvert.DeserializeObject<List<Teacher>>(content);
                    return View(teachers);
                }

            }
            return BadRequest();
        }
        [HttpGet]
        public IActionResult CreateTeacher()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacher(Teacher teacher)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7003/api/Teacher/CreateTeacher");
            var content = new StringContent(JsonConvert.SerializeObject(teacher));

            request.Content = content;
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                //var statuscode = await response.Content.ReadAsStringAsync();
                //var statusmessgae = JsonConvert.DeserializeObject<ApiStatus>(statuscode);
                //ViewData["Error"] = statusmessgae.Message;
                return View();
            }

            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> UpdateTeacher( int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7003/api/Teacher/GetTeacherById/{id}");
                if (response.IsSuccessStatusCode)
                {
                    Teacher teacher = new Teacher();
                    String content = await response.Content.ReadAsStringAsync();

                    teacher = JsonConvert.DeserializeObject<Teacher>(content);

                    return View(teacher);
                }


            }

            return BadRequest("No Teacher Found for id:" + id);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTeacher(Teacher teacher)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                ApiStatus apiStatus = new ApiStatus();
                var response = await httpClient.PostAsJsonAsync("https://localhost:7003/api/Teacher/UpdateTeacher", teacher);
                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    apiStatus = JsonConvert.DeserializeObject<ApiStatus>(content);
                    ViewData["Error"] = apiStatus.Message;
                    return View();




                }


                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteTeacher(int id)
        {

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7003/api/Teacher/GetTeacherById/" + id);
                if (response.IsSuccessStatusCode)
                {
                    Teacher teacher = new Teacher();
                    String content = await response.Content.ReadAsStringAsync();

                    teacher = JsonConvert.DeserializeObject<Teacher>(content);

                    return View(teacher);
                }


            }

            return BadRequest("No Teacher Found for id:" + id);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTeacher(Teacher teacher)
        {
            
            using (HttpClient httpClient = new HttpClient())
            {
				ApiStatus apiStatus = new ApiStatus();
				var response = await httpClient.DeleteAsync("https://localhost:7003/api/Teacher/DeleteTeacher/" + teacher.TeacherId);
                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    apiStatus = JsonConvert.DeserializeObject<ApiStatus>(content);
                    ViewData["Error"] = apiStatus.Message;
                    return View();


                }
				return RedirectToAction("Index");

			}


			

		}

    }
}
