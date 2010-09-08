
using DBDiff.Schema.SQLServer.Generates.Model;

namespace DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands
{
	internal static class FunctionSQLCommand
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
			string sql = "";
			sql += "select distinct ";
			sql += "NULL AS ReturnType, NULL as max_length, NULL as [precision], NULL as Scale, ";
			sql += "'CALLER' as ExecuteAs, ";
			sql += "P.type, ";
			sql += "NULL AS assembly_name, ";
			sql += "NULL AS assembly_class, ";
			sql += "NULL AS assembly_id, ";
			sql += "NULL AS assembly_method, ";
			sql += "ISNULL('[' + S3.Name + '].[' + O2.name + ']','') AS DependOut, '[' + S2.Name + '].[' + O.name + ']' AS TableName, D.depid as referenced_major_id, OBJECTPROPERTY (P.id,'IsSchemaBound') AS IsSchemaBound, P.id as object_id, S.name as owner, P.name as name ";
			sql += "FROM sysobjects P ";
			sql += "INNER JOIN sysusers S ON S.uid = P.uid ";
			sql += "LEFT JOIN sysdepends D ON P.id = D.id ";
			sql += "LEFT JOIN sysobjects O ON O.id = D.depid ";
			sql += "LEFT JOIN sysusers S2 ON S2.uid = O.uid ";
			sql += "LEFT JOIN sysdepends D2 ON P.id = D2.depid ";
			sql += "LEFT JOIN sysobjects O2 ON O2.id = D2.id ";
			sql += "LEFT JOIN sysusers S3 ON S3.uid = O2.uid ";
			sql += "WHERE P.type IN ('IF','FN','TF','FS','AF') ORDER BY P.id";
			return sql;
		}
		
		private static string Get2005()
		{
			string sql = "";
			sql += "select distinct ";
			sql += "T.name AS ReturnType, PP.max_length, PP.precision, PP.Scale, ";
			sql += "ISNULL(CONVERT(varchar,AM.execute_as_principal_id),'CALLER') as ExecuteAs, ";
			sql += "P.type, ";
			sql += "AF.name AS assembly_name, ";
			sql += "AM.assembly_class, ";
			sql += "AM.assembly_id, ";
			sql += "AM.assembly_method, ";
			sql += "ISNULL('[' + S3.Name + '].[' + object_name(D2.object_id) + ']','') AS DependOut, '[' + S2.Name + '].[' + object_name(D.referenced_major_id) + ']' AS TableName, D.referenced_major_id, OBJECTPROPERTY (P.object_id,'IsSchemaBound') AS IsSchemaBound, P.object_id, S.name as owner, P.name as name from sys.objects P ";
			sql += "INNER JOIN sys.schemas S ON S.schema_id = P.schema_id ";
			sql += "LEFT JOIN sys.sql_dependencies D ON P.object_id = D.object_id ";
			sql += "LEFT JOIN sys.objects O ON O.object_id = D.referenced_major_id ";
			sql += "LEFT JOIN sys.schemas S2 ON S2.schema_id = O.schema_id ";
			sql += "LEFT JOIN sys.sql_dependencies D2 ON P.object_id = D2.referenced_major_id ";
			sql += "LEFT JOIN sys.objects O2 ON O2.object_id = D2.object_id ";
			sql += "LEFT JOIN sys.schemas S3 ON S3.schema_id = O2.schema_id ";
			sql += "LEFT JOIN sys.assembly_modules AM ON AM.object_id = P.object_id  ";
			sql += "LEFT JOIN sys.assemblies AF ON AF.assembly_id = AM.assembly_id ";
			sql += "LEFT JOIN sys.parameters PP ON PP.object_id = AM.object_id AND PP.parameter_id = 0 and PP.is_output = 1 ";
			sql += "LEFT JOIN sys.types T ON T.system_type_id = PP.system_type_id ";
			sql += "WHERE P.type IN ('IF','FN','TF','FS','AF') ORDER BY P.object_id";
			return sql;
		}

		private static string Get2008()
		{
			string sql = "";
			sql += "select distinct ";
			sql += "T.name AS ReturnType, PP.max_length, PP.precision, PP.Scale, ";
			sql += "ISNULL(CONVERT(varchar,AM.execute_as_principal_id),'CALLER') as ExecuteAs, ";
			sql += "P.type, ";
			sql += "AF.name AS assembly_name, ";
			sql += "AM.assembly_class, ";
			sql += "AM.assembly_id, ";
			sql += "AM.assembly_method, ";
			sql += "ISNULL('[' + S3.Name + '].[' + object_name(D2.object_id) + ']','') AS DependOut, '[' + S2.Name + '].[' + object_name(D.referenced_major_id) + ']' AS TableName, D.referenced_major_id, OBJECTPROPERTY (P.object_id,'IsSchemaBound') AS IsSchemaBound, P.object_id, S.name as owner, P.name as name from sys.objects P ";
			sql += "INNER JOIN sys.schemas S ON S.schema_id = P.schema_id ";
			sql += "LEFT JOIN sys.sql_dependencies D ON P.object_id = D.object_id ";
			sql += "LEFT JOIN sys.objects O ON O.object_id = D.referenced_major_id ";
			sql += "LEFT JOIN sys.schemas S2 ON S2.schema_id = O.schema_id ";
			sql += "LEFT JOIN sys.sql_dependencies D2 ON P.object_id = D2.referenced_major_id ";
			sql += "LEFT JOIN sys.objects O2 ON O2.object_id = D2.object_id ";
			sql += "LEFT JOIN sys.schemas S3 ON S3.schema_id = O2.schema_id ";
			sql += "LEFT JOIN sys.assembly_modules AM ON AM.object_id = P.object_id  ";
			sql += "LEFT JOIN sys.assemblies AF ON AF.assembly_id = AM.assembly_id ";
			sql += "LEFT JOIN sys.parameters PP ON PP.object_id = AM.object_id AND PP.parameter_id = 0 and PP.is_output = 1 ";
			sql += "LEFT JOIN sys.types T ON T.system_type_id = PP.system_type_id ";
			sql += "WHERE P.type IN ('IF','FN','TF','FS','AF') ORDER BY P.object_id";
			return sql;
		}
	}
}
