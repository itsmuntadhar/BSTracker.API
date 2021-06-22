using BSTracker.Entities;
using BSTracker.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BSTracker.Repositories
{
    public class BullshitsRepository : BaseRepository<Bullshit>
    {
        public BullshitsRepository(IDbContext context) : base(context)
        {
        }

        public IEnumerable<Bullshit> Get(int offset, string whoSaidIt)
        {
            var query = Context.GetDbSet<Bullshit>()
                .AsQueryable();
            if (string.IsNullOrEmpty(whoSaidIt) == false)
                query = query.Where(x => x.WhoSaidIt.ToLower().Contains(whoSaidIt));
            return query
                .OrderByDescending(x => x.CreatedAt)
                .Skip(offset)
                .Take(25)
                .AsEnumerable();
        }

        public Dictionary<string, int> GetStats()
            => Context.GetDbSet<Bullshit>()
            .Select(x => new { x.WhoSaidIt, x.Id })
            .GroupBy(x => x.WhoSaidIt, (key, group) => new { key, count = group.Count() })
            .AsEnumerable()
            .OrderByDescending(x => x.count)
            .ToDictionary(x => x.key, x => x.count);
    }
}
