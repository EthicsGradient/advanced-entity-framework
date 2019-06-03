using System;
using System.Threading.Tasks;

namespace AdvancedEntityFramework.Seeder
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string connectionString = @"Data Source=.;Initial Catalog=AdvancedEntityFrameworkApi;Integrated Security=SSPI;";
            
            var databaseSeeder = new DatabaseSeeder(connectionString);

            Console.Write("Setting up database... ");
            await databaseSeeder.DeleteAndCreateDatabaseAsync();
            Console.WriteLine("DONE");

            Console.Write("Seeding... ");
            await databaseSeeder.InsertStudentsAsync(100);
            Console.WriteLine("DONE");

            Console.WriteLine();
            Console.Write("Press any key...");
            Console.ReadKey();
        }

    }
}
