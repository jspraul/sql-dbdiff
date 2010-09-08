
using System.Text;
using DBDiff.Schema.SQLServer.Generates.Model;

namespace DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands
{
	internal static class SchemaSQLCommand
	{
		public static string Get(DatabaseInfo.VersionNumber version)
		{
			if (version == DatabaseInfo.VersionNumber.SQLServer2000) return Get2000();
			if (version == DatabaseInfo.VersionNumber.SQLServer2005) return Get2005();
			if (version == DatabaseInfo.VersionNumber.SQLServer2008) return Get2008();
			return "";
		}

		private static string Get2000()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select name,convert(int,uid) as schema_id,name AS Owner from sysusers where uid>0");
			return sql.ToString();
		}

		private static string Get2005()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select S1.name,S1.schema_id, S2.name AS Owner from sys.schemas S1 ");
			sql.Append("INNER JOIN sys.database_principals S2 ON S2.principal_id = S1.principal_id ");
			return sql.ToString();
		}

		private static string Get2008()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select S1.name,S1.schema_id, S2.name AS Owner from sys.schemas S1 ");
			sql.Append("INNER JOIN sys.database_principals S2 ON S2.principal_id = S1.principal_id ");
			return sql.ToString();
		}
	}
}
