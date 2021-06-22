using BSTracker.Entities;
using BSTracker.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BSTracker.Data
{
    public class PostgresDbContext : DbContext, IDbContext
    {
        public DbSet<Bullshit> Bullshits { get; set; }

        public PostgresDbContext()
        {

        }

        public PostgresDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<T> GetDbSet<T>() where T : Entity
            => Set<T>();
    }
}
