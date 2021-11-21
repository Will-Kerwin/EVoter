using E_Voter.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Voter.Data
{
    public class ClientSideDbContext : DbContext
{
        public DbSet<User> Users { get; set; } = default!;

        public ClientSideDbContext(DbContextOptions<ClientSideDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(x => x.modifiedTicks);
            modelBuilder.Entity<User>().HasIndex(x => x.userID);
            modelBuilder.Entity<User>().HasIndex(x => x.email);
            modelBuilder.Entity<User>().HasIndex(x => x.lastName);
            modelBuilder.Entity<User>().Property(x => x.lastName).UseCollation("nocase");
        }
}
}
