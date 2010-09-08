using System.Data.SqlClient;
using DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands;
using System;

namespace DBDiff.Schema.SQLServer.Generates.Model
{
    
    public class DatabaseInfo
    {
        public enum VersionNumber
        {
            SQLServer2000 = 8,
            SQLServer2005 = 9,
            SQLServer2008 = 10
        }
        
        private string _connectionString;
        private Database _database;

        public DatabaseInfo(string connectionString, Database database)
        {
            Version = VersionNumber.SQLServer2005;
            this._connectionString = connectionString;
            this._database = database;

            //Initialise all the properties of this database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Set the version number
                this.Version = DBDiff.Schema.SQLServer.Util.GetVersionNumber(conn);

                using (SqlCommand command = new SqlCommand(DatabaseSQLCommand.GetDatabaseProperties(this.Version, this._database), conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            this.Collation = reader["Collation"].ToString();
                            this.HasFullTextEnabled = ((int)reader["IsFulltextEnabled"]) == 1;
                            this.CompatibilityLevel = reader["CompatibilityLevel"].ToString();
                        }
                    }

                }
            }
        }

        public VersionNumber Version { get; private set; }

        public string Collation { get; private set; }

        public string CompatibilityLevel { get; private set; }

        public bool HasFullTextEnabled { get; private set; }

        public string ChangeTrackingPeriodUnitsDesc { get; private set; }

        public int ChangeTrackingPeriodUnits { get; private set; }

        public int ChangeTrackingRetentionPeriod { get; private set; }

        public bool IsChangeTrackingAutoCleanup { get; private set; }

        public bool HasChangeTracking { get; private set; }
    }
}
