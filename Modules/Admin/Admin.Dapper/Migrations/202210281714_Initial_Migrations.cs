using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Dapper.Migrations
{
    [Migration(202210281714)]
    internal class M202210281714_Initial_Migrations : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("Users")
                .InSchema(AdminDbContext.SCHEMA)
                .WithColumn("Id").AsGuid().Unique().NotNullable().PrimaryKey()
                .WithColumn("FirstName").AsString().NotNullable()
                .WithColumn("LastName").AsString().NotNullable()
                .WithColumn("Email").AsString().NotNullable().Unique("IX_Users_Email")
                .WithColumn("IsLockedOut").AsBoolean()
                .WithColumn("AccountType").AsString().NotNullable()
                .WithColumn("PhoneNumber").AsString().Nullable()
                .WithColumn("PasswordHash").AsString().NotNullable();
        }
    }
}
