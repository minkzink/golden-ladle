using System;
using System.Collections.Generic;
using System.Linq;
using GoldenLadle.Models;
using Microsoft.EntityFrameworkCore;
using GoldenLadle.Data.Interfaces;
using System.Threading.Tasks;

namespace GoldenLadle.Data.Repos
{
    public class EventsRepository : Repository<Event>, IEventsRepository
    {
        public EventsRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext GetApplicationDbContext
        {
            get { return Context as ApplicationDbContext; }
        }

        public async Task<Event> GetAsync(int? id)
        {
            return await Context.Set<Event>()
                .Include(m => m.FilePaths)
                .Include(m => m.Entries)
                    .ThenInclude(e => e.Votes)
                .SingleOrDefaultAsync(m => m.Id == id);
        }
               
        public override IEnumerable<Event> GetAll()
        {
            return Context.Set<Event>()
                .Include(m => m.FilePaths)
                .ToList();
        }
        
        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await Context.Set<Event>()
                .Include(e => e.FilePaths)
                .Include(e => e.Entries)
                .OrderByDescending(f => f.StartDT)
                .ToListAsync();
        }

        public async void AddAsync(Event @event)
        {
            var e = SetDates(@event, SaveType.Add);
            await Context.Set<Event>().AddAsync(e);
        }

        public bool CheckIfAnyExist(int id)
        {
            return Context.Set<Event>().Any(e => e.Id == id);
        }

        public Event SetDates(Event entry, SaveType saveType)
        {
            switch (saveType)
            {
                case (SaveType.Add):
                    entry.CreateDate = DateTime.Now;
                    entry.ModifiedDate = DateTime.Now;
                    entry.DeletedDate = DateTime.Now;
                    break;
                case (SaveType.Update):
                    entry.ModifiedDate = DateTime.Now;
                    break;
                case (SaveType.Delete):
                    entry.DeletedDate = DateTime.Now;
                    break;
            }
            return entry;
        }
    }
}