using System.Collections.Generic;
using System.Data;
using System.Data.Sql;

namespace DBDiff.Schema.SQLServer.Generates.Front.Util
{
    internal static class SqlServerList
    {
       public static List<string> Get()
       {
             SqlDataSourceEnumerator sqlSource = SqlDataSourceEnumerator.Instance;
             DataTable dt = sqlSource.GetDataSources();

             List<string> serverList = new List<string>();

             foreach(DataRow dr in dt.Rows)
             {
                     serverList.Add(dr["ServerName"].ToString());
             }

             return serverList;
       }
    }
}
