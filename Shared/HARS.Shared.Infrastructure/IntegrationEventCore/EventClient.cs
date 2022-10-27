using HARS.Shared.Architecture;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Infrastructure.IntegrationEventCore
{
    public class EventClient : IEventClient
    {

        public EventClient(ILogger logger)
        {
            _logger = logger;
        }

        public void Dispose()
        {
        }

        public void Publish<T>(T @event) where T : IntegrationEvent
        {
            _logger.LogInformation("Publishing {Event}", @event.GetType().FullName);
            EventBus.Instance.Publish(@event);
        }

        public void Subscribe<T>(EventHandlerCallback<T> handler)
            where T : IntegrationEvent
        {
            EventBus.Instance.Subscribe(handler);
        }

        private readonly ILogger _logger;
    }
}
