using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Architecture
{
    public delegate void EventHandlerCallback<T>(T @event);

    public interface IEventClient : IDisposable
    {
        void Publish<T>(T @event)
            where T : IntegrationEvent;

        void Subscribe<T>(EventHandlerCallback<T> handler)
            where T : IntegrationEvent;
    }
}
