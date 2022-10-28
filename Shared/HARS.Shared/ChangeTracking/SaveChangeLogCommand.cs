using HARS.Shared.Architecture.Commands;
using HARS.Shared.Builders;
using HARS.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.ChangeTracking
{
    public class SaveChangeLogCommand : Command
    {
        public override ActionResult Validate()
        {
            return new FluentValidator()
               .IsValidEmail(Initiator, "Valid user email is required.")
               .IsValidCollection(ChangeLogs, "Audit note cannot be null or empty.")
               .Result;
        }

        public string Initiator { get; set; }
        public IEnumerable<ChangeLog> ChangeLogs { get; set; }
    }
}
