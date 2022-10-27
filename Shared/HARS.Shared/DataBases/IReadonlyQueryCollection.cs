using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.DataBases
{
    public interface IReadonlyQueryCollection<T>
    {
        IQueryable<T> Entities { get; }
    }
}
