using System;
using System.Data.SqlClient;
using DBDiff.Schema.SQLServer.Generates.Model;

namespace DBDiff.Schema.SQLServer
{
    internal class Util
    {
        internal static DatabaseInfo.VersionNumber GetVersionNumber(SqlConnection connection)
        {
            //Default to 2000
            DatabaseInfo.VersionNumber version = DatabaseInfo.VersionNumber.SQLServer2000;

            if (connection.State == System.Data.ConnectionState.Open)
            {
                string[] versionNumbers = connection.ServerVersion.Split('.');
                version = (DatabaseInfo.VersionNumber)Convert.ToInt32(versionNumbers[0]);
            }

            return version;
        }
    }
}
