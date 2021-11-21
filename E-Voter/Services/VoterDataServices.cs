using E_Voter.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Voter.Services
{
    public static class VoterDataServices
    {
        public static void AddVoterDataDbContext(this IServiceCollection services)
        {
            services.AddDbContextFactory<ClientSideDBContext>(
                options => options.UseSqlite($"Filename={DataSynchronizer.SqliteDbFilename}")
                );
            services.AddScoped<DataSynchronizer>();
        }
    }
}
