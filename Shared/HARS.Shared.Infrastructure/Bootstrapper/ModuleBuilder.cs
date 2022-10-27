using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Infrastructure.Bootstrapper
{
    public class ModuleBuilder<T> where T : Module, new()
    {
        internal ModuleBuilder(Bootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper;
        }

        public void Build()
        {
            _bootstrapper.ContinueRegistration<T>(_containerBuilder);
        }

        private readonly Bootstrapper _bootstrapper;
        private readonly ContainerBuilder _containerBuilder = new ContainerBuilder();
    }
}
