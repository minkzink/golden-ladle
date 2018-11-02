using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenLadle.Data.Interfaces;
using GoldenLadle.Models;

namespace GoldenLadle.Data.Repos
{
    public class VotesRepository : Repository<Vote>, IVotesRepository
    {
        public VotesRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext GetApplicationDbContext
        {
            get { return Context as ApplicationDbContext; }
        }

        public async Task<Vote> GetAsync(int voteId)
        {
            return await Context.Set<Vote>()
                .SingleOrDefaultAsync(m => m.Id == voteId);
        }

        public async Task<Vote> GetAsync(string userId, int? entryId)
        {
            return await Context.Set<Vote>()
                .FirstOrDefaultAsync(v => v.UserId == userId && v.EntryId == entryId);
        }

        public async Task<IEnumerable<Vote>> GetVotesPerUserEvent(int eventId)
        {
            return await Context.Set<Vote>()
                .Where(v => v.EventId == eventId)
                .ToListAsync();
        }

        public override void Add(Vote vote)
        {
            if (!CheckIfAnyExist(null, vote.UserId, vote.EntryId, vote.EventId))
            {
                var v = SetDates(vote, SaveType.Add);
                Context.Set<Vote>().Add(vote);
            }
        }

        public async Task<int> GetEntryVoteCount(int entryId)
        {
            return await Context.Set<Vote>()
                          .Where(v => v.EntryId == entryId)
                          .SumAsync(v => v.Value);
        }
        
        public bool CheckIfAnyExist(int? voteId = null, string userId = "", int? entryId = null, int? eventId = null)
        {
            if (voteId.HasValue)
            {
                return Context.Set<Vote>().Any(v => v.Id == voteId);
            }
            else
            {
                return Context.Set<Vote>().Any(v => v.EntryId == entryId && v.UserId == userId && v.EventId == eventId);
            }
        }

        public Vote SetDates(Vote vote, SaveType saveType)
        {
            switch (saveType)
            {
                case (SaveType.Add):
                    vote.CreateDate = DateTime.Now;
                    vote.ModifiedDate = DateTime.Now;
                    break;
                case (SaveType.Update):
                    break;
                case (SaveType.Delete):
                    break;
            }
            return vote;
        }
    }
}
