using AdvancedEntityFramework.Shared;
using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AdvancedEntityFramework.Console
{
    public class EntityFrameworkRepository
    { 
        public void FirstOrDefault()
        {
            using (var dbContext = GenerateDbContext())
            {
                dbContext.Students.FirstOrDefault(s => s.StudentId == 1);
            }
        }

        public void FirstOrDefaultAsNoTracking()
        {
            using (var dbContext = GenerateDbContext())
            {
                dbContext.Students.AsNoTracking()
                    .FirstOrDefault(s => s.StudentId == 1);
            }
        }

        public void SingleOrDefault()
        {
            using (var dbContext = GenerateDbContext())
            {
                dbContext.Students.SingleOrDefault(s => s.StudentId == 1);
            }
        }

        public void SingleOrDefaultAsNoTracking()
        {
            using (var dbContext = GenerateDbContext())
            {
                dbContext.Students.AsNoTracking()
                    .SingleOrDefault(s => s.StudentId == 1);
            }
        }

        public void InsertStudents(int count)
        {
            using (var dbContext = GenerateDbContext())
            {
                const int batchSize = 1000;

                var studentFaker = new Faker<StudentEntity>()
                    .RuleFor(x => x.FirstName, f => f.Name.FirstName())
                    .RuleFor(x => x.LastName, f => f.Name.LastName())
                    .RuleFor(x => x.DateOfBirth, f => f.Date.Between(DateTime.UtcNow.AddYears(-21), DateTime.UtcNow.AddYears(-18)));

                var insertedCount = 0;

                while (insertedCount < count)
                {
                    var students = studentFaker.Generate(Math.Min(count - insertedCount, batchSize));

                    dbContext.Students.AddRange(students);
                    dbContext.SaveChanges();

                    insertedCount += batchSize;
                }
            }
        }

        public static SchoolDbContext GenerateDbContext()
        {
            var options = new DbContextOptionsBuilder<SchoolDbContext>()
                .UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog=Schools;Integrated Security=SSPI;")
                .Options;

            return new SchoolDbContext(options);
        }
    }
}
