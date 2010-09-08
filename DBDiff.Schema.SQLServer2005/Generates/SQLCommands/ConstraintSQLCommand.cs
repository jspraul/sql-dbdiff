using System.Text;
using DBDiff.Schema.SQLServer.Generates.Model;

namespace DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands
{
	internal static class ConstraintSQLCommand
	{
		public static string GetUniqueKey(DatabaseInfo.VersionNumber version)
		{
			if (version == DatabaseInfo.VersionNumber.SQLServer2000) return GetUniqueKey2000();
			if (version == DatabaseInfo.VersionNumber.SQLServer2005) return GetUniqueKey2005();
			if (version == DatabaseInfo.VersionNumber.SQLServer2008) return GetUniqueKey2008();
			return "";
		}

		public static string GetCheck(DatabaseInfo.VersionNumber version)
		{
			if (version == DatabaseInfo.VersionNumber.SQLServer2000) return GetCheck2000();
			if (version == DatabaseInfo.VersionNumber.SQLServer2005) return GetCheck2005();
			if (version == DatabaseInfo.VersionNumber.SQLServer2008) return GetCheck2008();
			return "";
		}

		public static string GetPrimaryKey(DatabaseInfo.VersionNumber version, Table table)
		{
			if (version == DatabaseInfo.VersionNumber.SQLServer2000) return GetPrimaryKey2000(/*table*/);
			if (version == DatabaseInfo.VersionNumber.SQLServer2005) return GetPrimaryKey2005();
			if (version == DatabaseInfo.VersionNumber.SQLServer2008) return GetPrimaryKey2008();
			return "";
		}

		private static string GetUniqueKey2008()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT O.type as ObjectType, S.Name as Owner, I.object_Id AS id,dsidx.Name as FileGroup, C.user_type_id, C.column_id, I.Index_id, C.Name AS ColumnName, I.Name, I.type, I.fill_factor, I.is_padded, I.allow_row_locks, I.allow_page_locks, I.ignore_dup_key, I.is_disabled, IC.is_descending_key, IC.is_included_column ");
			sql.Append("FROM sys.indexes I ");
			sql.Append("INNER JOIN sys.objects O ON O.object_id = I.object_id ");
			sql.Append("INNER JOIN sys.schemas S ON S.schema_id = O.schema_id ");
			sql.Append("INNER JOIN sys.index_columns IC ON IC.index_id = I.index_id AND IC.object_id = I.object_id ");
			sql.Append("INNER JOIN sys.columns C ON C.column_id = IC.column_id AND IC.object_id = C.object_id ");
			sql.Append("LEFT JOIN sys.data_spaces AS dsidx ON dsidx.data_space_id = I.data_space_id ");
			sql.Append("WHERE is_unique_constraint = 1 AND O.type <> 'TF' ORDER BY I.object_id,I.Name");
			return sql.ToString();
		}

