using System.Data.SqlClient;
using DBDiff.Schema.SQLServer.Generates.Model;
using DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands;

namespace DBDiff.Schema.SQLServer.Generates.Generates
{
    public class GenerateSchemas
    {
        private Generate root;

        public GenerateSchemas(Generate root)
        {
            this.root = root;
        }
        
        //private static string GetSQL()
        //{
        //    string sql;
        //    sql = "select S1.name,S1.schema_id, S2.name AS Owner from sys.schemas S1 ";
        //    sql += "INNER JOIN sys.database_principals S2 ON S2.principal_id = S1.principal_id ";
        //    return sql;
        //}

        public void Fill(Database database, string connectioString)
        {
            if (database.Info.Version == DatabaseInfo.VersionNumber.SQLServer2000) return;
            if (database.Options.Ignore.FilterSchema)
            {
                using (SqlConnection conn = new SqlConnection(connectioString))
                {
                    using (SqlCommand command = new SqlCommand(SchemaSQLCommand.Get(database.Info.Version), conn))
                    {
                        conn.Open();
                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Model.Schema item = new Model.Schema(database);
                                item.Id = (int)reader["schema_id"];
                                item.Name = reader["name"].ToString();
                                item.Owner = reader["owner"].ToString();
                                database.Schemas.Add(item);
                            }
                        }
                    }
                }
            }
        }
    }
}
