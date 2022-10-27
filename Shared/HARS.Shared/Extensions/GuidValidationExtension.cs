using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Extensions
{
    public static class GuidValidationExtension
    {
        public static bool IsValidGuid(this Guid id)
        {
            return id != Guid.Empty;
        }

        public static bool IsInvalidGuid(this Guid id)
        {
            return id == Guid.Empty;
        }
    }
}
