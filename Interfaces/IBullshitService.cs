using BSTracker.DTOs.Requests;
using BSTracker.Entities;
using System.Collections.Generic;

namespace BSTracker.Interfaces
{
    public interface IBullshitService : IService<Bullshit>
    {
        IEnumerable<Bullshit> Get(int offset = 0, string whoSaidIt = "");
        Bullshit Create(NewBullshit dto);
    }
}
