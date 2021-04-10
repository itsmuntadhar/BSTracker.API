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

        public override IEnumerable<Bullshit> Get(int offset)
            => Context.GetDbSet<Bullshit>()
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Skip(offset)
            .Take(Limit)
            .AsEnumerable();

        public IEnumerable<Bullshit> Get(int offset, string whoSaidIt)
            => Context.GetDbSet<Bullshit>()
            .AsNoTracking()
            .Where(x => x.WhoSaidIt.ToLower().Contains(whoSaidIt))
            .OrderByDescending(x => x.CreatedAt)
            .Skip(offset)
            .Take(Limit)
            .AsEnumerable();
    }
}
