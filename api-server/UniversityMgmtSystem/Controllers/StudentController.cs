using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using UniversityMgmtSystem.Data;
using UniversityMgmtSystemServerApi.Models;

namespace UniversityMgmtSystemServerApi.Controllers
{
	[Route("api/[controller]")]
	public class StudentController : Controller
	{
		protected AppDbContext _db;
		public StudentController(AppDbContext dbContext) 
		{ 
		_db = dbContext;
		
		}
		[HttpGet]
		[Route("GetProfile/{StudentEmail}")]
		public async Task<Student> GetProfile(string StudentEmail)
		{
			Student student= await _db.Students.Where(s=>s.Email == StudentEmail).FirstOrDefaultAsync();

			if(student==null)
			{
				_db.Students.Add(new Student()
				{
					StudentName = StudentEmail,
					Email = StudentEmail
				});
				await _db.SaveChangesAsync();
				Student newstudent = await _db.Students.Where(s => s.Email == StudentEmail).FirstOrDefaultAsync();
				return newstudent;
			}
			return student;
		}
		[HttpPost]
		[Route("EditProfile")]
		public async Task<IActionResult>EditProfile([FromBody]Student student)
		{
		   var editStudent	= await _db.Students.Where(s=>s.StudentId==student.StudentId).FirstOrDefaultAsync();
			if(editStudent==null) 
			{
				return StatusCode(StatusCodes.Status204NoContent, new Response
				{
					Status = "Error",
					Message = "No Student Found With Similar Data"
				});
			}
			editStudent.StudentName= student.StudentName;
			editStudent.DateOfBirth= student.DateOfBirth;
			await _db.SaveChangesAsync();

			return StatusCode(StatusCodes.Status200OK);
		}


		[HttpPost]
		[Route("EnrollCourse")]
		public async Task<IActionResult> EnrollCourse([FromBody]EnrollCourse enrollCourse)
		{
			if(enrollCourse.StudentEmail==null)
			{
				return BadRequest("No Data Contents Form Server Side");
			}
			Student student= _db.Students.Where(s=>s.Email==enrollCourse.StudentEmail).FirstOrDefault();
			if(student==null)
			{
				_db.Students.Add(new Student()
				{
					StudentName = enrollCourse.StudentEmail,
					Email = enrollCourse.StudentEmail
				});
				 await _db.SaveChangesAsync();
				Student newstudent = await _db.Students.Where(s => s.Email == enrollCourse.StudentEmail)
					.FirstOrDefaultAsync();
				StudentCourse studentCourse= new StudentCourse()
				{
					StudentCourseId=newstudent.StudentId,
					CouserId=enrollCourse.CourseId
				};
				await _db.StudentCourses.AddAsync(studentCourse);
				await _db.SaveChangesAsync();
				return StatusCode(StatusCodes.Status200OK);

			}
			else
			{
				StudentCourse studentCourse = new StudentCourse()
				{
					StudentId = student.StudentId,
					CouserId = enrollCourse.CourseId
				};
				await _db.StudentCourses.AddAsync(studentCourse);
				await _db.SaveChangesAsync();
				return StatusCode(StatusCodes.Status200OK);

			}
	
		}
		[HttpGet]
		[Route("GetEnrollCourse/{StudentEmail}")]
		public async Task<List<Course>> GetEnrolledCourse( string StudentEmail)
		{
			if (StudentEmail == null)
			{
				return null;
			}
			List<Course> enrollCourses = new List<Course>();
			Student students = await _db.Students.FirstOrDefaultAsync(s => s.Email == StudentEmail);
			if (students == null)
			{
				_db.Students.Add(new Student
				{
					StudentName = StudentEmail,
					Email = StudentEmail
				});
				students = await _db.Students.FirstOrDefaultAsync(s => s.Email == StudentEmail);
			}
			List<StudentCourse> studentCourses = await _db.StudentCourses.
			Where(sc => sc.StudentId == students.StudentId).Include(c => c.Course).ToListAsync();
			studentCourses.ForEach(sc => enrollCourses.Add(sc.Course));
			return enrollCourses;

		}
		[HttpPost]
		[Route("UnEnrollCourse")]
		public async Task<IActionResult> UnEnrollCourse([FromBody]EnrollCourse enrollcourse)
		{
			if(enrollcourse.StudentEmail==null && enrollcourse.CourseId==null)
			{
				return BadRequest("No Data Found From Client Side");
			}
			Student student= await _db.Students.Where(s => s.Email == enrollcourse.StudentEmail).FirstOrDefaultAsync();
			if(student == null)
			{
				return StatusCode(StatusCodes.Status404NotFound,new Response
				{
					Status="Error",
					Message="Student Found At Given Email"
				});
			}
			StudentCourse studentCourses = await _db.StudentCourses.Where(sc => sc.StudentCourseId == student.StudentId
			&& sc.CouserId == enrollcourse.CourseId).FirstOrDefaultAsync();
			_db.StudentCourses.Remove(studentCourses);
			await _db.SaveChangesAsync();
			return StatusCode(StatusCodes.Status200OK);
			

		}


	}
}
