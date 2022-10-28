using HARS.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.UserMangement.Core
{
    public interface IUserRepository<T> where T : ApplicationUser
    {
        void Remove(T user);
        void UpdateUser(T user);
        void Add(T user);
        Task<List<T>> FetchAllUsers();
        Task<ActionResult<T>> FindUserByEmail(string email);
        Task<ActionResult<T>> FindUserById(Guid id);
        Task<ActionResult<IList<T>>> FetchUsers(IEnumerable<string> emails);
        Task<ActionResult<IList<T>>> FetchUsers(IEnumerable<Guid> userIds);
    }
}
