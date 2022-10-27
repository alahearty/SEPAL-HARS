using Autofac;
using HARS.Shared.Architecture;
using HARS.Shared.Architecture.Commands;
using HARS.Shared.Architecture.Querries;
using HARS.Shared.DataBases;
using HARS.Shared.Infrastructure.Communications.Emails;
using HARS.Shared.Infrastructure.IntegrationEventCore;
using HARS.Shared.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Infrastructure.Bootstrapper
{
    public abstract class Module<TContext> : Module
    {
        public override Task InitialiseModuleAsync(CoreModuleSettings coreConfiguration, ContainerBuilder containerBuilder)
        {
            _applicationConfiguration = coreConfiguration;
            RegisterLogger(coreConfiguration, containerBuilder);

            if (!typeof(TContext).IsInterface)
            {
                containerBuilder
                    .RegisterType<TContext>()
                    .PropertiesAutowired()
                    .InstancePerLifetimeScope();
            }

            RegisterEventClient(containerBuilder);
            RegisterCommandHandlers(containerBuilder);
            RegisterQueryHandlers(containerBuilder);
            RegisterEmailingService(containerBuilder);
            RegisterIntegrationEventHandlers(containerBuilder);

            return Task.CompletedTask;
        }

        private void RegisterLogger(CoreModuleSettings coreConfiguration, ContainerBuilder containerBuilder)
        {
            var logger = coreConfiguration.LoggerFactory.CreateLogger(Name);
            logger.LogInformation($"Initialising {Name} application");

            containerBuilder.Register((o) => logger)
                .InstancePerLifetimeScope();
        }

        private void RegisterIntegrationEventHandlers(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterAssemblyTypes(ApplicationAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(IIntegrationEventHandler<>)))
                .As(type => FilterHandlers(type, typeof(IIntegrationEventHandler<>)))
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
        }

        private void RegisterEmailingService(ContainerBuilder containerBuilder)
        {
            if (InfrastructureAssembly != null)
            {
                containerBuilder
                    .Register((o) => _applicationConfiguration.DeploymentSettings);
                containerBuilder
                    .RegisterAssemblyTypes(InfrastructureAssembly)
                    .Where(t => t.IsClosedTypeOf(typeof(IEmailGenerator<>)))
                    .As(type => FilterHandlers(type, typeof(IEmailGenerator<>)))
                    .InstancePerLifetimeScope();
            }

            containerBuilder
                .Register((context) => new EmailService(_applicationConfiguration.EmailSettings))
                .As<IEmailServer>()
                .SingleInstance();
        }

        private void RegisterQueryHandlers(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterAssemblyTypes(ApplicationAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(IQueryHandler<,>)))
                .As(type => FilterHandlers(type, typeof(IQueryHandler<,>)))
                .PropertiesAutowired()
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterAssemblyTypes(ApplicationAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(QueryHandler<,,>)))
                .As(type => FilterHandlers(type, typeof(IQueryHandler<,>)))
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
        }

        protected void SetDbContext<T>(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<T>()
                .As<CommandAccessor<TContext>>()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
        }

        private void RegisterCommandHandlers(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterAssemblyTypes(ApplicationAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(CommandHandler<,,>)))
                .As(type => FilterHandlers(type, typeof(ICommandHandler<,>)))
                .AsClosedTypesOf(typeof(CommandHandler<,,>))
                .PropertiesAutowired()
                .InstancePerLifetimeScope();

            containerBuilder
               .RegisterAssemblyTypes(ApplicationAssembly)
               .Where(type => type.IsClosedTypeOf(typeof(CommandHandler<,>)))
               .As(type => FilterHandlers(type, typeof(ICommandHandler<>)))
               .PropertiesAutowired()
               .InstancePerLifetimeScope();
        }

        private Type FilterHandlers(Type type, Type handlerType)
        {
            return type
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType)
                .Single();
        }

        private void RegisterEventClient(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterType<EventClient>()
                .As<IEventClient>()
                .SingleInstance();
        }
        private CoreModuleSettings _applicationConfiguration;
    }

    public abstract class Module
    {
        public abstract Task InitialiseModuleAsync(CoreModuleSettings coreConfiguration, ContainerBuilder containerBuilder);

        public void ListenTo<TEvent>() where TEvent : IntegrationEvent
        {
            EventBus.Instance.Subscribe<TEvent>(async (@event) =>
            {
                using var scope = Container.BeginLifetimeScope();
                var handler = scope.Resolve<IIntegrationEventHandler<TEvent>>();
                await handler.HandleAsync(@event);
            });
        }

        internal IContainer Container { get; set; }
        public abstract string Name { get; }
        public abstract Assembly ApplicationAssembly { get; }
        public virtual Assembly InfrastructureAssembly { get; }
    }
}
