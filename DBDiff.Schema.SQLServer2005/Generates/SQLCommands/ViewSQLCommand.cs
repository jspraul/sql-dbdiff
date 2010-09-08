
using System.Text;
using DBDiff.Schema.SQLServer.Generates.Model;

namespace DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands
{
	internal static class ViewSQLCommand
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
			sql.Append("select distinct ISNULL('[' + S3.Name + '].[' + O2.name + ']','') AS DependOut, '[' + S2.Name + '].[' + O.name + ']' AS TableName, D.depid as referenced_major_id, OBJECTPROPERTY (P.id,'IsSchemaBound') AS IsSchemaBound, P.id as object_id, S.name as owner, P.name as name ");
			sql.Append("from sysobjects P ");
			sql.Append("INNER JOIN sysusers S ON S.uid = P.uid ");
			sql.Append("LEFT JOIN sysdepends D ON P.id = D.id  ");
			sql.Append("LEFT JOIN sysobjects O ON O.id = D.depid ");
			sql.Append("LEFT JOIN sysusers S2 ON S2.uid = O.uid ");
			sql.Append("LEFT JOIN sysdepends D2 ON P.id = D2.depid ");
			sql.Append("LEFT JOIN sysobjects O2 ON O2.id = D2.id ");
			sql.Append("LEFT JOIN sysusers S3 ON S3.uid = O2.uid ");
			sql.Append("where p.type='V' and P.category <>2");
			sql.Append("ORDER BY P.id");
			return sql.ToString();
		}

		private static string Get2005()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select distinct ISNULL('[' + S3.Name + '].[' + object_name(D2.object_id) + ']','') AS DependOut, '[' + S2.Name + '].[' + object_name(D.referenced_major_id) + ']' AS TableName, D.referenced_major_id, OBJECTPROPERTY (P.object_id,'IsSchemaBound') AS IsSchemaBound, P.object_id, S.name as owner, P.name as name from sys.views P ");
			sql.Append("INNER JOIN sys.schemas S ON S.schema_id = P.schema_id ");
			sql.Append("LEFT JOIN sys.sql_dependencies D ON P.object_id = D.object_id ");
			sql.Append("LEFT JOIN sys.objects O ON O.object_id = D.referenced_major_id ");
			sql.Append("LEFT JOIN sys.schemas S2 ON S2.schema_id = O.schema_id ");
			sql.Append("LEFT JOIN sys.sql_dependencies D2 ON P.object_id = D2.referenced_major_id ");
			sql.Append("LEFT JOIN sys.objects O2 ON O2.object_id = D2.object_id ");
			sql.Append("LEFT JOIN sys.schemas S3 ON S3.schema_id = O2.schema_id ");
			sql.Append("ORDER BY P.object_id");
			return sql.ToString();
		}

		private static string Get2008()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select distinct ISNULL('[' + S3.Name + '].[' + object_name(D2.object_id) + ']','') AS DependOut, '[' + S2.Name + '].[' + object_name(D.referenced_major_id) + ']' AS TableName, D.referenced_major_id, OBJECTPROPERTY (P.object_id,'IsSchemaBound') AS IsSchemaBound, P.object_id, S.name as owner, P.name as name from sys.views P ");
			sql.Append("INNER JOIN sys.schemas S ON S.schema_id = P.schema_id ");
			sql.Append("LEFT JOIN sys.sql_dependencies D ON P.object_id = D.object_id ");
			sql.Append("LEFT JOIN sys.objects O ON O.object_id = D.referenced_major_id ");
			sql.Append("LEFT JOIN sys.schemas S2 ON S2.schema_id = O.schema_id ");
			sql.Append("LEFT JOIN sys.sql_dependencies D2 ON P.object_id = D2.referenced_major_id ");
			sql.Append("LEFT JOIN sys.objects O2 ON O2.object_id = D2.object_id ");
			sql.Append("LEFT JOIN sys.schemas S3 ON S3.schema_id = O2.schema_id ");
			sql.Append("ORDER BY P.object_id");
			return sql.ToString();
		}
	}
}
