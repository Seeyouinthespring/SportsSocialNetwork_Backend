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
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentVisiting> AppointmentVisitings { get; set; }
        public DbSet<RentRequest> RentRequests { get; set; }
        public DbSet<ConfirmedRent> ConfirmedRents { get; set; }
        public DbSet<ContactInformation> ContactInformation { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<PersonalActivity> PersonalActivities { get; set; }

    }
}
