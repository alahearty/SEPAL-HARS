using HARS.Shared.Architecture;
using HARS.Shared.Repository;
using HARS.Shared.Utilities;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Configuration.DatabasePipeline
{
    public class NamedRepository<T> : Repository, INamedRepository<T> where T : IEntity
    {
        public NamedRepository(ISession session) : base(session) { }
        public void Update(T entity)
        {
            Session.SaveOrUpdate(entity);
        }

        public void Remove(T entity)
        {
            Session.Delete(entity);
        }

        public async Task<ActionResult<T>> FetchByIdAsync(Guid id)
        {
            var entity = Session.Query<T>().FirstOrDefault(entity => entity.Id == id);

            if (entity == null)
                return ActionResult<T>.Failed().AddError($"No Entity found with Id {id}");

            return await Task.FromResult(ActionResult<T>.Success(entity));
        }

        public async Task<IEnumerable<T>> FetchAllAsync()
        {
            return await Session.Query<T>().ToListAsync();
        }

        public void Add(T entity)
        {
            Session.Save(entity);
        }
    }
}
