using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenLadle.Data.Interfaces;
using GoldenLadle.Data.Repos;

namespace GoldenLadle.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IEventsRepository Events { get; private set; }
        public IEntriesRepository Entries { get; private set; }
        public IVotesRepository Votes { get; private set; }
        
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Events = new EventsRepository(_context);
            Entries = new EntriesRepository(_context);
            Votes = new VotesRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
