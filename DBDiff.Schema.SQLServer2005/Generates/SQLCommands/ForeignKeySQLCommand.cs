
using System.Text;
using DBDiff.Schema.SQLServer.Generates.Model;

namespace DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands
{
	internal static class ForeignKeySQLCommand
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
			sql.Append("SELECT FK.constid as object_id, convert(int,C.xusertype) as user_type_id ,convert(int,FK.id) as parent_object_id,S.Name AS Owner, S2.Name AS ReferenceOwner, C2.Name AS ColumnName, convert(int,C2.colid) AS ColumnId, C.name AS ColumnRelationalName, convert(int,C.colid) AS ColumnRelationalId, convert(int,T.id) AS TableRelationalId, convert(int,FK.id) AS TableId, T.Name AS TableRelationalName, ODC.Name, convert(bit,FK.status & 16384) as is_disabled, convert(bit,FK.status & 2097152) as  is_not_for_replication,convert(bit,0) as is_not_trusted, convert(tinyint,(ODC.status & 4096)/4096) as delete_referential_action, convert(tinyint,(ODC.status & 8192)/8192) as update_referential_action ");
			sql.Append("FROM sysconstraints FK inner join sysobjects  ODC on ODC.id=FK.constid and ODC.xtype ='F ' ");
			sql.Append("INNER join sysforeignkeys FKC ON FK.constid=FKC.constid ");
			sql.Append("INNER JOIN sysobjects T ON T.id = FKC.rkeyid and T.type='U' ");
			sql.Append("INNER JOIN sysusers S2 ON S2.uid = T.uid ");
			sql.Append("INNER JOIN syscolumns C ON C.id = FKC.rkeyid AND C.colid = FKC.rkey ");
			sql.Append("INNER JOIN syscolumns C2 ON C2.id = FKC.fkeyid AND C2.colid = FKC.fkey ");
			sql.Append("INNER JOIN sysusers S ON S.uid = ODC.uid ");
			sql.Append("ORDER BY FK.id, ODC.Name, ColumnId");
			return sql.ToString();
		}

		private static string Get2005()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT FK.object_id, C.user_type_id ,FK.parent_object_id,S.Name AS Owner, S2.Name AS ReferenceOwner, C2.Name AS ColumnName, C2.column_id AS ColumnId, C.name AS ColumnRelationalName, C.column_id AS ColumnRelationalId, T.object_id AS TableRelationalId, FK.Parent_object_id AS TableId, T.Name AS TableRelationalName, FK.Name, FK.is_disabled, FK.is_not_for_replication, FK.is_not_trusted, FK.delete_referential_action, FK.update_referential_action ");
			sql.Append("FROM sys.foreign_keys FK ");
			sql.Append("INNER JOIN sys.tables T ON T.object_id = FK.referenced_object_id ");
			sql.Append("INNER JOIN sys.schemas S2 ON S2.schema_id = T.schema_id ");
			sql.Append("INNER JOIN sys.foreign_key_columns FKC ON FKC.constraint_object_id = FK.object_id ");
			sql.Append("INNER JOIN sys.columns C ON C.object_id = FKC.referenced_object_id AND C.column_id = referenced_column_id ");
			sql.Append("INNER JOIN sys.columns C2 ON C2.object_id = FKC.parent_object_id AND C2.column_id = parent_column_id ");
			sql.Append("INNER JOIN sys.schemas S ON S.schema_id = FK.schema_id ");
			sql.Append("ORDER BY FK.parent_object_id, FK.Name, ColumnId");
			return sql.ToString();
		}

		private static string Get2008()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT FK.object_id, C.user_type_id ,FK.parent_object_id,S.Name AS Owner, S2.Name AS ReferenceOwner, C2.Name AS ColumnName, C2.column_id AS ColumnId, C.name AS ColumnRelationalName, C.column_id AS ColumnRelationalId, T.object_id AS TableRelationalId, FK.Parent_object_id AS TableId, T.Name AS TableRelationalName, FK.Name, FK.is_disabled, FK.is_not_for_replication, FK.is_not_trusted, FK.delete_referential_action, FK.update_referential_action ");
			sql.Append("FROM sys.foreign_keys FK ");
			sql.Append("INNER JOIN sys.tables T ON T.object_id = FK.referenced_object_id ");
			sql.Append("INNER JOIN sys.schemas S2 ON S2.schema_id = T.schema_id ");
			sql.Append("INNER JOIN sys.foreign_key_columns FKC ON FKC.constraint_object_id = FK.object_id ");
			sql.Append("INNER JOIN sys.columns C ON C.object_id = FKC.referenced_object_id AND C.column_id = referenced_column_id ");
			sql.Append("INNER JOIN sys.columns C2 ON C2.object_id = FKC.parent_object_id AND C2.column_id = parent_column_id ");
			sql.Append("INNER JOIN sys.schemas S ON S.schema_id = FK.schema_id ");
			sql.Append("ORDER BY FK.parent_object_id, FK.Name, ColumnId");
			return sql.ToString();
		}
	}
}
