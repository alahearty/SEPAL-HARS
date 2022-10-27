using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared
{
    public class DeploymentConfiguration
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string DefaultPassword { get; set; }
        public string ServerAddress { get; set; }
    }
}
