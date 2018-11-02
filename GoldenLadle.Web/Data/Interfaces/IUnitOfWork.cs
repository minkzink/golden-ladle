using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenLadle.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IEventsRepository Events { get; }
        IEntriesRepository Entries { get; }
        IVotesRepository Votes { get; }
        int Complete();
        Task<int> CompleteAsync();
    }
}
