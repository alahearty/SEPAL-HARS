using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Dapper
{
    public class AdminDbContext : DatabasePipeline<AdminContext>
    {
        public AdminDbContext(AdminContext store, AccessManager accessManager, ISession session, ILogger logger) : base(session)
        {
            Store = store;
            Logger = logger;
            Store.AccessManager = accessManager;
            Pipeline = "Admin DB";
        }

        public const string SCHEMA = "administration";
    }
}
