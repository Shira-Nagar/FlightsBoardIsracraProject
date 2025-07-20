using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FlightsDl
{
    /// <summary>
    /// DB context factory.
    /// </summary>
    public class FlightDbContextFactory : IDesignTimeDbContextFactory<FlightDbContext>
    {
        /// <summary>
        /// Create DB context.
        /// </summary>
        /// <param name="args">Arguments for context creation (not used).</param>
        /// <returns>A new <see cref="FlightDbContext"/> instance.</returns>
        public FlightDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FlightDbContext>();
            optionsBuilder.UseSqlite("Data Source=flights.db"); // או מחרוזת החיבור שלך

            return new FlightDbContext(optionsBuilder.Options);
        }
    }
}