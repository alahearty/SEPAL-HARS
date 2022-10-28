using Admin.Domain.UsersManagement;
using NHibernate.Configuration.BaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Dapper.UserManagement.Mappings
{
    public class HARSUserMapping : BaseEntityMapping<HARSUser>
    {
        public HARSUserMapping()
        {
            Table("Users");
            Map(user => user.FirstName).IsRequiredColumn();
            Map(user => user.LastName).IsRequiredColumn();
            Map(user => user.Email).Unique().IsRequiredColumn();
            Map(user => user.IsLockedOut);
            Map(user => user.IsDeleted);
            Map(user => user.AccountType).IsRequiredColumn();
            Map(user => user.PhoneNumber);
            Map(user => user.PasswordHash).IsRequiredColumn();
        }
    }

    
}
