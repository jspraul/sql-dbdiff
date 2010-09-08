
using System.Text;
using DBDiff.Schema.SQLServer.Generates.Model;

namespace DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands
{
	internal static class UserSQLCommand
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
			sql.Append("select convert(bit, case when u.uid >= 16384 and u.uid < 16400 then 1 else 0 end)  AS is_fixed_role,(case when issqluser=1 then 'S' when issqlrole=1 then 'R' when isapprole=1 then 'A' when isntuser=1 then 'U'end) as type,ISNULL(L.name,'') AS Login,u.name,convert(int,U.uid) as principal_id, '' AS default_schema_name from sysusers u left join master..sysxlogins L on(u.sid=L.sid) ");
			sql.Append("WHERE issqluser=1 or issqlrole=1 or isapprole=1 or isntuser=1 ");
			sql.Append("ORDER BY U.Name");
			return sql.ToString();
		}

		private static string Get2005()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select is_fixed_role, type, ISNULL(suser_sname(sid),'') AS Login,Name,principal_id, ISNULL(default_schema_name,'') AS default_schema_name from sys.database_principals ");
			sql.Append("WHERE type IN ('S','U','A','R') ");
			sql.Append("ORDER BY Name");
			return sql.ToString();
		}

		private static string Get2008()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select is_fixed_role, type, ISNULL(suser_sname(sid),'') AS Login,Name,principal_id, ISNULL(default_schema_name,'') AS default_schema_name from sys.database_principals ");
			sql.Append("WHERE type IN ('S','U','A','R') ");
			sql.Append("ORDER BY Name");
			return sql.ToString();
		}
	}
}
