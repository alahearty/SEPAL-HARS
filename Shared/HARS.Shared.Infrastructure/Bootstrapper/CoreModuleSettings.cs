using HARS.Shared.Architecture;
using HARS.Shared.Contracts;
using HARS.Shared.DataBases;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Infrastructure.Bootstrapper
{
    public sealed class CoreModuleSettings
    {
        public IEmailServiceConfiguration EmailSettings { get; set; }
        public ILoggerFactory LoggerFactory { get; set; }
        public DatabaseTypes SQLDatabaseType { get; set; }
        public string SQLConnectionString { get; set; }
        public DeploymentConfiguration DeploymentSettings { get; set; }
        public IEventClient EventClient { get; set; }
        public ITokenSecret TokenSecret { get; set; }
        public bool IsTesting { get; set; } = false;
        public string WebRootPath { get; set; }
    }
}
