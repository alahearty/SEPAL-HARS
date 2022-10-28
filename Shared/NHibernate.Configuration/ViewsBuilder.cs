using HARS.Shared.DataBases;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace NHibernate.Configuration
{
    public class ViewsBuilder
    {
        /// Format: The format for embedded resources is "{Namespace}.{Folder}.{filename}.{Extension}"
        /// All .sql scripts which are for view creation should be embedded resources.
        public ViewsBuilder SetAssembly(Assembly assembly)
        {
            _fromAssembly = assembly;
            _assemblyNamespace = _fromAssembly.GetName().Name;
            return this;
        }

        public ViewsBuilder SetFolder(string path = "Views")
        {
            _folderPath = path;
            return this;
        }

        public ViewsBuilder SetSchema(string schema)
        {
            _schema = schema;
            return this;
        }

        public async Task CreateSQLViewsAsync(string connectionString, DatabaseTypes databaseType)
        {
            var resourceFiles = GetEmbeddedResources();
            using var connection = SQL.CreateConnection(connectionString, databaseType);

            try
            {
                await connection.OpenAsync();
                foreach (var file in resourceFiles)
                {
                    /// Format: {schema}.{viewName}
                    /// The name of the view will be extracted from the 'viewname.sql'
                    await SQL.ExecuteAsync($"DROP VIEW IF EXISTS [{_schema}].[{file.viewName}]", connection, databaseType);
                    await SQL.ExecuteAsync(file.sqlScript, connection, databaseType);
                }
            }
            catch (SqlException ex) { throw new Exception("Failed to create views", ex); }
            finally { connection.Close(); }
        }

        private string RemoveExtension(string viewFile)
        {
            return viewFile.Split(".sql")[0];
        }

        private IEnumerable<(string viewName, string sqlScript)> GetEmbeddedResources()
        {
            var resourceNameSpace = $"{_assemblyNamespace}.{_folderPath}";
            var resources = _fromAssembly
                .GetManifestResourceNames()
                .Where(resourceName => resourceName.StartsWith(resourceNameSpace));

            foreach (var resource in resources)
            {
                var fileName = resource.Replace(resourceNameSpace, string.Empty).TrimStart('.');
                var stream = _fromAssembly.GetManifestResourceStream($"{resourceNameSpace}.{fileName}");

                using var reader = new StreamReader(stream);
                yield return (RemoveExtension(fileName), reader.ReadToEnd());
            }
        }

        private string _folderPath;
        private Assembly _fromAssembly;
        private string _assemblyNamespace;
        private string _schema;
    }
}