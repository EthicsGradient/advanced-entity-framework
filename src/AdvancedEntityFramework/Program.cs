using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AdvancedEntityFramework
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("- Default -");
            await InsertStudents(count: 10000, false);
            await FirstOrDefault(false);
            await SingleOrDefault(false);

            Console.WriteLine();

            Console.WriteLine("- Optimised -");
            await InsertStudents(count: 10000, true);
            await FirstOrDefault(true);
            await SingleOrDefault(true);

            Console.ReadLine();
        }

        public static async Task FirstOrDefault(bool isOptimised)
        {
            using (var dbContext = await GenerateDbContext(isOptimised))
            {
                var stopwatch = Stopwatch.StartNew();

                await dbContext.Students.FirstOrDefaultAsync(s => s.StudentId == 1);

                stopwatch.Stop();

                Console.WriteLine($"FirstOrDefault: {stopwatch.ElapsedMilliseconds}ms");
            }
        }

        public static async Task SingleOrDefault(bool isOptimised)
        {
            using (var dbContext = await GenerateDbContext(isOptimised))
            {
                var stopwatch = Stopwatch.StartNew();

                await dbContext.Students.SingleOrDefaultAsync(s => s.StudentId == 1);

                stopwatch.Stop();

                Console.WriteLine($"SingleOrDefault: {stopwatch.ElapsedMilliseconds}ms");
            }
        }

        private static async Task InsertStudents(int count, bool isOptimised)
        {
            using (var dbContext = await GenerateDbContext(isOptimised))
            {
                const int batchSize = 1000;

                var studentFaker = new Faker<StudentEntity>()
                    .RuleFor(x => x.FirstName, f => f.Name.FirstName())
                    .RuleFor(x => x.LastName, f => f.Name.LastName())
                    .RuleFor(x => x.DateOfBirth, f => f.Date.Between(DateTime.UtcNow.AddYears(-21), DateTime.UtcNow.AddYears(-18)));

                var insertedCount = 0;

                var stopwatch = Stopwatch.StartNew();

                while (insertedCount < count)
                {
                    var students = studentFaker.Generate(Math.Min(count - insertedCount, batchSize));

                    await dbContext.Students.AddRangeAsync(students);
                    await dbContext.SaveChangesAsync();

                    insertedCount += batchSize;
                }

                stopwatch.Stop();

                Console.WriteLine($"InsertStudents ({count}): {stopwatch.ElapsedMilliseconds}ms");
            }
        }

        private static async Task<SchoolDbContext> GenerateDbContext(bool isOptimised)
        {
            var options = new DbContextOptionsBuilder<SchoolDbContext>()
                .UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog=Schools;Integrated Security=SSPI;")
                .Options;
            var dbContext = new SchoolDbContext(options);

            await dbContext.Database.EnsureCreatedAsync();

            if (isOptimised)
            {
                dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                dbContext.ChangeTracker.LazyLoadingEnabled = false;
                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }

            return dbContext;
        }
    }
}
