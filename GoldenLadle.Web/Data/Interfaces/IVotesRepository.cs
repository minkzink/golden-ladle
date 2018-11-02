using System.Collections.Generic;
using System.Threading.Tasks;
using GoldenLadle.Models;

namespace GoldenLadle.Data.Interfaces
{
    public interface IVotesRepository : IRepository<Vote>
    {
        Task<IEnumerable<Vote>> GetVotesPerUserEvent(int eventId);
        Task<Vote> GetAsync(int voteId);
        Vote SetDates(Vote vote, SaveType saveType);
        void Add(Vote vote);
        void Remove(Vote vote);
        Task<int> GetEntryVoteCount(int entryId);
        bool CheckIfAnyExist(int? voteId = null, string userId = "", int? entryId = null, int? eventId = null);
        Task<Vote> GetAsync(string userId, int? entryId);
    }
}