		private static string GetUniqueKey2005()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT O.type as ObjectType, S.Name as Owner, I.object_Id AS id,dsidx.Name as FileGroup, C.user_type_id, C.column_id, I.Index_id, C.Name AS ColumnName, I.Name, I.type, I.fill_factor, I.is_padded, I.allow_row_locks, I.allow_page_locks, I.ignore_dup_key, I.is_disabled, IC.is_descending_key, IC.is_included_column ");
			sql.Append("FROM sys.indexes I ");
			sql.Append("INNER JOIN sys.objects O ON O.object_id = I.object_id ");
			sql.Append("INNER JOIN sys.schemas S ON S.schema_id = O.schema_id ");
			sql.Append("INNER JOIN sys.index_columns IC ON IC.index_id = I.index_id AND IC.object_id = I.object_id ");
			sql.Append("INNER JOIN sys.columns C ON C.column_id = IC.column_id AND IC.object_id = C.object_id ");
			sql.Append("LEFT JOIN sys.data_spaces AS dsidx ON dsidx.data_space_id = I.data_space_id ");
			sql.Append("WHERE is_unique_constraint = 1 AND O.type <> 'TF' ORDER BY I.object_id,I.Name");
			return sql.ToString();
		}
		
		private static string GetUniqueKey2000()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT O.type as ObjectType, S.Name as Owner, I.Id AS id,dsidx.groupname as FileGroup, C.xusertype as user_type_id, C.colid as column_id, I.indid, C.Name AS ColumnName, I.Name, type=convert(int,case convert(bit,I.status&16) when 0 then 2 when 1 then 1 else 0 end), fill_factor=OrigFillFactor, is_padded=convert(bit,I.status & 256), allow_row_locks=~convert(bit,lockflags&1), allow_page_locks=~convert(bit,lockflags&2), ignore_dup_key=convert(bit,I.status & 1),0 as is_disabled,INDEXKEY_PROPERTY (IC.id,IC.indid ,IC.keyno,'isdescending') as is_descending_key, 0 as is_included_column  ");
			sql.Append("FROM sysindexes I  ");
			sql.Append("INNER JOIN sysobjects O ON O.id = I.id ");
			sql.Append("INNER JOIN sysusers S ON S.uid = O.uid ");
			sql.Append("INNER JOIN sysindexkeys IC ON IC.indid = I.indid AND IC.id = I.id ");
			sql.Append("INNER JOIN syscolumns C ON C.colid = IC.colid AND IC.id = C.id ");
			sql.Append("LEFT JOIN sysfilegroups AS dsidx ON dsidx.groupid = I.groupid ");
			sql.Append("WHERE convert(bit,I.status & 4096) = 1 AND O.type <> 'TF' ORDER BY I.id,I.Name");
			return sql.ToString();
		}

		private static string GetCheck2008()
		{
			string sql;
			sql = "SELECT  ";
			sql += "CC.parent_object_id, ";
			sql += "O.type as ObjectType, ";
			sql += "CC.object_id AS ID, ";
			sql += "CC.parent_column_id, ";
			sql += "CC.name, ";
			sql += "CC.type, ";
			sql += "CC.definition, ";
			sql += "CC.is_disabled, ";
			sql += "CC.is_not_trusted AS WithCheck, ";
			sql += "CC.is_not_for_replication, ";
			sql += "0, ";
			sql += "schema_name(CC.schema_id) AS Owner ";
			sql += "FROM sys.check_constraints CC ";
			sql += "INNER JOIN sys.objects O ON O.object_id = CC.parent_object_id ";
			sql += "ORDER BY CC.parent_object_id,CC.name";
			return sql;
		}

		private static string GetCheck2005()
		{
			string sql;
			sql = "SELECT  ";
			sql += "CC.parent_object_id, ";
			sql += "O.Type as ObjectType, ";
			sql += "CC.object_id AS ID, ";
			sql += "CC.parent_column_id, ";
			sql += "CC.name, ";
			sql += "CC.type, ";
			sql += "CC.definition, ";
			sql += "CC.is_disabled, ";
			sql += "CC.is_not_trusted AS WithCheck, ";
			sql += "CC.is_not_for_replication, ";
			sql += "0, ";
			sql += "schema_name(CC.schema_id) AS Owner ";
			sql += "FROM sys.check_constraints CC ";
			sql += "INNER JOIN sys.objects O ON O.object_id = CC.parent_object_id ";
			sql += "ORDER BY CC.parent_object_id,CC.name";
			return sql;
		}

		private static string GetCheck2000()
		{
			string sql;
			sql = "SELECT  ";
			sql += "CC.id as parent_object_id,  ";
			sql += "O.Type as ObjectType, ";
			sql += "CC.constid AS ID, ";
			sql += "CC.colid as parent_column_id, ";
			sql += "ODC.name, ";
			sql += "ODC.type, ";
			sql += "sccom.Text as definition, ";
			sql += "convert(bit,CC.status & 16384) as is_disabled, ";
			sql += "convert(bit,0) AS WithCheck, ";
			sql += "convert(bit,CC.status & 2097152) as is_not_for_replication, ";
			sql += "0, ";
			sql += "SU.name AS Owner ";
			sql += "FROM sysconstraints CC inner join sysobjects  ODC on ODC.id=CC.constid and ODC.xtype ='C ' inner join syscomments sccom on (ODC.id=sccom.id) ";
			sql += "inner join sysusers SU on ODC.uid=SU.uid ";
			sql += "inner join sysobjects O on(O.id=cc.id) ";
			sql += "ORDER BY CC.id,ODC.name";
			return sql;
		}

		private static string GetPrimaryKey2008()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT O.type as ObjectType, S.Name as Owner, IC.key_ordinal, C.user_type_id, I.object_id AS ID, dsidx.Name AS FileGroup, C.column_id, I.Index_id, C.Name AS ColumnName, I.Name, I.type, I.fill_factor, I.is_padded, I.allow_row_locks, I.allow_page_locks, I.ignore_dup_key, I.is_disabled, IC.is_descending_key, IC.is_included_column, CONVERT(bit,INDEXPROPERTY(I.object_id,I.name,'IsAutoStatistics')) AS IsAutoStatistics ");
			sql.Append("FROM sys.indexes I ");
			sql.Append("INNER JOIN sys.objects O ON O.object_id = I.object_id ");
			sql.Append("INNER JOIN sys.schemas S ON S.schema_id = O.schema_id ");
			sql.Append("INNER JOIN sys.index_columns IC ON IC.index_id = I.index_id AND IC.object_id = I.object_id ");
			sql.Append("INNER JOIN sys.columns C ON C.column_id = IC.column_id AND IC.object_id = C.object_id ");
			sql.Append("LEFT JOIN sys.data_spaces AS dsidx ON dsidx.data_space_id = I.data_space_id ");
			sql.Append("WHERE is_primary_key = 1 AND O.type <> 'TF' ORDER BY I.object_id");
			return sql.ToString();
		}

		private static string GetPrimaryKey2005()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT O.type as ObjectType, S.Name as Owner, IC.key_ordinal, C.user_type_id, I.object_id AS ID, dsidx.Name AS FileGroup, C.column_id, I.Index_id, C.Name AS ColumnName, I.Name, I.type, I.fill_factor, I.is_padded, I.allow_row_locks, I.allow_page_locks, I.ignore_dup_key, I.is_disabled, IC.is_descending_key, IC.is_included_column, CONVERT(bit,INDEXPROPERTY(I.object_id,I.name,'IsAutoStatistics')) AS IsAutoStatistics ");
			sql.Append("FROM sys.indexes I ");
			sql.Append("INNER JOIN sys.objects O ON O.object_id = I.object_id ");
			sql.Append("INNER JOIN sys.schemas S ON S.schema_id = O.schema_id ");
			sql.Append("INNER JOIN sys.index_columns IC ON IC.index_id = I.index_id AND IC.object_id = I.object_id ");
			sql.Append("INNER JOIN sys.columns C ON C.column_id = IC.column_id AND IC.object_id = C.object_id ");
			sql.Append("INNER JOIN sys.data_spaces AS dsidx ON dsidx.data_space_id = I.data_space_id ");
			sql.Append("WHERE is_primary_key = 1 AND O.type <> 'TF' ORDER BY I.object_id");
			return sql.ToString();
		}

		private static string GetPrimaryKey2000(/*Table table*/)
		{
			StringBuilder sql = new StringBuilder();
			/*sql.Append("SELECT CONVERT(tinyint,CASE WHEN SI.indid = 0 THEN 0 WHEN SI.indid = 1 THEN 1 WHEN SI.indid > 1 THEN 2 END) AS Type,f.groupname AS FileGroup,CONVERT(int,SI.indid) AS Index_id, CONVERT(int,SI.indid) AS ID, SI.name, SC.colid, SC.Name AS ColumnName, CONVERT(bit,0) AS is_included_column, SIK.keyno AS key_ordinal, CONVERT(bit,INDEXPROPERTY(SI.id,SI.name,'IsPadIndex')) AS is_padded, CONVERT(bit,INDEXPROPERTY(SI.id,SI.name,'IsRowLockDisallowed')) AS allow_row_locks, CONVERT(bit,INDEXPROPERTY(SI.id,SI.name,'IsPageLockDisallowed')) AS allow_page_locks, CONVERT(bit,INDEXPROPERTY(SI.id,SI.name,'IsAutoStatistics')) AS IsAutoStatistics, CONVERT(tinyint,INDEXPROPERTY(SI.id,SI.name,'IndexFillFactor')) AS fill_factor,  INDEXKEY_PROPERTY(SI.id, SI.indid,SC.colid,'IsDescending') AS is_descending_key, CONVERT(bit,0) AS is_disabled, CONVERT(bit,0) AS is_included_column ");
			sql.Append("FROM sysindexes SI INNER JOIN sysindexkeys SIK ON SI.indid = SIK.indid AND SIK.id = SI.ID ");
			sql.Append("INNER JOIN syscolumns SC ON SC.colid = SIK.colid AND SC.id = SI.ID ");
			sql.Append("inner join sysfilegroups f on f.groupid = SI.groupid ");
			sql.Append("WHERE (SI.status & 0x800) = 0x800 AND SI.id = " + table.Id.ToString() + " ORDER BY SIK.keyno");*/
			sql.Append("SELECT O.type as ObjectType, S.Name as Owner, convert(tinyint,IC.keyno) as key_ordinal, convert(int,C.xusertype) as user_type_id, I.id as ID, dsidx.groupname AS FileGroup, convert(int,C.colid) as column_id, convert(int,I.indid) as Index_id, C.Name AS ColumnName, I.Name,type=convert(tinyint,case convert(bit,I.status&16) when 0 then 2 when 1 then 1 else 0 end), fill_factor=OrigFillFactor, is_padded=convert(bit,I.status & 256),allow_row_locks=~convert(bit,lockflags&1), allow_page_locks=~convert(bit,lockflags&2), ignore_dup_key=convert(bit,I.status & 1), CONVERT(bit,0) AS is_disabled, convert(bit,INDEXKEY_PROPERTY (IC.id,IC.indid ,IC.keyno,'isdescending')) as is_descending_key, CONVERT(bit,0) AS is_included_column, CONVERT(bit,INDEXPROPERTY(I.id,I.name,'IsAutoStatistics')) AS IsAutoStatistics  ");
			sql.Append("FROM sysindexes I INNER JOIN sysobjects O ON O.id = I.id ");
			sql.Append("INNER JOIN sysusers S ON S.uid = O.uid ");
			sql.Append("INNER JOIN sysindexkeys IC ON IC.indid = I.indid AND IC.id = I.id ");
			sql.Append("INNER JOIN sysfilegroups AS dsidx ON dsidx.groupid = I.groupid ");
			sql.Append("INNER JOIN syscolumns C ON C.colid = IC.colid AND IC.id = C.id ");
			sql.Append("WHERE convert(bit,I.status & 2048)=1 AND O.type <> 'TF' ORDER BY I.id");
			return sql.ToString();
		}
	}
}
