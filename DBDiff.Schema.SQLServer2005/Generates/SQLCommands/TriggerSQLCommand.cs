
using System.Text;
using DBDiff.Schema.SQLServer.Generates.Model;

namespace DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands
{
	internal static class TriggerSQLCommand
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
			sql.Append("SELECT T.id as object_id, O.type AS ObjectType, 'CALLER' as ExecuteAs,NULL AS assembly_name,NULL AS assembly_class,NULL as assembly_id,NULL AS assembly_method, T.type, convert(bit,T.status&1024) AS IsInsert, convert(bit,T.status&512) AS IsUpdate, convert(bit,T.status&256) AS IsDelete, T.parent_obj as parent_id, S.name AS Owner,T.name,convert(bit,T.status&2048) AS is_disabled,convert(bit,T.status&4096) AS is_not_for_replication,convert(bit,T.status&8192) AS is_instead_of_trigger ");
			sql.Append("FROM sysobjects T ");
			sql.Append("INNER JOIN sysobjects O ON O.id = T.parent_obj and T.type= 'TR' ");
			sql.Append("INNER JOIN sysusers S ON S.uid = O.uid ");
			sql.Append("ORDER BY T.parent_obj ");

			return sql.ToString();
		}

		private static string Get2005()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT T.object_id, O.type AS ObjectType, ISNULL(CONVERT(varchar,AM.execute_as_principal_id),'CALLER') as ExecuteAs, AF.name AS assembly_name, AM.assembly_class, AM.assembly_id, AM.assembly_method, T.type, CAST(ISNULL(tei.object_id,0) AS bit) AS IsInsert, CAST(ISNULL(teu.object_id,0) AS bit) AS IsUpdate, CAST(ISNULL(ted.object_id,0) AS bit) AS IsDelete, T.parent_id, S.name AS Owner,T.name,is_disabled,is_not_for_replication,is_instead_of_trigger ");
			sql.Append("FROM sys.triggers T ");
			sql.Append("INNER JOIN sys.objects O ON O.object_id = T.parent_id ");
			sql.Append("INNER JOIN sys.schemas S ON S.schema_id = O.schema_id ");
			sql.Append("LEFT JOIN sys.trigger_events AS tei ON tei.object_id = T.object_id and tei.type=1 ");
			sql.Append("LEFT JOIN sys.trigger_events AS teu ON teu.object_id = T.object_id and teu.type=2 ");
			sql.Append("LEFT JOIN sys.trigger_events AS ted ON ted.object_id = T.object_id and ted.type=3 ");
			sql.Append("LEFT JOIN sys.assembly_modules AM ON AM.object_id = T.object_id ");
			sql.Append("LEFT JOIN sys.assemblies AF ON AF.assembly_id = AM.assembly_id ");
			sql.Append("ORDER BY T.parent_id ");
			return sql.ToString();
		}

		private static string Get2008()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT T.object_id, O.type AS ObjectType, ISNULL(CONVERT(varchar,AM.execute_as_principal_id),'CALLER') as ExecuteAs, AF.name AS assembly_name, AM.assembly_class, AM.assembly_id, AM.assembly_method, T.type, CAST(ISNULL(tei.object_id,0) AS bit) AS IsInsert, CAST(ISNULL(teu.object_id,0) AS bit) AS IsUpdate, CAST(ISNULL(ted.object_id,0) AS bit) AS IsDelete, T.parent_id, S.name AS Owner,T.name,is_disabled,is_not_for_replication,is_instead_of_trigger ");
			sql.Append("FROM sys.triggers T ");
			sql.Append("INNER JOIN sys.objects O ON O.object_id = T.parent_id ");
			sql.Append("INNER JOIN sys.schemas S ON S.schema_id = O.schema_id ");
			sql.Append("LEFT JOIN sys.trigger_events AS tei ON tei.object_id = T.object_id and tei.type=1 ");
			sql.Append("LEFT JOIN sys.trigger_events AS teu ON teu.object_id = T.object_id and teu.type=2 ");
			sql.Append("LEFT JOIN sys.trigger_events AS ted ON ted.object_id = T.object_id and ted.type=3 ");
			sql.Append("LEFT JOIN sys.assembly_modules AM ON AM.object_id = T.object_id ");
			sql.Append("LEFT JOIN sys.assemblies AF ON AF.assembly_id = AM.assembly_id ");
			sql.Append("ORDER BY T.parent_id ");
			return sql.ToString();
		}
	}
}
