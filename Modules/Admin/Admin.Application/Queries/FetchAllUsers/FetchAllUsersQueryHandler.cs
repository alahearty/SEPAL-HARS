using Admin.Domain.UsersManagement;
using HARS.Shared.Architecture.Querries;
using HARS.Shared.DataBases;
using HARS.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Application.Queries.FetchAllUsers
{
    public class FetchAllUsersQueryHandler :
        QueryHandler<FetchAllUsersQuery, HARSUser, IEnumerable<UserProfileDto>>
    {
        public FetchAllUsersQueryHandler(IReadonlyQueryCollection<HARSUser> profileCollection)
        {
            QueryContext = profileCollection;
        }

        public override async Task<ActionResult<IEnumerable<UserProfileDto>>> HandleAsync(FetchAllUsersQuery query, CancellationToken token = default)
        {

            var queryResult = QueryContext.Entities.Where(x => x.IsDeleted == false && x.AccountType != AccountTypes.Admin).ToList();
            var users = new List<UserProfileDto>();
            await Task.WhenAll(queryResult.Select(async x =>
                users.Add(new UserProfileDto()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    IsLockedOut = x.IsLockedOut
                })));

            return SuccessfulOperation(users);
        }
    }
}
