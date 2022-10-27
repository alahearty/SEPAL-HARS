using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string error) : base(error) { }
    }
}
