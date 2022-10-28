using HARS.Shared.DataBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Configuration
{
    public class ReadonlyQueryCollection<T> : IReadonlyQueryCollection<T>
    {
        public ReadonlyQueryCollection(ISession session)
        {
            _session = session;
            _session.DefaultReadOnly = true;
        }

        public IQueryable<T> Entities => _session.Query<T>();

        private readonly ISession _session;
    }
}
