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

        public IEnumerable<Bullshit> Get(int offset = 0, string whoSaidIt = "")
        {
            if (string.IsNullOrEmpty(whoSaidIt))
                return Repo.Get(offset);
            return ((BullshitsRepository)Repo).Get(offset, whoSaidIt);
        }
    }
}
