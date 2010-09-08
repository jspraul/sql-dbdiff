
using System.Text;
using DBDiff.Schema.SQLServer.Generates.Model;

namespace DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands
{
	internal static class IndexSQLCommand
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
			sql.Append("SELECT OO.type AS ObjectType, convert(tinyint,IC.keyno) as key_ordinal, convert(int,C.xusertype) as user_type_id, I.id as object_id, dsidx.groupname as  FileGroup, convert(int,C.colid) as column_id,C.Name AS ColumnName, I.Name, convert(int,I.indid) as index_id, type=convert(tinyint,case convert(bit,I.status&16) when 0 then 2 when 1 then 1 else 0 end), is_unique=convert(bit,I.status & 2), ignore_dup_key=convert(bit,I.status & 1), is_primary_key=convert(bit,I.status & 2048), is_unique_constraint =convert(bit,I.status & 4096), fill_factor=OrigFillFactor, is_padded=convert(bit,I.status & 256), is_disabled=convert(bit,0), allow_row_locks=~convert(bit,lockflags&1), allow_page_locks=~convert(bit,lockflags&2), convert(bit,INDEXKEY_PROPERTY (IC.id,IC.indid ,IC.keyno,'isdescending')) as is_descending_key, convert(bit,0) as is_included_column, ISNULL(convert(bit,I.status & 16777216),0) AS NoAutomaticRecomputation ");
			sql.Append("FROM sysindexes I ");
			sql.Append("INNER JOIN sysobjects OO ON OO.id = I.id  ");
			sql.Append("INNER JOIN sysindexkeys IC ON IC.indid = I.indid AND IC.id = I.id ");
			sql.Append("INNER JOIN sysfilegroups AS dsidx ON dsidx.groupid = I.groupid ");
			sql.Append("INNER JOIN syscolumns C ON C.colid = IC.colid AND IC.id = C.id ");
			sql.Append("WHERE I.status&8388608=0 AND ");
			sql.Append("(I.status & 4096)/4096=0 AND (I.status & 2048)/2048=0 "); //AND I.object_id = " + table.Id.ToString(CultureInfo.InvariantCulture) + " ");
			sql.Append("AND objectproperty(I.id, 'IsMSShipped') <> 1 ");
			sql.Append("ORDER BY I.id, I.Name, IC.colid");
			return sql.ToString();
		}

		private static string Get2005()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT OO.type AS ObjectType, IC.key_ordinal, C.user_type_id, I.object_id, dsidx.Name as FileGroup, C.column_id,C.Name AS ColumnName, I.Name, I.index_id, I.type, is_unique, ignore_dup_key, is_primary_key, is_unique_constraint, fill_factor, is_padded, is_disabled, allow_row_locks, allow_page_locks, IC.is_descending_key, IC.is_included_column, ISNULL(ST.no_recompute,0) AS NoAutomaticRecomputation ");
			sql.Append("FROM sys.indexes I ");
			sql.Append("INNER JOIN sys.objects OO ON OO.object_id = I.object_id ");
			sql.Append("INNER JOIN sys.index_columns IC ON IC.index_id = I.index_id AND IC.object_id = I.object_id ");
			sql.Append("INNER JOIN sys.data_spaces AS dsidx ON dsidx.data_space_id = I.data_space_id ");
			sql.Append("INNER JOIN sys.columns C ON C.column_id = IC.column_id AND IC.object_id = C.object_id ");
			sql.Append("LEFT JOIN sys.stats AS ST ON ST.stats_id = I.index_id AND ST.object_id = I.object_id ");
			sql.Append("WHERE I.type IN (1,2,3) ");
			sql.Append("AND is_unique_constraint = 0 AND is_primary_key = 0 "); //AND I.object_id = " + table.Id.ToString(CultureInfo.InvariantCulture) + " ");
			sql.Append("AND objectproperty(I.object_id, 'IsMSShipped') <> 1 ");
			sql.Append("ORDER BY I.object_id, I.Name, IC.column_id");
			return sql.ToString();
		}

		private static string Get2008()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT ISNULL(I.filter_definition,'') AS FilterDefinition, OO.type AS ObjectType, IC.key_ordinal, C.user_type_id, I.object_id, dsidx.Name as FileGroup, C.column_id,C.Name AS ColumnName, I.Name, I.index_id, I.type, is_unique, ignore_dup_key, is_primary_key, is_unique_constraint, fill_factor, is_padded, is_disabled, allow_row_locks, allow_page_locks, IC.is_descending_key, IC.is_included_column, ISNULL(ST.no_recompute,0) AS NoAutomaticRecomputation ");
			sql.Append("FROM sys.indexes I ");
			sql.Append("INNER JOIN sys.objects OO ON OO.object_id = I.object_id ");
			sql.Append("INNER JOIN sys.index_columns IC ON IC.index_id = I.index_id AND IC.object_id = I.object_id ");
			sql.Append("INNER JOIN sys.data_spaces AS dsidx ON dsidx.data_space_id = I.data_space_id ");
			sql.Append("INNER JOIN sys.columns C ON C.column_id = IC.column_id AND IC.object_id = C.object_id ");
			sql.Append("LEFT JOIN sys.stats AS ST ON ST.stats_id = I.index_id AND ST.object_id = I.object_id ");
			sql.Append("WHERE I.type IN (1,2,3) ");
			sql.Append("AND is_unique_constraint = 0 AND is_primary_key = 0 "); //AND I.object_id = " + table.Id.ToString(CultureInfo.InvariantCulture) + " ");
			sql.Append("AND objectproperty(I.object_id, 'IsMSShipped') <> 1 ");
			sql.Append("ORDER BY I.object_id, I.Name, IC.column_id");
			return sql.ToString();
		}
	}
}
