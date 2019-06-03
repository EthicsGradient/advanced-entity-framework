using AdvancedEntityFramework.Shared;
using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AdvancedEntityFramework.Seeder
{
    internal class DatabaseSeeder
    {
        private readonly SchoolDbContext _schoolDbContext;

        private DatabaseSeeder(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        public static async Task<DatabaseSeeder> Create(string connectionString)
        {
            var options = new DbContextOptionsBuilder<SchoolDbContext>()
                  .UseSqlServer(connectionString)
                  .Options;
            var schoolDbContext = new SchoolDbContext(options);
            schoolDbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            schoolDbContext.ChangeTracker.LazyLoadingEnabled = false;
            schoolDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            await schoolDbContext.Database.EnsureDeletedAsync();
            await schoolDbContext.Database.EnsureCreatedAsync();

            return new DatabaseSeeder(schoolDbContext);
        }

        public async Task InsertStudentsAsync(int count)
        {
            const int batchSize = 1000;

            var studentFaker = new Faker<StudentEntity>()
                .RuleFor(x => x.FirstName, f => f.Name.FirstName())
                .RuleFor(x => x.LastName, f => f.Name.LastName())
                .RuleFor(x => x.DateOfBirth, f => f.Date.Between(DateTime.UtcNow.AddYears(-16), DateTime.UtcNow.AddYears(-11)));

            var insertedCount = 0;

            while (insertedCount < count)
            {
                var students = studentFaker.Generate(Math.Min(count - insertedCount, batchSize));

                await _schoolDbContext.Students.AddRangeAsync(students);
                await _schoolDbContext.SaveChangesAsync();

                insertedCount += batchSize;
            }
        }
    }
}
