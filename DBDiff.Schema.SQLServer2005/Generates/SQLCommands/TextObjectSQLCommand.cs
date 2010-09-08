
using System.Text;
using DBDiff.Schema.SQLServer.Generates.Model;

namespace DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands
{
	internal static class TextObjectSQLCommand
	{
		public static string Get(DatabaseInfo.VersionNumber version,string filter)
		{
			if (version == DatabaseInfo.VersionNumber.SQLServer2000) return Get2000(filter);
			if (version == DatabaseInfo.VersionNumber.SQLServer2005) return Get2005(filter);
			if (version == DatabaseInfo.VersionNumber.SQLServer2008) return Get2008(filter);
			return "";
		}

		private static string Get2000(string filter)
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select O.type,C.id as object_id,text ");
			sql.Append("FROM sysobjects O inner join syscomments C on O.id=c.id ");
			sql.Append("WHERE ");
			sql.Append(filter);
			sql.Append(" order by c.id, colid");
			return sql.ToString();
		}

		private static string Get2005(string filter)
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT O.type, M.object_id, OBJECT_DEFINITION(M.object_id) AS Text FROM sys.sql_modules M ");
			sql.Append("INNER JOIN sys.objects O ON O.object_id = M.object_id ");
			sql.Append("WHERE ");
			sql.Append(filter);
			return sql.ToString();
		}

		private static string Get2008(string filter)
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT O.type, M.object_id, OBJECT_DEFINITION(M.object_id) AS Text FROM sys.sql_modules M ");
			sql.Append("INNER JOIN sys.objects O ON O.object_id = M.object_id ");
			sql.Append("WHERE ");
			sql.Append(filter);
			return sql.ToString();
		}
	}
}
