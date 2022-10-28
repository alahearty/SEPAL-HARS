using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Configuration.SessionFactory
{
    public interface INhibernateSessionFactory
    {
        /// <summary>
        /// This will be return a singleton throughout the IOC Container
        /// The ISessionFactory will be constructor once. However, multiple sessions will be created per request.
        /// </summary>
        /// <returns></returns>
        ISession GetFreshSession();
    }
}
