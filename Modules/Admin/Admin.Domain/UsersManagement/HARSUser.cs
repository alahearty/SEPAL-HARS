using HARS.Shared.Architecture;
using HARS.UserMangement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Domain.UsersManagement
{
    public class HARSUser : ApplicationUser, IUser, IEntity
    {
        public HARSUser(string firstName, string lastName, string email, AccountTypes accountType = AccountTypes.Regular)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = email;
            AccountType = accountType;
        }
        public virtual void ChangeAccountType(AccountTypes accountType)
        {
            AccountType = accountType;
            //Token?.ChangeAccountType(AccountType);
        }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string UserName { get; set; }
        public virtual AccountTypes AccountType { get; protected set; } = AccountTypes.Regular;
        public virtual bool IsLockedOut { get; set; }
        public virtual bool IsDeleted { get; set; }
    }

    //public class ApplicationUser : IdentityUser<Guid>
    //{
    //}
    public enum AccountTypes
    {
        Admin,
        Regular
    }
}
