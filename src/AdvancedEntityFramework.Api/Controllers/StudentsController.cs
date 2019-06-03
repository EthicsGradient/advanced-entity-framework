using System;
using System.Linq;
using AdvancedEntityFramework.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AdvancedEntityFramework.Api.Extensions;
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
            var entity =  await _schoolDbContext.Students
                .Where(x => x.StudentId == studentId)
                .SingleOrDefaultAsync();

            return new FindStudentResponse(entity.StudentId, entity.FirstName, entity.LastName,
                entity.DateOfBirth);
        }

        [HttpGet]
        public async Task<ActionResult<ListStudentsResponse>> List()
        {
            var entities = await _schoolDbContext.Students
                .ToListAsync();
            
            var students = entities.Select(x => new Student(x.StudentId, x.FirstName, x.LastName,
                    x.DateOfBirth))
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
