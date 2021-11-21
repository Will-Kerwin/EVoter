using Microsoft.EntityFrameworkCore;

namespace E_Voter.Data
{
    public class ClientSideDBContext : DbContext
{
        public DbSet<ElectionModel> Elections { get; set; } = default!;

        public ClientSideDBContext(DbContextOptions<ClientSideDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ElectionModel>().HasIndex(x => x.electionID);
            modelBuilder.Entity<ElectionModel>().HasIndex(x => x.electionName);
            modelBuilder.Entity<ElectionModel>().HasIndex(x => x.winnerID);
            modelBuilder.Entity<ElectionModel>().Property(x => x.electionName).UseCollation("nocase");
        }
}
}
