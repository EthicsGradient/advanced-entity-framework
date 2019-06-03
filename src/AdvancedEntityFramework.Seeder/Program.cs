using System;
using System.Threading.Tasks;

namespace AdvancedEntityFramework.Seeder
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Schools;Integrated Security=SSPI;";
            var databaseSeeder = await DatabaseSeeder.Create(connectionString);
            await databaseSeeder.InsertStudentsAsync(100);

            Console.Write("Press any key...");
            Console.ReadKey();
        }

    }
}
