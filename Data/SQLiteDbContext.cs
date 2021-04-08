using System;
using System.Linq;
using BSTracker.Entities;
using BSTracker.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BSTracker.Data
{
    public class SQLiteDbContext : DbContext, IDbContext
    {
        public SQLiteDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Bullshit> Bullshits { get; set; }

        public DbSet<T> GetDbSet<T>() where T : Entity
            => Set<T>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset) || p.PropertyType == typeof(DateTimeOffset?));
                foreach (var property in properties)
                {
                    modelBuilder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(new DateTimeOffsetToBinaryConverter());
                }
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
