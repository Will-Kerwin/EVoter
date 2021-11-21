using E_VoterApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EVoterAPI.Context
{
    public class EVoterContext : DbContext
    {
        public EVoterContext(DbContextOptions options) 
            : base(options)
        {

        }

        public DbSet<Election> Elections { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<ElectionResult> Results { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Nominee> Nominees { get; set; }
    }
}
