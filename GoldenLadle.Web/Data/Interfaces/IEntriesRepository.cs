using System.Collections.Generic;
using System.Threading.Tasks;
using GoldenLadle.Models;

namespace GoldenLadle.Data.Interfaces
{
    public interface IEntriesRepository : IRepository<Entry>
    {
        Task<Entry> GetAsync(int? id);
        Task<IEnumerable<Entry>> GetAllAsync();
        Entry SetDates(Entry entry, SaveType saveType);
        void AddAsync(Entry entry);
        void Update(Entry entry);
        void Remove(Entry entry);
        bool CheckIfAnyExist(int id);
    }
}
