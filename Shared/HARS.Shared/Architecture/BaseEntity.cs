using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Architecture
{
    public class BaseEntity : IEntity
    {
        public virtual Guid Id { get; set; }
    }

    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
