using Autofac;
using HARS.Shared.DataBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Configuration.SessionFactory
{
    internal static class Database
    {
        /// <summary>
        /// Creates the singleton session factory that will produce scoped sessions
        /// </summary>
        /// <param name="mappingAssembly">Assembly of mappings</param>
        /// <param name="schema">Database schema</param>
        /// <param name="builder">The Autofac containerbuilder to be hydrated</param>
        /// <param name="connectionString">Connection to the SQL database</param>
        /// <param name="dbType">Database type</param>
        /// <param name="isTesting">Used to support faster in-memory data testing</param>
        internal static void Configure(
            Assembly mappingAssembly, string schema, ContainerBuilder builder,
            string connectionString, DatabaseTypes dbType, bool isTesting)
        {
            builder
                .RegisterGeneric(typeof(ReadonlyQueryCollection<>))
                .As(typeof(IReadonlyQueryCollection<>))
                .InstancePerLifetimeScope();

            var sessionFactory = new SessionFactory(connectionString, dbType, mappingAssembly, schema, updateSchema: isTesting);

            if (!isTesting)
            {
                builder
                    .Register((o) => sessionFactory)
                    .As<INhibernateSessionFactory>()
                    .SingleInstance();

                builder
                    .Register((o) =>
                    {
                        var factoryInstance = o.Resolve<INhibernateSessionFactory>();
                        return factoryInstance.GetFreshSession();
                    })
                    .As<ISession>()
                    .InstancePerLifetimeScope();
            }
            else
            {
                //TODO: WIP for integration tests
            }
        }

        /// <summary>
        /// Used to create the database schema
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="schema">Database schema</param>
        internal static async Task CreateSchemaAsync(string connectionString, string schema, DatabaseTypes databaseType)
        {
            var query = $"CREATE SCHEMA [{schema}] AUTHORIZATION [dbo]";
            //schema may exist: silentFail. Alternatively first check for schema. But naah, unecessary dynamic SQL
            await SQL.ExecuteAsync(query, connectionString, databaseType, true);
        }
    }
}
