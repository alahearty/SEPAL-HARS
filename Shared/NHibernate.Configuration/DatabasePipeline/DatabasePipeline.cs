using HARS.Shared.DataBases;
using HARS.Shared.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Configuration.DatabasePipeline
{
    public class DatabasePipeline<T> : CommandAccessor<T>
    {
        public DatabasePipeline(ISession session)
        {
            Session = session;
        }

        public override async Task<ActionResult> CommitToDatabaseAsync()
        {
            Logger?.LogInformation($"Comitting changes in {Pipeline}");
            using (var transaction = Session.BeginTransaction())
            {
                try
                {
                    transaction.Commit();
                    await Task.CompletedTask;
                }
                catch (Exception ec)
                {
                    Logger?.LogWarning($"Committing changes in {Pipeline}", ec.InnerException.Message);
                    return ActionResult.Failed()
                        .AddError($"Unable to save changes in {Pipeline}")
                        .AddError(ec.Message);
                }
            }

            return ActionResult.Success();
        }
        public string Pipeline { get; protected set; }
        public ISession Session { get; protected set; }
    }
}
