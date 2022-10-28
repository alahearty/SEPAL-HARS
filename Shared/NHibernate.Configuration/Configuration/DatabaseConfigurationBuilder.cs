using Autofac;
using HARS.Shared.DataBases;
using HARS.Shared.Extensions;
using NHibernate.Configuration.Migrations;
using NHibernate.Configuration.SessionFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Configuration.Configuration
{
    public class DatabaseConfigurationBuilder
    {
        public DatabaseConfigurationBuilder SetSchema(string schema)
        {
            _schema = schema;
            return this;
        }

        public DatabaseConfigurationBuilder SetDatabaseType(DatabaseTypes type)
        {
            _databaseType = type;
            return this;
        }

        public DatabaseConfigurationBuilder SetConnectionString(string connectionString)
        {
            _connectionString = connectionString.IsValid() ? connectionString : throw new ArgumentException("Invalid connection string");
            return this;
        }

        public DatabaseConfigurationBuilder IsTesting(bool isTesting = false)
        {
            _isTesting = isTesting;
            return this;
        }

        public DatabaseConfigurationBuilder MappingFrom<T>()
        {
            _mappingAssembly = typeof(T).Assembly;
            return this;
        }

        public DatabaseConfigurationBuilder RunMigrationsFrom(Assembly assembly)
        {
            _migrationsAssembly = assembly;
            return this;
        }

        public DatabaseConfigurationBuilder SetupDatabaseViews(Assembly assembly, string viewsFolder = "Views")
        {
            _viewsAssembly = assembly;
            _viewsFolder = viewsFolder;
            return this;
        }

        public async Task SetupContainerAsync(ContainerBuilder containerBuilder)
        {
            if (_schema != null)
                await Database.CreateSchemaAsync(_connectionString, _schema, _databaseType);

            if (_migrationsAssembly != null)
                RunMigrations();

            if (_viewsAssembly != null)
            {
                await new ViewsBuilder()
                       .SetAssembly(_viewsAssembly)
                       .SetFolder(_viewsFolder)
                       .SetSchema(_schema)
                       .CreateSQLViewsAsync(_connectionString, _databaseType);
            }

            if (_mappingAssembly != null)
                Database.Configure(_mappingAssembly, _schema, containerBuilder, _connectionString, _databaseType, _isTesting);
        }

        private void RunMigrations()
        {
            try
            {
                new MigrationRunner(_migrationsAssembly, _connectionString, _databaseType).Run();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private string _schema;
        private DatabaseTypes _databaseType;
        private string _connectionString;
        private Assembly _mappingAssembly;
        private bool _isTesting;
        private Assembly _migrationsAssembly;
        private Assembly _viewsAssembly;
        private string _viewsFolder;
    }
}
