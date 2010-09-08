using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DBDiff.Schema.Errors;
using DBDiff.Schema.Events;
using DBDiff.Schema.Model;
using DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands;
using DBDiff.Schema.SQLServer.Generates.Generates.Util;
using DBDiff.Schema.SQLServer.Generates.Model;

namespace DBDiff.Schema.SQLServer.Generates.Generates
{
    public class GenerateUserDataTypes
    {
        private readonly Generate root;

        public GenerateUserDataTypes(Generate root)
        {
            this.root = root;
        }

        private static string GetSQLColumnsDependencis2000()
        {
            string sql = "SELECT TT.type, 0 AS IsComputed, T.xusertype as user_type_id,'[' + S.Name + '].[' + TT.Name + ']' AS TableName, '[' + S.Name + '].[' + TT.Name + '].[' + C.Name + ']' AS ColumnName,'[' + S2.Name + '].[' + T.Name + ']' AS TypeName FROM systypes T ";
            sql += "INNER JOIN syscolumns C ON C.xusertype = T.xusertype ";
            sql += "INNER JOIN sysobjects TT ON TT.id = C.id ";
            sql += "INNER JOIN sysusers S ON S.uid = TT.uid ";
            sql += "INNER JOIN sysusers S2 ON S2.uid = T.uid ";
            sql += "WHERE T.xusertype>256 ";
            sql += "UNION ";
            sql += "SELECT TT.type, 1 AS IsComputed, T.xusertype as user_type_id, '[' + S.Name + '].[' + TT.Name + ']' AS TableName, '[' + S.Name + '].[' + TT.Name + '].[' + C2.Name + ']' AS ColumnName, '[' + S2.Name + '].[' + T.Name + ']' AS TypeName FROM systypes T ";
            sql += "INNER JOIN syscolumns C ON C.xusertype = T.xusertype ";
            sql += "INNER JOIN sysdepends DEP ON DEP.depid = C.id AND DEP.depnumber = C.colId AND DEP.id = C.id ";
            sql += "INNER JOIN syscolumns C2 ON C2.colId = DEP.number AND C2.id = DEP.id ";
            sql += "INNER JOIN sysobjects TT ON TT.id = C2.id ";
            sql += "INNER JOIN sysusers S ON S.uid = TT.uid ";
            sql += "INNER JOIN sysusers S2 ON S2.uid = T.uid ";
            sql += "WHERE T.xusertype>256 ";
            sql += "UNION ";
            sql += "SELECT TT.type, 0 AS IsComputed, T.xusertype as user_type_id,'[' + S.Name + '].[' + TT.Name + ']' AS TableName, '[' + S.Name + '].[' + TT.Name + '].[' + C.Name + ']' AS ColumnName,'[' + S2.Name + '].[' + T.Name + ']' AS TypeName from sysdepends DEP ";
            sql += "INNER JOIN sysobjects TT ON DEP.id = TT.id ";
            sql += "INNER JOIN sysusers S ON S.uid = TT.uid ";
            sql += "INNER JOIN syscolumns C ON C.id = TT.id AND C.colid = DEP.depnumber ";
            sql += "INNER JOIN systypes T ON C.xusertype = T.xusertype ";
            sql += "INNER JOIN sysusers S2 ON S2.uid = T.uid ";
            sql += "WHERE T.xusertype>256 ";
            sql += "ORDER BY IsComputed DESC,T.xusertype";
            return sql;
        }

