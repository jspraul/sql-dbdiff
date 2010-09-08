
using System.Text;
using DBDiff.Schema.SQLServer.Generates.Model;

namespace DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands
{
	internal static class StoreProcedureSQLCommand
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
			sql.Append("select 'CALLER' as ExecuteAs,O.type, null AS assembly_name, null AS assembly_class, null as  assembly_id, null as assembly_method, O.id as object_id, S.name as owner, O.name as name ");
			sql.Append("FROM sysobjects O inner join sysusers S on(O.uid=S.uid) ");
			sql.Append("where type ='P'");
			return sql.ToString();
		}

		private static string Get2005()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select ISNULL(CONVERT(varchar,AM.execute_as_principal_id),'CALLER') as ExecuteAs, P.type, AF.name AS assembly_name, AM.assembly_class, AM.assembly_id, AM.assembly_method, P.object_id, S.name as owner, P.name as name from sys.procedures P ");
			sql.Append("INNER JOIN sys.schemas S ON S.schema_id = P.schema_id ");
			sql.Append("LEFT JOIN sys.assembly_modules AM ON AM.object_id = P.object_id ");
			sql.Append("LEFT JOIN sys.assemblies AF ON AF.assembly_id = AM.assembly_id");
			return sql.ToString();
		}

		private static string Get2008()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select ISNULL(CONVERT(varchar,AM.execute_as_principal_id),'CALLER') as ExecuteAs, P.type, AF.name AS assembly_name, AM.assembly_class, AM.assembly_id, AM.assembly_method, P.object_id, S.name as owner, P.name as name from sys.procedures P ");
			sql.Append("INNER JOIN sys.schemas S ON S.schema_id = P.schema_id ");
			sql.Append("LEFT JOIN sys.assembly_modules AM ON AM.object_id = P.object_id ");
			sql.Append("LEFT JOIN sys.assemblies AF ON AF.assembly_id = AM.assembly_id");
			return sql.ToString();
		}

		public static string GetParameters(DatabaseInfo.VersionNumber version)
		{
			if (version == DatabaseInfo.VersionNumber.SQLServer2000) return GetParameters2000();
			if (version == DatabaseInfo.VersionNumber.SQLServer2005) return GetParameters2005();
			if (version == DatabaseInfo.VersionNumber.SQLServer2008) return GetParameters2008();
			return "";
		}

		private static string GetParameters2000()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select isoutparam as is_output ,P.xscale as scale,P.xprec as precision,'[' + S.name + '].['+  O.name + ']' AS ObjectName,P.name,T.name AS TypeName, P.length as max_length ");
			sql.Append("from sysobjects O inner join sysusers S on(O.uid=S.uid) ");
			sql.Append("inner join syscolumns P on O.id=P.id ");
			sql.Append("inner join systypes T on (T.xusertype = P.xusertype) ");
			sql.Append("where O.type ='PC' ");
			sql.Append("ORDER BY O.id, P.id");
			return sql.ToString();
		}

		private static string GetParameters2005()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select AP.is_output, AP.scale, AP.precision, '[' + SCHEMA_NAME(O.schema_id) + '].['+  O.name + ']' AS ObjectName, AP.name, TT.name AS TypeName, AP.max_length from sys.all_parameters AP ");
			sql.Append("INNER JOIN sys.types TT ON TT.user_type_id = AP.user_type_id ");
			sql.Append("INNER JOIN sys.objects O ON O.object_id = AP.object_id ");
			sql.Append("WHERE type = 'PC' ORDER BY O.object_id, AP.parameter_id ");
			return sql.ToString();
		}

		private static string GetParameters2008()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select AP.is_output, AP.scale, AP.precision, '[' + SCHEMA_NAME(O.schema_id) + '].['+  O.name + ']' AS ObjectName, AP.name, TT.name AS TypeName, AP.max_length from sys.all_parameters AP ");
			sql.Append("INNER JOIN sys.types TT ON TT.user_type_id = AP.user_type_id ");
			sql.Append("INNER JOIN sys.objects O ON O.object_id = AP.object_id ");
			sql.Append("WHERE type = 'PC' ORDER BY O.object_id, AP.parameter_id ");
			return sql.ToString();
		}
	}
}
