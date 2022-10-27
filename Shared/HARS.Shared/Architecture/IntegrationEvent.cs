using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Architecture
{
    public abstract class IntegrationEvent
    {
        protected IntegrationEvent()
        {
            TimeStamp = DateTime.UtcNow;
        }
        public DateTime TimeStamp { get; }
    }
}
