using System;
using System.Linq;
using AdvancedEntityFramework.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AdvancedEntityFramework.Api.Models;
using AdvancedEntityFramework.Api.Models.CreateStudent;
using AdvancedEntityFramework.Api.Models.FindStudent;
using AdvancedEntityFramework.Api.Models.ListStudents;
using AdvancedEntityFramework.Api.Models.UpdateStudent;
using AdvancedEntityFramework.Shared.Entities.Students;

namespace AdvancedEntityFramework.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StudentsController : Controller
    {
        private readonly SchoolDbContext _schoolDbContext;

        public StudentsController(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateStudentRequest request)
        {
            var student = new StudentEntity
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth.Value
            };

            _schoolDbContext.Students.Add(student);
            await _schoolDbContext.SaveChangesAsync();

            return null;
        }

        [HttpGet]
        public async Task<ActionResult<FindStudentResponse>> Find(Guid studentId)
        {
            var student = await _schoolDbContext.Students
                .Where(x => x.StudentId == studentId)
                .Select(x => new
                {
                    x.StudentId,
                    x.FirstName,
                    x.LastName,
                    x.DateOfBirth
                })
                .SingleOrDefaultAsync();

            return new FindStudentResponse(student.StudentId, student.FirstName, student.LastName,
                student.DateOfBirth);
        }

        [HttpGet]
        public async Task<ActionResult<ListStudentsResponse>> List()
        {
            var students = (await _schoolDbContext.Students
                    .ToListAsync())
                .Select(x => new Student(x.StudentId, x.FirstName, x.LastName, x.DateOfBirth))
                .ToList();
            
            return new ListStudentsResponse(students);
        }

        [HttpPost]
        public async Task<ActionResult> Update(UpdateStudentRequest request)
        {
            var student = new StudentEntity
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth.Value
            };

            _schoolDbContext.Students.Update(student);
            await _schoolDbContext.SaveChangesAsync();

            return null;
        }
    }
}
