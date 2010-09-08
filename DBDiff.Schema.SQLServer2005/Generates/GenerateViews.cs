using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DBDiff.Schema.Errors;
using DBDiff.Schema.Events;
using DBDiff.Schema.SQLServer.Generates.Generates.Util;
using DBDiff.Schema.SQLServer.Generates.Model;
using DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands;

namespace DBDiff.Schema.SQLServer.Generates.Generates
{
    public class GenerateViews
    {
        private Generate root;

        public GenerateViews(Generate root)
        {
            this.root = root;
        }

        //private static string GetSQL()
        //{
        //    string sql = "";
        //    sql += "select distinct ISNULL('[' + S3.Name + '].[' + object_name(D2.object_id) + ']','') AS DependOut, '[' + S2.Name + '].[' + object_name(D.referenced_major_id) + ']' AS TableName, D.referenced_major_id, OBJECTPROPERTY (P.object_id,'IsSchemaBound') AS IsSchemaBound, P.object_id, S.name as owner, P.name as name from sys.views P ";
        //    sql += "INNER JOIN sys.schemas S ON S.schema_id = P.schema_id ";
        //    sql += "LEFT JOIN sys.sql_dependencies D ON P.object_id = D.object_id ";
        //    sql += "LEFT JOIN sys.objects O ON O.object_id = D.referenced_major_id ";
        //    sql += "LEFT JOIN sys.schemas S2 ON S2.schema_id = O.schema_id ";
        //    sql += "LEFT JOIN sys.sql_dependencies D2 ON P.object_id = D2.referenced_major_id ";
        //    sql += "LEFT JOIN sys.objects O2 ON O2.object_id = D2.object_id ";
        //    sql += "LEFT JOIN sys.schemas S3 ON S3.schema_id = O2.schema_id ";
        //    sql += "ORDER BY P.object_id";
        //    return sql;
        //}

        public void Fill(Database database, string connectionString, List<MessageLog> messages)
        {
            try
            {
                root.RaiseOnReading(new ProgressEventArgs("Reading views...", Constants.READING_VIEWS));
                if (database.Options.Ignore.FilterView)
                {
                    FillView(database,connectionString);
                }
            }
            catch (Exception ex)
            {
                messages.Add(new MessageLog(ex.Message, ex.StackTrace, MessageLog.LogType.Error));
            }
        }

        private void FillView(Database database, string connectionString)
        {
            int lastViewId = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(ViewSQLCommand.Get(database.Info.Version), conn))
                {
                    conn.Open();
                    command.CommandTimeout = 0;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        View item = null;                        
                        while (reader.Read())
                        {
                            if (lastViewId != (int)reader["object_id"])
                            {
                                item = new View(database);
                                item.Id = (int)reader["object_id"];
                                item.Name = reader["name"].ToString();
                                item.Owner = reader["owner"].ToString();
                                item.IsSchemaBinding = reader["IsSchemaBound"].ToString().Equals("1");
                                database.Views.Add(item);
                                lastViewId = item.Id;
                            }
                            if (item.IsSchemaBinding)
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("referenced_major_id")))
                                    database.Dependencies.Add(database,(int)reader["referenced_major_id"], item);
                                if (!String.IsNullOrEmpty(reader["TableName"].ToString()))
                                    item.DependenciesIn.Add(reader["TableName"].ToString());
                                if (!String.IsNullOrEmpty(reader["DependOut"].ToString()))
                                    item.DependenciesOut.Add(reader["DependOut"].ToString());
                            }                            
                        }
                    }
                }                    
            }
        }
    }
}
