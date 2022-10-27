using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Infrastructure.ModuleConfiguration
{
    internal static class ConfigureUserManagementService
    {
        internal static void ConfigureUserManagementServices(this ContainerBuilder builder)
        {

            //builder.RegisterType<JWTTokenProvider>()
            //               .As<IJWTTokenProvider>()
            //               .InstancePerLifetimeScope();

            //builder.RegisterType<SecurityProvider>()
            //       .SingleInstance();

            //builder.RegisterType<TableReader>()
            //   .As<ITableReader>()
            //   .InstancePerLifetimeScope();

            //builder.RegisterType<ExcelFileImporter>()
            //        .As<IExcelFileImporter>()
            //        .InstancePerLifetimeScope();

        }
    }
}
