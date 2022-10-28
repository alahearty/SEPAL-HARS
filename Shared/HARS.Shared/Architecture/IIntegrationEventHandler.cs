using HARS.Shared.DataBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Architecture
{
    public interface IIntegrationEventHandler<TEvent> where TEvent : IntegrationEvent
    {
        Task HandleAsync(TEvent eventData, CancellationToken cancellationToken = default);
    }

    public abstract class IntegrationEventHandler<TEvent, TDbContext> : IIntegrationEventHandler<TEvent>
        where TEvent : IntegrationEvent
        where TDbContext : class, ICommandContext
    {
        public abstract Task HandleAsync(TEvent eventData, CancellationToken cancellationToken = default);

        public TDbContext DbContext { get; set; }
    }
}
