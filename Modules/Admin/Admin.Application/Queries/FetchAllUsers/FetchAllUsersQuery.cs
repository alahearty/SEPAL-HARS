using HARS.Shared.Architecture.Querries;
using HARS.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Application.Queries.FetchAllUsers
{
    public class FetchAllUsersQuery : IQuery<IEnumerable<UserProfileDto>>
    {
        public ActionResult Validate()
        {
            return ActionResult.Success();
        }
    }

    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsLockedOut { get; set; }
    }
}
