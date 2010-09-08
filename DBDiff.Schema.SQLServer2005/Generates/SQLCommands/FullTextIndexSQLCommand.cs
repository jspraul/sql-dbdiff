
using System.Text;
using DBDiff.Schema.SQLServer.Generates.Model;

namespace DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands
{
	internal static class FullTextIndexSQLCommand
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
			sql.Append("SELECT I.id as object_id,T.name as TableName,FC.name AS FullTextCatalogName,I.name AS [IndexName],CAST(OBJECTPROPERTY(T.id,'TableHasActiveFulltextIndex') AS bit) AS is_enabled,'['+ S.name + '].['+ T.name + '].[' + FC.name + ']' AS Name, C.name AS ColumnName, ");
			sql.Append("ISNULL(OBJECTPROPERTY(T.id,'TableFullTextPopulateStatus'),0) AS [PopulationStatus],(case ISNULL(OBJECTPROPERTY(T.id,'TableFullTextBackgroundUpdateIndexOn'),0) + ISNULL(OBJECTPROPERTY(T.id,'TableFullTextChangeTrackingOn'),0) when 0 then 'OFF' when 1 then 'MANUAL' when 2 then 'AUTO' end) AS [ChangeTracking],L.[alias] AS LanguageName ");
			sql.Append("FROM sysobjects AS T INNER JOIN sysusers AS S ON S.uid = T.uid ");
			sql.Append("INNER JOIN sysfulltextcatalogs AS FC ON (FC.ftcatid=T.ftcatid) AND (1=CAST(T.ftcatid AS bit)) ");
			sql.Append("INNER JOIN sysindexes AS I ON I.id = T.id and INDEXPROPERTY(T.id, I.name, 'IsFulltextKey') <> 0 ");
			sql.Append("INNER JOIN syscolumns C ON (COLUMNPROPERTY(C.id, C.name, 'IsFulltextIndexed') <> 0) AND (C.id=T.id) ");
			sql.Append("INNER JOIN master..syslanguages L ON(L.lcid=C.[language]) ");
			sql.Append("WHERE ((T.type='U' or T.type='S')) ");            
			return sql.ToString();
		}

		private static string Get2005()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT ");
			sql.Append("FI.object_id, ");
			sql.Append("T.Name AS TableName,  ");
			sql.Append("FC.name AS FullTextCatalogName, ");
			sql.Append("I.name AS IndexName, ");
			sql.Append("FI.is_enabled, ");
			sql.Append("'['+ S.name + '].['+ T.name + '].[' + FC.name + ']' AS Name, ");
			sql.Append("C.name as ColumnName, ");
			sql.Append("FI.change_tracking_state_desc AS ChangeTracking, ");
			sql.Append("FL.name AS LanguageName ");
			sql.Append("FROM sys.fulltext_indexes FI ");
			sql.Append("INNER JOIN sys.fulltext_catalogs FC ON FC.fulltext_catalog_id = FI.fulltext_catalog_id  ");
			sql.Append("INNER JOIN sys.indexes I ON I.index_id = FI.unique_index_id and I.object_id = FI.object_id  ");
			sql.Append("INNER JOIN sys.tables T ON T.object_id = FI.object_id ");
			sql.Append("INNER JOIN sys.schemas S ON S.schema_id = T.schema_id ");
			sql.Append("INNER JOIN sys.fulltext_index_columns FIC ON FIC.object_id = FI.object_id ");
			sql.Append("INNER JOIN sys.columns C ON C.object_id = FIC.object_id AND C.column_id = FIC.column_id ");
			sql.Append("INNER JOIN sys.fulltext_languages FL ON FL.lcid = FIC.language_id ");
			sql.Append("ORDER BY OBJECT_NAME(FI.object_id), I.name  ");
			return sql.ToString();
		}

		private static string Get2008()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT ");
			sql.Append("FI.object_id, ");
			sql.Append("T.Name AS TableName,  ");
			sql.Append("FC.name AS FullTextCatalogName, ");
			sql.Append("I.name AS IndexName, ");
			sql.Append("FI.is_enabled, ");
			sql.Append("'['+ S.name + '].['+ T.name + '].[' + FC.name + ']' AS Name, ");
			sql.Append("C.name as ColumnName, ");
			sql.Append("FL.name AS LanguageName,");
			sql.Append("DS.name AS FileGroupName, ");
			sql.Append("FI.change_tracking_state_desc AS ChangeTracking ");
			sql.Append("FROM sys.fulltext_indexes FI ");
			sql.Append("INNER JOIN sys.fulltext_catalogs FC ON FC.fulltext_catalog_id = FI.fulltext_catalog_id  ");
			sql.Append("INNER JOIN sys.indexes I ON I.index_id = FI.unique_index_id and I.object_id = FI.object_id  ");
			sql.Append("INNER JOIN sys.tables T ON T.object_id = FI.object_id ");
			sql.Append("INNER JOIN sys.schemas S ON S.schema_id = T.schema_id ");
			sql.Append("INNER JOIN sys.fulltext_index_columns FIC ON FIC.object_id = FI.object_id ");
			sql.Append("INNER JOIN sys.columns C ON C.object_id = FIC.object_id AND C.column_id = FIC.column_id ");
			sql.Append("INNER JOIN sys.data_spaces DS ON DS.data_space_id = FI.data_space_id ");
			sql.Append("INNER JOIN sys.fulltext_languages FL ON FL.lcid = FIC.language_id ");
			sql.Append("ORDER BY OBJECT_NAME(FI.object_id), I.name  ");
			return sql.ToString();
		}
	}
}
