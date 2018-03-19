using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenLadle.Data.Interfaces;
using GoldenLadle.Models;

namespace GoldenLadle.Data.Repos
{
    public class EntriesRepository : Repository<Entry>, IEntriesRepository
    {
        public EntriesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public ApplicationDbContext GetApplicationDbContext
        {
            get { return Context as ApplicationDbContext; }
        }

        public async Task<Entry> GetAsync(int? id)
        {
            return await Context.Set<Entry>()
                .Include(e => e.Event)
                .Include(e => e.User)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Entry>> GetAllAsync()
        {
            return await Context.Set<Entry>()
                .Include(e => e.Event)
                .Include(e => e.User)
                .ToListAsync();
        }

        public async void AddAsync(Entry entry)
        {
            var e = SetDates(entry, SaveType.Add);
            await Context.Set<Entry>().AddAsync(e);
        }

        public bool CheckIfAnyExist(int id)
        {
            return Context.Set<Entry>().Any(e => e.Id == id);
        }

        public Entry SetDates(Entry entry, SaveType saveType)
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
