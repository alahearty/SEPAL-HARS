using Admin.Dapper.UserManagement.Repositories;
using Admin.Domain.UsersManagement;
using HARS.Shared.Utilities;
using HARS.UserMangement.Core;
using NHibernate;
using NHibernate.Configuration.DatabasePipeline;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Dapper.UserManagement
{
    public class AccessManager : Repository, IAccessManager
    {
        public AccessManager(ISession session) : base(session)
        {
            UserRepository = new UserRepository(Session);
        }

        public void UpdatePassword(HARSUser user, string newPassword)
        {
            user.PasswordHash = HashPassword(newPassword);
            Session.SaveOrUpdate(user);
        }

        public string HashPassword(string plainPassword)
        {
            return plainPassword; // to be updated
        }

        public async Task<bool> AdminAccountSeeded()
        {
            var admin = Session.Query<HARSUser>()
                               .FirstOrDefault(i => i.AccountType == AccountTypes.Admin);

            return await Task.FromResult(admin != null);
        }

        public async Task<bool> DoesUserExistAsync(string email)
        {
            return await Session
                .Query<HARSUser>()
                .AnyAsync(user => user.Email.ToLower() == email.ToLower());
        }

        public virtual IUserRepository<HARSUser> UserRepository { get; }

    }
}
