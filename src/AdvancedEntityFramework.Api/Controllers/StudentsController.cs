using AdvancedEntityFramework.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvancedEntityFramework.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolDbContext _schoolDbContext;

        public StudentsController(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        public async Task<ActionResult<IReadOnlyCollection<StudentEntity>>> List()
        {
            var students = await _schoolDbContext.Students.ToListAsync();
             
            return students;
        }
    }
}
