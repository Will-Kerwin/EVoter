using E_Voter.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Voter.Services
{
    public static class ElectionDataService
{
        public static void AddElectionDataDbContext(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContextFactory<ClientSideDbContext>(
                options => options.UseSqlite($"Filename={DataSynchroniser.SqliteDbFilename}")
                );
            serviceCollection.AddScoped<DataSynchroniser>();

        }
}
}
