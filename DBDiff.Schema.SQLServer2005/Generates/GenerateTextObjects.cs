using System;
using System.Data.SqlClient;
using DBDiff.Schema.Events;
using DBDiff.Schema.SQLServer.Generates.Generates.Util;
using DBDiff.Schema.SQLServer.Generates.Model;
using DBDiff.Schema.SQLServer.Generates.Options;
using DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands;

namespace DBDiff.Schema.SQLServer.Generates.Generates
{
    public class GenerateTextObjects
    {
        private Generate root;

        public GenerateTextObjects(Generate root)
        {
            this.root = root;
        }

        //private static string GetSQL(SqlOption options)
        //{
        //    string filter = "";
        //    string sql = "";
        //    sql += "SELECT O.type, M.object_id, OBJECT_DEFINITION(M.object_id) AS Text FROM sys.sql_modules M ";
        //    sql += "INNER JOIN sys.objects O ON O.object_id = M.object_id ";
        //    sql += "WHERE ";
        //    if (options.Ignore.FilterStoreProcedure)
        //        filter += "O.type = 'P' OR ";
        //    if (options.Ignore.FilterView)
        //        filter += "O.type = 'V' OR ";
        //    if (options.Ignore.FilterTrigger)
        //        filter += "O.type = 'TR' OR ";
        //    if (options.Ignore.FilterFunction)
        //        filter += "O.type IN ('IF','FN','TF') OR ";
        //    filter = filter.Substring(0, filter.Length - 4);
        //    return sql + filter;
        //}

        public void Fill(Database database, string connectionString)
        {
            ICode code = null;
            try
            {
                if ((database.Options.Ignore.FilterStoreProcedure) || (database.Options.Ignore.FilterView) || (database.Options.Ignore.FilterFunction) || (database.Options.Ignore.FilterTrigger))
                {
                    root.RaiseOnReading(new ProgressEventArgs("Reading Text Objects...", Constants.READING_TEXTOBJECTS));
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string filter = "";
                        if (database.Options.Ignore.FilterStoreProcedure)
                            filter += "O.type = 'P' OR ";
                        if (database.Options.Ignore.FilterView)
                            filter += "O.type = 'V' OR ";
                        if (database.Options.Ignore.FilterTrigger)
                            filter += "O.type = 'TR' OR ";
                        if (database.Options.Ignore.FilterFunction)
                            filter += "O.type IN ('IF','FN','TF') OR ";
                        filter = filter.Substring(0, filter.Length - 4);
                        using (SqlCommand command = new SqlCommand(TextObjectSQLCommand.Get(database.Info.Version,filter), conn))
                        {
                            conn.Open();
                            command.CommandTimeout = 0;
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (database.Info.Version == DatabaseInfo.VersionNumber.SQLServer2000)
                                {
                                    bool end=false;
                                    bool flag = true;
                                    int newid;
                                    end=!reader.Read();
                                    while (!end)
                                    {

                                        flag = true;
                                            string type = reader["Type"].ToString().Trim();
                                            int id = (int)reader["object_id"];
                                            if (type.Equals("V"))
                                                code = (ICode)database.Views.Find(id);

                                            if (type.Equals("TR"))
                                                code = (ICode)database.Find(id);

                                            if (type.Equals("P"))
                                                code = (ICode)database.Procedures.Find(id);

                                            if (type.Equals("IF") || type.Equals("FN") || type.Equals("TF") || type.Equals("AF"))
                                                code = (ICode)database.Functions.Find(id);
                                            //code.Text = reader["Text"].ToString();
                                            //newid = id;
                                            if (code != null)
                                                while (flag && !end)
                                                {  
                                                    newid = (int)reader["object_id"];
                                                    if (newid != id) { flag = false; continue; }                                                   
                                                    code.Text += reader["Text"].ToString();
                                                    end = !reader.Read();                                                    
                                                }
                                            else end = !reader.Read();  
                                    }
                                }
                                else
                                while (reader.Read())
                                {
                                    string type = reader["Type"].ToString().Trim();
                                    int id = (int)reader["object_id"];
                                    if (type.Equals("V"))
                                        code = (ICode)database.Views.Find(id);

                                    if (type.Equals("TR"))
                                        code = (ICode)database.Find(id);

                                    if (type.Equals("P"))
                                        code = (ICode)database.Procedures.Find(id);

                                    if (type.Equals("IF") || type.Equals("FN") || type.Equals("TF") || type.Equals("AF"))
                                        code = (ICode)database.Functions.Find(id);

                                    if (code != null)
                                        
                                        code.Text = reader["Text"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
