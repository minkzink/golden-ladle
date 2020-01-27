using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenLadle.Data.Interfaces;
using GoldenLadle.Models;
using Microsoft.EntityFrameworkCore;

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

        public override ICollection<Event> GetAll()
        {
            return Context.Set<Event>()
                .Include(m => m.FilePaths)
                .Include(m => m.Entries)
                .OrderByDescending(f => f.StartDT)
                .ToList();
        }

        public async Task<ICollection<Event>> GetAllAsync()
        {
            List<Event> @events = await Context.Set<Event>()
                .Include(e => e.FilePaths)
                .Include(e => e.Entries)
                .OrderByDescending(f => f.StartDT)
                .ToListAsync();
            return @events;
        }

        public async Task<ICollection<Event>> GetAllPast()
        {
            return await Context.Set<Event>()
                .Include(m => m.FilePaths)
                .Include(m => m.Entries)
                .Where(ev => ev.EndDT < DateTime.Now)
                .ToListAsync();
        }

        public async Task<ICollection<Event>> GetAllCurrent()
        {
            return await Context.Set<Event>()
                .Include(m => m.FilePaths)
                .Include(m => m.Entries)
                .Where(ev => ev.EndDT <= DateTime.UtcNow.AddDays(14))
                .OrderByDescending(ev => ev.StartDT)
                .ToListAsync();
        }

        public async Task AddAsync(Event @event)
        {
            var e = SetDates(@event, SaveType.Add);
            await Context.Set<Event>().AddAsync(e);
        }

        public bool CheckIfAnyExist(int id)
        {
            return Context.Set<Entry>().Any(e => e.Id == id);
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