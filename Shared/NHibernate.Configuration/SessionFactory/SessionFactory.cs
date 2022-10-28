using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using HARS.Shared.DataBases;
using NHibernate.Tool.hbm2ddl;
using nhb = NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Configuration.SessionFactory
{
    public class SessionFactory : INhibernateSessionFactory
    {
        public SessionFactory(string connectionString, DatabaseTypes type, Assembly assembly, string schema, bool updateSchema = true)
        {
            _connectionString = connectionString;
            DatabaseType = type;
            Assemblies = new List<Assembly>() { assembly };
            Schema = schema;
            UpdateSchema = updateSchema;
            Factory = InitializeSessionFactory();
        }

        /// <summary>
        /// Ensure use of a single session Factory for application. Contrasted to new session factory per request.
        /// A new request will use a new Session
        /// </summary>
        /// <returns></returns>
        public ISession GetFreshSession()
        {
            return Factory.OpenSession();
        }

        private ISessionFactory InitializeSessionFactory()
        {
            return DatabaseType switch
            {
                DatabaseTypes.MsSql => InitializeSessionFactoryForMsSql(),
                DatabaseTypes.Sqlite => InitializeSessionFactoryForSqlite(),
                _ => null,
            };
        }

        private ISessionFactory InitializeSessionFactoryForMsSql()
        {
            return Fluently.Configure().Database(SetupMsSQL)
                                       .Mappings((m) => Assemblies.ForEach(assembly => m.FluentMappings.AddFromAssembly(assembly)))
                                       .ExposeConfiguration(ConfigureSchemas())
                                       .BuildSessionFactory();
        }

        private ISessionFactory InitializeSessionFactoryForSqlite()
        {
            return Fluently.Configure().Database(SetupSQLite)
                                       .Mappings((m) => Assemblies.ForEach(assembly => m.FluentMappings.AddFromAssembly(assembly)))
                                       .ExposeConfiguration(ConfigureSchemas())
                                       .BuildSessionFactory();
        }

        private SQLiteConfiguration SetupSQLite()
        {
            return SQLiteConfiguration.Standard.ShowSql().DefaultSchema(Schema)
                                                     .ConnectionString(_connectionString);
        }

        private Action<nhb.Configuration> ConfigureSchemas()
        {
            return cfg =>
            {
                if (!UpdateSchema) return;
                var schema = new SchemaUpdate(cfg);
                schema.Execute(useStdOut: true, true);
                cfg.SetProperty(nhb.Environment.CommandTimeout, "2000");
            };

        }

        private MsSqlConfiguration SetupMsSQL()
        {
            return MsSqlConfiguration.MsSql2012.ShowSql().DefaultSchema(Schema)
                                                     .ConnectionString(_connectionString);
        }

        private DatabaseTypes DatabaseType { get; }
        public List<Assembly> Assemblies { get; }
        public string Schema { get; }
        public ISessionFactory Factory { get; }
        public bool UpdateSchema { get; }

        private readonly string _connectionString;
    }
}
