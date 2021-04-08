using BSTracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace BSTracker.Interfaces
{
    public interface IDbContext
    {
        DbSet<Bullshit> Bullshits { get; set; }
        Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade Database { get; }
        DbSet<T> GetDbSet<T>() where T : Entity;
        int SaveChanges();
    }
}
