using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SportsSocialNetwork.DataBaseModels
{
    public class DataBaseContext : IdentityDbContext<ApplicationUser>
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Sport> Sports { get; set; }
        public DbSet<Playground> Playgrounds { get; set; }
        public DbSet<PlaygroundSportConnection> PlaygroundSportConnections { get; set; }
    }
}
