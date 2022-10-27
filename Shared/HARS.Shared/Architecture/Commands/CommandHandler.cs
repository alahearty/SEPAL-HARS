using HARS.Shared.ChangeTracking;
using HARS.Shared.DataBases;
using HARS.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Architecture.Commands
{
    public abstract class CommandHandler<TCommand, TDbContext, TResponse> : CommandHandler<TDbContext>,
        ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TDbContext : class, ICommandContext
    {
        protected ActionResult<TResponse> FailedOperation(string error)
        {
            return ActionResult<TResponse>.Failed()
                                        .SetErrors(new List<string>() { error });
        }
        protected ActionResult<TResponse> FailedOperation(ErrorCode errorCode, string error)
        {
            return ActionResult<TResponse>.Failed(errorCode)
                                        .SetErrors(new List<string> { error });
        }

        protected ActionResult<TResponse> FailedOperation(IEnumerable<string> errors)
        {
            return ActionResult<TResponse>.Failed()
                                        .SetErrors(errors);
        }

        protected ActionResult<TResponse> SuccessfulOperation(TResponse response)
        {
            return ActionResult<TResponse>.Success(response);
        }

        protected async Task<ActionResult> CommitToDatabaseAsync()
        {
            return await DbContext.CommitToDatabaseAsync();
        }

        List<ChangeLog> ICommandHandler<TCommand, TResponse>.ChangeLogs => ChangeLogs;

        public abstract Task<ActionResult<TResponse>> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }

    public abstract class CommandHandler<TCommand, TDbContext> : CommandHandler<TDbContext>, ICommandHandler<TCommand>
        where TCommand : Command
        where TDbContext : class, ICommandContext
    {
        protected ActionResult FailedOperation(string error)
        {
            return ActionResult.Failed()
                .SetErrors(new List<string>() { error });
        }
        protected ActionResult FailedOperation(ErrorCode errorCode, string error)
        {
            return ActionResult.Failed(errorCode)
                                        .SetErrors(new List<string> { error });
        }

        protected ActionResult FailedOperation(IEnumerable<string> errors)
        {
            return ActionResult.Failed()
                .SetErrors(errors);
        }

        protected ActionResult SuccessfulOperation()
        {
            return ActionResult.Success();
        }

        protected async Task<ActionResult> CommitToDatabaseAsync()
        {
            return await DbContext.CommitToDatabaseAsync();
        }

        List<ChangeLog> ICommandHandler<TCommand>.ChangeLogs => ChangeLogs;
        public abstract Task<ActionResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);

    }
    public interface ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        public List<ChangeLog> ChangeLogs { get; }
        Task<ActionResult<TResponse>> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }

    public interface ICommandHandler<TCommand> where TCommand : Command
    {
        public List<ChangeLog> ChangeLogs { get; }
        public Task<ActionResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }

    public abstract class CommandHandler<TDbContext>
    {
        public void AddLog(string message, ChangeLogType logType)
        {
            ChangeLogs.Add(new ChangeLog(message, logType));
        }

        internal List<ChangeLog> ChangeLogs { get; } = new List<ChangeLog>();
        public IEventClient EventClient { get; set; }
        public TDbContext DbContext { get; set; }
    }
}
