using System.Collections.Generic;
using System.Threading.Tasks;
using GoldenLadle.Models;

namespace GoldenLadle.Data.Interfaces
{
    public interface IEventsRepository : IRepository<Event>
    {
        Task<Event> GetAsync(int? id);
        Task<IEnumerable<Event>> GetAllAsync();
        Event SetDates(Event @event, SaveType saveType);
        Task AddAsync(Event @event);
        void Update(Event @event);
        void Remove(Event @event);
        bool CheckIfAnyExist(int id);
    }
}
