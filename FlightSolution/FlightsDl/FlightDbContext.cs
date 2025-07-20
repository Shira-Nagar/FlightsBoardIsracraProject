using Microsoft.EntityFrameworkCore;
using FlightsEntity;

namespace FlightsDl
{
    /// Flights DB context.
  
    public class FlightDbContext : DbContext
    {
        /// Flights table.
       
        public DbSet<FlightEntity> Flights => Set<FlightEntity>();
        
        public DbSet<UserLogIn> UserLogIns => Set<UserLogIn>();
        public DbSet<UserFlight> UserFlights => Set<UserFlight>();



        /// <summary>
        /// Init context.
        /// </summary>
        public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options)
        {
        }
        /// <summary>
        /// Configure model.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserFlight>()
     .HasKey(uf => new { uf.UserLogInId, uf.FlightEntityId }); 

            modelBuilder.Entity<UserFlight>()
                .HasOne(uf => uf.UserLogIn)
                .WithMany(u => u.UserFlights)
                .HasForeignKey(uf => uf.UserLogInId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserFlight>()
                .HasOne(uf => uf.FlightEntity)
                .WithMany(f => f.UserFlights)
                .HasForeignKey(uf => uf.FlightEntityId)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
