using System.Collections.Generic;

namespace GoldenLadle.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        ICollection<TEntity> GetAll();
    }
}