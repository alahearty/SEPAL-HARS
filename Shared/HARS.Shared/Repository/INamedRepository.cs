using HARS.Shared.Architecture;
using HARS.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Repository
{
    public interface INamedRepository<T> where T : IEntity
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<ActionResult<T>> FetchByIdAsync(Guid id);
        Task<IEnumerable<T>> FetchAllAsync();
    }
}
