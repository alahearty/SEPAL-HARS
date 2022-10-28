using FluentMigrator.Runner;
using HARS.Shared.DataBases;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Configuration.Migrations
{
    internal class MigrationRunner
    {
        internal MigrationRunner(Assembly migrationsAssembly, string connectionString, DatabaseTypes databaseType)
        {
            _connectionString = connectionString;
            _databaseType = databaseType;
            _migrationsAssembly = migrationsAssembly ?? throw new ArgumentNullException("Invalid migration assembly");
            _serviceProvider = CreateServices();
        }

        internal void Run()
        {
            using var scope = _serviceProvider.CreateScope();
            var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
            runner.MigrateUp();
        }

        private IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(BuildConnection())
                .BuildServiceProvider(false);
        }

        private Action<IMigrationRunnerBuilder> BuildConnection()
        {
            if (_databaseType == DatabaseTypes.Sqlite)
            {
                return builder => builder
                                .AddSQLite()
                                .WithGlobalConnectionString(_connectionString)
                                .ScanIn(_migrationsAssembly).For.Migrations();
            }
            else
            {
                return builder => builder
                                .AddSqlServer2012()
                                .WithGlobalConnectionString(_connectionString)
                                .ScanIn(_migrationsAssembly).For.Migrations();
            }
        }

        private readonly Assembly _migrationsAssembly;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _connectionString;
        private readonly DatabaseTypes _databaseType;
    }
}
