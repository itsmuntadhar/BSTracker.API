using BSTracker.DTOs.Requests;
using BSTracker.Entities;
using System.Collections.Generic;

namespace BSTracker.Interfaces
{
    public interface IBullshitService : IService<Bullshit>
    {
        IEnumerable<Bullshit> Get(int offset, string whoSaidIt);
        Bullshit Create(NewBullshit dto);
        Dictionary<string, int> GetStats();
    }
}
