using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.ChangeTracking
{
    public class ChangeLog
    {
        public ChangeLog(string message, ChangeLogType logType)
        {
            LogMessage = message;
            LogType = logType;
        }

        public string LogMessage { get; }
        public ChangeLogType LogType { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
    }

    public enum ChangeLogType
    {
        Admin,
        Dashboard,
        DataManager,
        HydrocarbornAllocation
    }
}
