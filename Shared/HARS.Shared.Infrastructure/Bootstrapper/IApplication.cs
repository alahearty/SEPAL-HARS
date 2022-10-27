using HARS.Shared.Architecture.Commands;
using HARS.Shared.Architecture.Querries;
using HARS.Shared.ChangeTracking;
using HARS.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Infrastructure.Bootstrapper
{
    public interface IApplication
    {
        Task<ActionResult<TResponse>> SendQueryAsync<TModule, TQuery, TResponse>(TQuery query)
            where TQuery : class, IQuery<TResponse>
            where TModule : Module;

        Task<ActionResult<TResponse>> ExecuteCommandAsync<TModule, TCommand, TResponse>(TCommand command)
            where TCommand : Command<TResponse>
            where TModule : Module;

        Task<ActionResult> ExecuteCommandAsync<TModule, TCommand>(TCommand command)
          where TCommand : Command
          where TModule : Module;

        public IServiceProvider ApplicationServiceProvider { get; set; }

        public IUserIdentity GetExecutingUser();
    }
}
