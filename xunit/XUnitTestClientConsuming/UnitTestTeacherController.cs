
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using UniversityMgmtSystem.Data;
using UniversityMgmtSystemServerApi.Controllers;
using Assert = NUnit.Framework.Assert;

namespace XUnitTestClientConsuming
{
	[TestFixture]
	public class UnitTestTeacherController
	{
		AppDbContext _db;
		public UnitTestTeacherController(AppDbContext appDbContext)
		{
			_db = appDbContext;
		}

		

		//[SetUp]
		//public void Setup()
		//{
		//	_mockDbContext = new Mock<AppDbContext>();
		//	_teacherController = new TeacherController(_mockDbContext.Object);
		//}

		private Mock<AppDbContext> _mockDbContext;
		private TeacherController _teacherController;

		[SetUp]
		public void Setup()
		{
			// Create a mock DbContext for testing
			_mockDbContext = new Mock<AppDbContext>();
			_teacherController = new TeacherController(_mockDbContext.Object);
		}

		[Test]
		public async Task GetTeachers_ReturnsListOfTeachers()
		{
			// Arrange
			var expectedTeachers = new List<Teacher>
			{
				new Teacher { TeacherId = 1, TeacherName = "John Doe" },
				new Teacher { TeacherId = 2, TeacherName = "Jane Smith" }
			};
			_mockDbContext.Setup(db => db.Teachers.FindAsync(It.IsAny<int>())).Returns(() => Task.FromResult(expectedTeachers));
			_db.Teachers.FindAsync(It.IsAny<int>());


			// Act
			var result = await _teacherController.GetTeachers();

			// Assert
			Assert.IsInstanceOf<List<Teacher>>(result);
			Assert.AreEqual(expectedTeachers.Count, result.Count);
			
		}

		
		[Test]
		public async Task CreateTeacher_ReturnsOk_WhenTeacherIsNotNull()
		{
            // Arrange
            var expectedTeachers = new List<Teacher>
            {
                new Teacher { TeacherId = 1, TeacherName = "John Doe" },
                new Teacher { TeacherId = 2, TeacherName = "Jane Smith" }
            };
            var teacher = new UniversityMgmtSystemServerApi.Models.Teacher();

			

			// Act
			var result = await _teacherController.CreateTeacher(teacher);

			// Assert
		
			var statusCodeResult = (StatusCodeResult)result;
			Assert.Equals(StatusCodes.Status200OK, statusCodeResult.StatusCode);

			_mockDbContext.Verify();
		}
		


	}
}
