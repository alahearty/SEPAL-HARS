using Autofac;
using HARS.Shared;
using HARS.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Infrastructure.ModuleConfiguration
{
    internal static class ConfigureApplicationSetting
    {
        internal static void ConfigureApplicationSettings(this ContainerBuilder builder,
            ITokenSecret tokenSecret,
            DeploymentConfiguration deploymentConfiguration)
        {

            builder.Register((o) => tokenSecret)
                .SingleInstance();

            builder.Register((o) => deploymentConfiguration)
                .SingleInstance();
        }
    }
}
