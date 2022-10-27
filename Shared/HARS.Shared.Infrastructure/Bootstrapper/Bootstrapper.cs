using Autofac;
using HARS.Shared.Architecture.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Infrastructure.Bootstrapper
{
    public class Bootstrapper
    {
        public Bootstrapper(CoreModuleSettings coreApplicationConfiguration)
        {
            ApplicationConfiguration = coreApplicationConfiguration;
            Application.SetContainers(Containers);
        }

        public void RegisterModule<T>() where T : Module, new()
        {
            AddModule<T>();
        }

        public void RegisterModuleWithServices<T>() where T : Module, new()
        {
            ModulesWithServices.Add(typeof(T));
        }

        public void RegisterModule<T, TCommand>(TCommand seedCommand = null) where T : Module, new()
            where TCommand : Command
        {
            AddModule<T>();

            var seedTask = Application.ExecuteCommandAsync<T, TCommand>(seedCommand);

            seedTask.Wait();

            if (seedTask.Result.NotSuccessful)
                throw new Exception(seedTask.Result.Errors.FirstOrDefault());
        }

        public TService ResolveFrom<TModule, TService>()
        {
            var module = Containers[typeof(TModule)];
            return module.Resolve<TService>();
        }

        public IContainer GetModuleContainer<T>() where T : Module
        {
            return Containers[typeof(T)];
        }

        public ModuleBuilder<T> BeginRegister<T>() where T : Module, new()
        {
            return new ModuleBuilder<T>(this);
        }

        internal void ContinueRegistration<T>(ContainerBuilder container) where T : Module, new()
        {
            if (Containers.ContainsKey(typeof(T))) return;
            var moduleInstance = new T();
            ConfigureContainer(moduleInstance, container);
        }

        private void AddModule<T>() where T : Module, new()
        {
            if (Containers.ContainsKey(typeof(T))) return;

            var moduleInstance = new T();
            var containerBuilder = new ContainerBuilder();

            ConfigureContainer(moduleInstance, containerBuilder);
        }

        public void SetApplicationLogContainer(IContainer container)
        {
            Application.SetApplicationLogContainer(container);
        }

        private void ConfigureContainer<T>(T moduleInstance, ContainerBuilder containerBuilder) where T : Module, new()
        {
            Task.WaitAll(moduleInstance
                            .InitialiseModuleAsync(ApplicationConfiguration, containerBuilder));

            var container = containerBuilder.Build();
            moduleInstance.Container = container;

            Containers.Add(typeof(T), container);
        }

        public async Task AddApplicationServiceAsync(IServiceProvider serviceProvider)
        {
            Application.ApplicationServiceProvider = serviceProvider;

            foreach (var type in ModulesWithServices)
            {
                if (Containers.ContainsKey(type)) return;

                var moduleInstance = Activator.CreateInstance(type) as Module;
                var containerBuilder = new ContainerBuilder();

                await moduleInstance.InitialiseModuleAsync(ApplicationConfiguration, containerBuilder);
                containerBuilder
                    .Register((_) => serviceProvider)
                    .As<IServiceProvider>()
                    .SingleInstance();

                var container = containerBuilder.Build();
                moduleInstance.Container = container;
                Containers.Add(type, container);
            }
        }
        protected Dictionary<Type, IContainer> Containers { get; } = new Dictionary<Type, IContainer>();
        internal List<Type> ModulesWithServices { get; } = new List<Type>();
        internal CoreModuleSettings ApplicationConfiguration { get; }
        public virtual Application Application { get; } = new Application();
    }
}
