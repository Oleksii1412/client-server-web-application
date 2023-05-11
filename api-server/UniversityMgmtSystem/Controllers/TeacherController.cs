using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityMgmtSystem.Data;
using UniversityMgmtSystemServerApi.Models;

namespace UniversityMgmtSystemServerApi.Controllers
{
	[Route("api/[controller]")]
	public class TeacherController : Controller
	{

		AppDbContext _db;
		public TeacherController( AppDbContext appDbContext)
		{
			_db = appDbContext;
		}


		
		[HttpGet]
		[Route("GetTeacher")]
		public async Task<List<Teacher>> GetTeachers()
		{
			return await _db.Teachers.ToListAsync();
		}

		[HttpPost]
		[Route("CreateTeacher")]
		public async Task<IActionResult> CreateTeacher([FromBody] Teacher teacher)
		{

			if(teacher == null)
			{
				return StatusCode( StatusCodes.Status404NotFound,
					new Response
					{
						Status="Error",
						Message="Teacher is null"
					}
					);

			}

			await _db.Teachers.AddAsync(teacher);
			await _db.SaveChangesAsync();
			return StatusCode(StatusCodes.Status200OK);
		}
		[HttpPost]
		[Route("UpdateTeacher")]
		public async Task<IActionResult> UpdateTeacher([FromBody] Teacher teacher)
		{

			var editTeacher = await _db.Teachers.Where(t => t.TeacherId == teacher.TeacherId).FirstOrDefaultAsync();
			if(editTeacher == null)
			{
				return StatusCode(StatusCodes.Status404NotFound,
					new Response
					{
						Status="Error",
						Message="No Teacher found similiar"
					});
			}

			 editTeacher.TeacherName = teacher.TeacherName;
			await _db.SaveChangesAsync();

			return StatusCode(StatusCodes.Status200OK);
		

		}
		[HttpGet]
		[Route("GetTeacherById/{id}")]
		public async Task<Teacher> GetTeacherById(int id)
		{
			Teacher teacher = _db.Teachers.FirstOrDefault(c => c.TeacherId == id);


			return teacher;

		}


		[HttpDelete]
		[Route("DeleteTeacher/{id}")]
		public async Task<IActionResult> DeleteTeacher(int id)
		{
			var deleteTeacher = await _db.Teachers.Where(c => c.TeacherId == id).FirstOrDefaultAsync();
			if (deleteTeacher == null)
			{

				return StatusCode(StatusCodes.Status404NotFound,
					new Response
					{
						Status = "Error",
						Message = "No Teacher fount at id: " + id

					});


			}
			_db.Teachers.Remove(deleteTeacher);
			await _db.SaveChangesAsync();



			return StatusCode(StatusCodes.Status200OK);
		}


	}
}
