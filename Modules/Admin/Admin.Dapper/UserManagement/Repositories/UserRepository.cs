using Admin.Domain.UsersManagement;
using HARS.Shared.Utilities;
using HARS.UserMangement.Core;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Dapper.UserManagement.Repositories
{
    public sealed class UserRepository : IUserRepository<HARSUser>
    {
        public UserRepository(ISession activeSession)
        {
            ActiveSession = activeSession;
        }

        public void Remove(HARSUser user)
        {
            ActiveSession.Delete(user);
        }

        public async Task<List<HARSUser>> FetchAllUsers()
        {
            var users = await ActiveSession.Query<HARSUser>()
                                           .Where(user => user.AccountType != AccountTypes.Admin)
                                           .ToListAsync();

            return users;
        }

        public async Task<ActionResult<HARSUser>> FindUserByEmail(string email)
        {
            if (email == null) return ActionResult<HARSUser>.Failed();
            var users = await ActiveSession.Query<HARSUser>()
                                           .Where(user => user.Email.ToLower().Equals(email.ToLower())).ToListAsync();
            //.FetchMany(user => user.Roles).ToListAsync();
            if (users.FirstOrDefault() == null) return ActionResult<HARSUser>.Failed();
            return ActionResult<HARSUser>.Success(users.FirstOrDefault());
        }

        public void Add(HARSUser user)
        {
            ActiveSession.SaveOrUpdate(user);
        }

        public void UpdateUser(HARSUser user)
        {
            ActiveSession.Update(user);
        }

        public async Task<ActionResult<HARSUser>> FindUserById(Guid id)
        {
            var users = await ActiveSession.Query<HARSUser>()
                                    .Where(user => user.Id == id).ToListAsync();
            if (users.FirstOrDefault() == null) return ActionResult<HARSUser>.Failed();
            return ActionResult<HARSUser>.Success(users.FirstOrDefault());
        }

        public async Task<ActionResult<IList<HARSUser>>> FetchUsers(IEnumerable<string> emails)
        {
            var emailCollection = emails.ToList();
            var users = await ActiveSession.QueryOver<HARSUser>()
               .WhereRestrictionOn(user => user.Email).IsIn(emailCollection).ListAsync();

            return users != null ? ActionResult<IList<HARSUser>>.Success(users) : ActionResult<IList<HARSUser>>.Failed();
        }

        public async Task<ActionResult<IList<HARSUser>>> FetchUsers(IEnumerable<Guid> userIds)
        {
            var userCollection = userIds.ToList();
            var users = await ActiveSession.QueryOver<HARSUser>()
               .WhereRestrictionOn(user => user.Id).IsIn(userCollection).ListAsync();

            return users != null ? ActionResult<IList<HARSUser>>.Success(users) : ActionResult<IList<HARSUser>>.Failed();
        }

        public ISession ActiveSession { get; }
    }
}
