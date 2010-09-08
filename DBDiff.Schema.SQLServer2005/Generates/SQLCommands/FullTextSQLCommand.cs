
using System.Text;
using DBDiff.Schema.SQLServer.Generates.Model;

namespace DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands
{
	internal static class FullTextSQLCommand
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
			sql.Append("SELECT '' as Owner,'' AS FileGroupName,convert(int,FC.ftcatid) as fulltext_catalog_id, FC.Name, path, is_default=convert(bit,0),convert(bit,0) as is_accent_sensitivity_on ");
			sql.Append("FROM sysfulltextcatalogs FC ");
			return sql.ToString();
		}

		private static string Get2005()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT S.Name as Owner,F.name AS FileGroupName, fulltext_catalog_id, FC.Name, path, FC.is_default, is_accent_sensitivity_on ");
			sql.Append("FROM sys.fulltext_catalogs FC ");
			sql.Append("LEFT JOIN sys.filegroups F ON F.data_space_id = FC.data_space_id ");
			sql.Append("INNER JOIN sys.schemas S ON S.schema_id = FC.principal_id");
			return sql.ToString();
		}

		private static string Get2008()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT S.Name as Owner,F.name AS FileGroupName, fulltext_catalog_id, FC.Name, path, FC.is_default, is_accent_sensitivity_on ");
			sql.Append("FROM sys.fulltext_catalogs FC ");
			sql.Append("LEFT JOIN sys.filegroups F ON F.data_space_id = FC.data_space_id ");
			sql.Append("INNER JOIN sys.schemas S ON S.schema_id = FC.principal_id");
			return sql.ToString();
		}
	}
}
