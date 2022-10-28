using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Configuration.DatabasePipeline
{
    public class Repository
    {
        public Repository(ISession session)
        {
            Session = session;
        }

        public ISession Session { get; }
    }
}
