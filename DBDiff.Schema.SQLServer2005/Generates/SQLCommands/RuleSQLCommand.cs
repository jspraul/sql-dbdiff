
using System.Text;
using DBDiff.Schema.SQLServer.Generates.Model;

namespace DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands
{
	internal static class RuleSQLCommand
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
			sql.Append("select obj.id as object_id,obj.name as Name, su.name AS Owner, smobj.text AS [Definition] from sysobjects obj  ");
			sql.Append("LEFT OUTER JOIN syscomments AS smobj ON smobj.id = obj.id ");
			sql.Append("left join sysusers su on obj.uid=su.uid ");
			sql.Append("where obj.type='R'");            
			return sql.ToString();
		}

		private static string Get2005()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select obj.object_id, Name, SCHEMA_NAME(obj.schema_id) AS Owner, ISNULL(smobj.definition, ssmobj.definition) AS [Definition] from sys.objects obj  ");
			sql.Append("LEFT OUTER JOIN sys.sql_modules AS smobj ON smobj.object_id = obj.object_id ");
			sql.Append("LEFT OUTER JOIN sys.system_sql_modules AS ssmobj ON ssmobj.object_id = obj.object_id ");
			sql.Append("where obj.type='R'");
			return sql.ToString();
		}

		private static string Get2008()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select obj.object_id, Name, SCHEMA_NAME(obj.schema_id) AS Owner, ISNULL(smobj.definition, ssmobj.definition) AS [Definition] from sys.objects obj  ");
			sql.Append("LEFT OUTER JOIN sys.sql_modules AS smobj ON smobj.object_id = obj.object_id ");
			sql.Append("LEFT OUTER JOIN sys.system_sql_modules AS ssmobj ON ssmobj.object_id = obj.object_id ");
			sql.Append("where obj.type='R'");
			return sql.ToString();
		}
	}
}
