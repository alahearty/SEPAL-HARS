using Autofac;
using HARS.Shared.Architecture.Commands;
using HARS.Shared.Architecture.Querries;
using HARS.Shared.ChangeTracking;
using HARS.Shared.Exceptions;
using HARS.Shared.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Infrastructure.Bootstrapper
{
    public class Application : IApplication
    {
        public async Task<ActionResult<TResponse>> SendQueryAsync<TModule, TQuery, TResponse>(TQuery query)
            where TQuery : class, IQuery<TResponse>
            where TModule : Module
        {
            var validationOutput = query?.Validate();
            if (validationOutput.NotSuccessful)
            {
                return ActionResult<TResponse>
                    .Failed(ErrorCode.BadRequest)
                    .SetErrors(validationOutput.Errors);
            }

            var container = Containers[typeof(TModule)];

            using var scope = container.BeginLifetimeScope();
            try
            {
                var handler = scope.Resolve<IQueryHandler<TQuery, TResponse>>();
                return await handler.HandleAsync(query);
            }
            catch (DomainException ex)
            {
                Debug.WriteLine(ex.Message);
                return ActionResult<TResponse>.Failed().AddError(ex.Message);
            }
        }

        public async Task<ActionResult<TResponse>> ExecuteCommandAsync<TModule, TCommand, TResponse>(TCommand command)
            where TCommand : Command<TResponse>
            where TModule : Module
        {
            var validationOutput = command?.Validate();
            if (validationOutput.NotSuccessful)
            {
                return ActionResult<TResponse>
                    .Failed(ErrorCode.BadRequest)
                    .SetErrors(validationOutput.Errors);
            }

            using var scope = CreateModuleScope<TModule>();
            var handler = scope.Resolve<ICommandHandler<TCommand, TResponse>>();
            try
            {
                var response = await handler.HandleAsync(command);
                if (response.WasSuccessful && handler.ChangeLogs?.Count > 0)
                {
                    await SaveLogsAsync(handler.ChangeLogs);
                }

                return response;
            }
            catch (DomainException ex)
            {
                Debug.WriteLine(ex.Message);
                return ActionResult<TResponse>.Failed().AddError(ex.Message);
            }
        }

        internal void SetApplicationLogContainer(IContainer container)
        {
            _applicationLoggerContainer = container;
        }

        private async Task SaveLogsAsync(List<ChangeLog> changeLogs)
        {
            var saveLogCommand = new SaveChangeLogCommand
            {
                Initiator = GetExecutingUser()?.Email,
                ChangeLogs = changeLogs
            };

            using var scope = _applicationLoggerContainer?.BeginLifetimeScope();

            if (scope != null)
            {
                var handler = scope.Resolve<ICommandHandler<SaveChangeLogCommand>>();

                await handler.HandleAsync(saveLogCommand);
            }
        }

        public async Task<ActionResult> ExecuteCommandAsync<TModule, TCommand>(TCommand command)
           where TCommand : Command
           where TModule : Module
        {
            var validationOutput = command?.Validate();
            if (validationOutput.NotSuccessful)
            {
                return ActionResult
                    .Failed(ErrorCode.BadRequest)
                    .SetErrors(validationOutput.Errors);
            }

            using var scope = CreateModuleScope<TModule>();
            var handler = scope.Resolve<ICommandHandler<TCommand>>();
            try
            {
                var response = await handler.HandleAsync(command);

                if (response.WasSuccessful && handler.ChangeLogs?.Count > 0)
                {
                    await SaveLogsAsync(handler.ChangeLogs);
                }

                return response;
            }
            catch (DomainException ex)
            {
                Debug.WriteLine(ex.Message);
                return ActionResult.Failed().AddError(ex.Message);
            }
        }

        internal void SetContainers(Dictionary<Type, IContainer> containers)
        {
            Containers = containers;
        }

        internal void SetAPIServices(IServiceProvider serviceProvider)
        {
            ApplicationServiceProvider = serviceProvider;
        }

        public IUserIdentity GetExecutingUser()
        {
            using var identityScope = ApplicationServiceProvider.CreateScope();
            return identityScope.ServiceProvider.GetService<IUserIdentity>();
        }

        private ILifetimeScope CreateModuleScope<TModule>() where TModule : Module
        {
            var container = Containers[typeof(TModule)];
            var scope = container.BeginLifetimeScope();
            return scope;
        }

        private ILifetimeScope CreateModuleScope(object obj)
        {
            var container = Containers[obj.GetType()];
            var scope = container.BeginLifetimeScope();
            return scope;
        }

        private IContainer _applicationLoggerContainer;
        internal Dictionary<Type, IContainer> Containers { get; set; }
        public IServiceProvider ApplicationServiceProvider { get; set; }
    }
}
