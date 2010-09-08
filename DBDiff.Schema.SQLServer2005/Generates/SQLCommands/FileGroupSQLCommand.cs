
using System.Text;
using DBDiff.Schema.SQLServer.Generates.Model;

namespace DBDiff.Schema.SQLServer.Generates.Generates.SQLCommands
{
	internal static class FileGroupSQLCommand
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
			sql.Append("SELECT groupname as name, convert(int,groupid)  AS [ID],is_default=convert(bit,status & 16),is_read_only=convert(bit,status & 8),'FG' as type ");
			sql.Append("FROM sysfilegroups ORDER BY groupname");
			return sql.ToString();
		}

		private static string Get2005()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT name, data_space_id AS [ID], is_default, is_read_only, type ");
			sql.Append("FROM sys.filegroups ORDER BY name");
			return sql.ToString();
		}

		private static string Get2008()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT name, data_space_id AS [ID], is_default, is_read_only, type ");
			sql.Append("FROM sys.filegroups ORDER BY name");
			return sql.ToString();
		}

		public static string GetFilesInFileGroup(DatabaseInfo.VersionNumber version, FileGroup filegroup)
		{
			if (version == DatabaseInfo.VersionNumber.SQLServer2000) return GetFilesInFileGroup2000(filegroup);
			if (version == DatabaseInfo.VersionNumber.SQLServer2005) return GetFilesInFileGroup2005(filegroup);
			if (version == DatabaseInfo.VersionNumber.SQLServer2008) return GetFilesInFileGroup2008(filegroup);
			return "";
		}

		private static string GetFilesInFileGroup2000(FileGroup filegroup)
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select convert(int,fileid) as file_id,type=convert(tinyint,(status&64)/64),name,filename as physical_name,size,maxsize as max_size,growth,convert(bit,0) as is_sparse,is_percent_growth =convert(bit,status & 1048576) ");
			sql.Append("FROM  sysfiles where groupid="+filegroup.Id.ToString());
			return sql.ToString();
		}

		private static string GetFilesInFileGroup2005(FileGroup filegroup)
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select file_id,type,name,physical_name,size,max_size,growth,is_sparse,is_percent_growth ");
			sql.Append("from sys.database_files WHERE data_space_id = " + filegroup.Id.ToString());
			return sql.ToString();
		}

		private static string GetFilesInFileGroup2008(FileGroup filegroup)
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select file_id,type,name,physical_name,size,max_size,growth,is_sparse,is_percent_growth ");
			sql.Append("from sys.database_files WHERE data_space_id = " + filegroup.Id.ToString());
			return sql.ToString();
		}
	}
}