        private static string GetSQLColumnsDependencis()
        {
            string sql = "SELECT TT.type, 0 AS IsComputed, T.user_type_id,'[' + S.Name + '].[' + TT.Name + ']' AS TableName, '[' + S.Name + '].[' + TT.Name + '].[' + C.Name + ']' AS ColumnName,'[' + S2.Name + '].[' + T.Name + ']' AS TypeName FROM sys.types T ";
            sql += "INNER JOIN sys.columns C ON C.user_type_id = T.user_type_id ";
            sql += "INNER JOIN sys.objects TT ON TT.object_id = C.object_id ";
            sql += "INNER JOIN sys.schemas S ON S.schema_id = TT.schema_id ";
            sql += "INNER JOIN sys.schemas S2 ON S2.schema_id = T.schema_id ";
            sql += "WHERE is_user_defined = 1 ";
            sql += "UNION ";
            sql += "SELECT TT.type, 1 AS IsComputed, T.user_type_id, '[' + S.Name + '].[' + TT.Name + ']' AS TableName, '[' + S.Name + '].[' + TT.Name + '].[' + C2.Name + ']' AS ColumnName, '[' + S2.Name + '].[' + T.Name + ']' AS TypeName FROM sys.types T ";
            sql += "INNER JOIN sys.columns C ON C.user_type_id = T.user_type_id ";
            sql += "INNER JOIN sys.sql_dependencies DEP ON DEP.referenced_major_id = C.object_id AND DEP.referenced_minor_id = C.column_Id AND DEP.object_id = C.object_id ";
            sql += "INNER JOIN sys.columns C2 ON C2.column_id = DEP.column_id AND C2.object_id = DEP.object_id ";
            sql += "INNER JOIN sys.objects TT ON TT.object_id = C2.object_id ";
            sql += "INNER JOIN sys.schemas S ON S.schema_id = TT.schema_id ";
            sql += "INNER JOIN sys.schemas S2 ON S2.schema_id = T.schema_id ";
            sql += "WHERE is_user_defined = 1 ";

            sql += "UNION ";
            sql += "SELECT TT.type, 0 AS IsComputed, T.user_type_id,'[' + S.Name + '].[' + TT.Name + ']' AS TableName, '[' + S.Name + '].[' + TT.Name + '].[' + C.Name + ']' AS ColumnName,'[' + S2.Name + '].[' + T.Name + ']' AS TypeName from sys.sql_dependencies DEP ";            
            sql += "INNER JOIN sys.objects TT ON DEP.object_id = TT.object_id ";
            sql += "INNER JOIN sys.schemas S ON S.schema_id = TT.schema_id ";
            sql += "INNER JOIN sys.parameters C ON C.object_id = TT.object_id AND C.parameter_id = DEP.referenced_minor_id ";
            sql += "INNER JOIN sys.types T ON C.user_type_id = T.user_type_id ";
            sql += "INNER JOIN sys.schemas S2 ON S2.schema_id = T.schema_id ";
            sql += "WHERE is_user_defined = 1 ";
            sql += "ORDER BY IsComputed DESC,T.user_type_id";
            return sql;
        }

        private static void FillColumnsDependencies(DatabaseInfo.VersionNumber DatabaseVersion,SchemaList<UserDataType, Database> types, string connectionString)
        {
            if (types == null) throw new ArgumentNullException("types");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand((DatabaseVersion==DatabaseInfo.VersionNumber.SQLServer2000?GetSQLColumnsDependencis2000(): GetSQLColumnsDependencis()), conn))
                {
                    conn.Open();
                    command.CommandTimeout = 0;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            types[reader["TypeName"].ToString()].Dependencys.Add(new ObjectDependency(reader["TableName"].ToString(), reader["ColumnName"].ToString(), ConvertType.GetObjectType(reader["Type"].ToString())));
                        }
                    }
                }
            }
        }

        public void Fill(Database database, string connectionString, List<MessageLog> messages)
        {
            try
            {
                if (database.Options.Ignore.FilterUserDataType)
                {
                    root.RaiseOnReading(new ProgressEventArgs("Reading UDT...", Constants.READING_UDT));
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(UserDataTypeCommand.Get(database.Info.Version), conn))
                        {
                            conn.Open();
                            command.CommandTimeout = 0;
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    UserDataType item = new UserDataType(database);
                                    item.Id = (int)reader["tid"];
                                    item.AllowNull = (bool)reader["is_nullable"];
                                    item.Size = (short)reader["max_length"];
                                    item.Name = reader["Name"].ToString();
                                    item.Owner = reader["owner"].ToString();
                                    item.Precision = int.Parse(reader["precision"].ToString());
                                    item.Scale = int.Parse(reader["scale"].ToString());
                                    if (!String.IsNullOrEmpty(reader["defaultname"].ToString()))
                                    {
                                        item.Default.Name = reader["defaultname"].ToString();
                                        item.Default.Owner = reader["defaultowner"].ToString();
                                    }
                                    if (!String.IsNullOrEmpty(reader["rulename"].ToString()))
                                    {
                                        item.Rule.Name = reader["rulename"].ToString();
                                        item.Rule.Owner = reader["ruleowner"].ToString();
                                    }
                                    item.Type = reader["basetypename"].ToString();
                                    item.IsAssembly = (bool)reader["is_assembly_type"];
                                    item.AssemblyId = (int)reader["assembly_id"];
                                    item.AssemblyName = reader["assembly_name"].ToString();
                                    item.AssemblyClass = reader["assembly_class"].ToString();
                                    database.UserTypes.Add(item);
                                }
                            }
                        }
                    }
                    if (database.Options.Ignore.FilterTable)
                        FillColumnsDependencies(database.Info.Version,database.UserTypes, connectionString);
                        
                }
            }
            catch (Exception ex)
            {
                messages.Add(new MessageLog(ex.Message, ex.StackTrace, MessageLog.LogType.Error));
            }
        }
    }
}
