using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Contracts
{
    public interface IEmailServiceConfiguration
    {
        string Password { get; set; }
        string UserName { get; set; }
        string Host { get; set; }
        int Port { get; set; }
        string From { get; set; }
    }
}
