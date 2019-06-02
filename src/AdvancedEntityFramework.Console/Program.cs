using BenchmarkDotNet.Running;
using SystemConsole = System.Console;

namespace AdvancedEntityFramework.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = EntityFrameworkRepository.GenerateDbContext();
            dbContext.Database.EnsureCreated();

            BenchmarkRunner.Run(typeof(FirstOrDefaultVsSingleOrDefault));

            SystemConsole.Read();
        }
    }
}
