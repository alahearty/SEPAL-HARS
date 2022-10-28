using HARS.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.UserMangement.Core
{
    public interface IRoleRepository<T> where T : ApplicationRole
    {
        Task<bool> DoesRoleExist(string roleName);

        void Add(T role);

        void UpdateRole(T role);

        void Remove(T role);

        Task<ActionResult<IEnumerable<T>>> FetchAllRoles();

        Task<ActionResult<T>> FetchRoleByName(string roleName);

        Task<ActionResult<T>> FetchRoleById(Guid id);

        Task<IEnumerable<T>> FetchRolesByNames(IEnumerable<string> roleNames);
    }
}
