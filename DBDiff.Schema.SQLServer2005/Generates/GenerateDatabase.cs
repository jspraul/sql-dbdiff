using DBDiff.Schema.SQLServer.Generates.Model;
using DBDiff.Schema.SQLServer.Generates.Options;

namespace DBDiff.Schema.SQLServer.Generates.Generates
{
    public class GenerateDatabase
    {
        private string _connectionString;
        private SqlOption _objectFilter;

                /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="connectioString">Connection string to the SQL Server</param>
        public GenerateDatabase(string connectionString, SqlOption filter)
        {
            this._connectionString = connectionString;
            this._objectFilter = filter;
        }

        public DatabaseInfo Get(Database database)
        {
            return new DatabaseInfo(_connectionString, database);
        }


    }
}
