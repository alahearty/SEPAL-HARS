using HARS.Shared.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.DataBases
{
    public abstract class CommandAccessor<T> : ICommandContext
    {
        public abstract Task<ActionResult> CommitToDatabaseAsync();
        public virtual T Store { get; set; }
        public ILogger Logger { get; protected set; }
    }

    public interface ICommandContext
    {
        Task<ActionResult> CommitToDatabaseAsync();
    }
}
