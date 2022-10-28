using Admin.Application;
using Admin.Dapper;
using Admin.Dapper.UserManagement.Mappings;
using Admin.Domain;
using Admin.Infrastructure.ModuleConfiguration;
using Autofac;
using HARS.Shared.Infrastructure.Bootstrapper;
using NHibernate.Configuration.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Infrastructure
{
    public class AdminModule : Module<AdminContext>
    {
        public override async Task InitialiseModuleAsync(CoreModuleSettings coreConfiguration, ContainerBuilder containerBuilder)
        {
            await base.InitialiseModuleAsync(coreConfiguration, containerBuilder);
            SetDbContext<AdminDbContext>(containerBuilder);

            var tokenSecret = coreConfiguration.TokenSecret;
            var deploymentConfig = coreConfiguration.DeploymentSettings;
            var emailConfig = coreConfiguration.EmailSettings;

            containerBuilder.ConfigureApplicationSettings(tokenSecret, deploymentConfig);
            containerBuilder.ConfigureUserManagementServices();

            //for NHibernate
           await new DatabaseConfigurationBuilder()
                   .SetSchema(AdminDbContext.SCHEMA)
                   .SetDatabaseType(coreConfiguration.SQLDatabaseType)
                   .SetConnectionString(coreConfiguration.SQLConnectionString)
                   .IsTesting(coreConfiguration.IsTesting)
                   .MappingFrom<HARSUserMapping>()
                   .RunMigrationsFrom(typeof(HARSUserMapping).Assembly)
                   .SetupDatabaseViews(typeof(HARSUserMapping).Assembly)
                   .SetupContainerAsync(containerBuilder);
        }

        public override string Name => "Admin Module";

        public override Assembly ApplicationAssembly => typeof(Class1).Assembly;
        public override Assembly InfrastructureAssembly => typeof(AdminModule).Assembly;
    }

}
