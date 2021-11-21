using CsvHelper;
using System.Globalization;

namespace EVoterAPI.Context
{
    public static class SeedData
    {
        public static void EnsureSeeded(IServiceProvider services)
        {
            var scopeFactory = services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<EVoterContext>();
            if (!db.Users.Any())
            {
                var dir = Path.GetDirectoryName(typeof(SeedData).Assembly.Location)!;
                // seed users
                using var fileReader = (TextReader)File.OpenText(Path.Combine(dir, "Context", "VoterUsers.csv"));
                using var csv = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                var count = 0;
                while (csv.Read())
                {
                    count++;

                    db.Users.Add(new E_VoterApi.Models.User
                    {
                        userID = csv.GetField<Guid>(0),
                        email = csv.GetField<string>(1),
                        firstName = csv.GetField<string>(2),
                        lastName = csv.GetField<string>(3),
                        contactNo = csv.GetField<string>(4),
                        modifiedTicks = count
                    });

                    if(count % 1000 == 0)
                    {
                        Console.WriteLine($"Seeded item {count}...");
                    }
                }

                db.SaveChanges();
            }
        }
    }
}
