using HARS.Shared.DataBases;
using HARS.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Architecture.Querries
{
    public interface IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        Task<ActionResult<TResponse>> HandleAsync(TQuery query, CancellationToken token = default);
    }
    public abstract class QueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    {
        public abstract Task<ActionResult<TResponse>> HandleAsync(TQuery query, CancellationToken token = default);
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

        protected ActionResult<TResponse> FailedOperation(ErrorCode errorCode, List<string> errors)
        {
            return ActionResult<TResponse>.Failed(errorCode)
                                        .SetErrors(errors);
        }

        protected ActionResult<TResponse> SuccessfulOperation(TResponse response)
        {
            return ActionResult<TResponse>.Success(response);
        }
    }

    public abstract class QueryHandler<TQuery, TEntity, TResponse> : QueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public virtual IReadonlyQueryCollection<TEntity> QueryContext { get; protected set; }
    }
}
