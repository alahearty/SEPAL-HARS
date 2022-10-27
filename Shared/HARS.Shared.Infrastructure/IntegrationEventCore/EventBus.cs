using HARS.Shared.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Infrastructure.IntegrationEventCore
{
    public sealed partial class EventBus
    {
        static EventBus() { }

        private EventBus()
        {
            _subscriptions = new Dictionary<Type, List<Delegate>>();
        }

        public void Subscribe<T>(EventHandlerCallback<T> handler) where T : IntegrationEvent
        {
            if (!_subscriptions.ContainsKey(typeof(T)))
            {
                _subscriptions.Add(typeof(T), new List<Delegate>());
            }
            var listener = handler;

            _subscriptions[typeof(T)].Add(listener);
        }

        public void Publish<T>(T @event) where T : IntegrationEvent
        {
            if (@event == null) return;

            try
            {
                var subscribers = _subscriptions[typeof(T)];

                foreach (var integrationEventHandler in subscribers)
                {
                    (integrationEventHandler as EventHandlerCallback<T>)(@event);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static EventBus Instance { get; } = new EventBus();
        private readonly Dictionary<Type, List<Delegate>> _subscriptions;
    }
}
