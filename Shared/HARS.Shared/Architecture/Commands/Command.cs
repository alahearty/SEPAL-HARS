using HARS.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Architecture.Commands
{
    public abstract class Command<TResult> : Command, ICommand<TResult> { }

    public abstract class Command
    {
        public abstract ActionResult Validate();
    }
}
