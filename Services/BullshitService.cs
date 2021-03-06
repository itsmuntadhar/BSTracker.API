using BSTracker.DTOs.Requests;
using BSTracker.Entities;
using BSTracker.Interfaces;
using BSTracker.Repositories;
using System.Collections.Generic;

namespace BSTracker.Services
{
    public class BullshitService : Service<Bullshit>, IBullshitService
    {
        public BullshitService(BullshitsRepository repo) : base(repo)
        {
        }

        public BullshitService(BullshitsRepository repo, bool explicitSaveChanges) : base(repo, explicitSaveChanges)
        {
        }

        public Bullshit Create(NewBullshit dto)
        {
            var bullshit = dto.GetBullshit();
            Add(bullshit);
            return bullshit;
        }

        public IEnumerable<Bullshit> Get(int offset, string whoSaidIt)
            => ((BullshitsRepository)Repo).Get(offset, whoSaidIt);

        public Dictionary<string, int> GetStats()
            => ((BullshitsRepository)Repo).GetStats();
    }
}
