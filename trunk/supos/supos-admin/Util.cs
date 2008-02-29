
using System;
using System.IO;
using Nini.Config;

namespace suposadmin
{
	
	
	public class Util
	{
		private static string m_ConfigDirectoryPath = Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "supos");
		private static string m_ConfigFileName = "supos-admin.config";
			
		
		
		public static bool IsConfigFileFull()
		{
			
				
			return false;
		}
			
		public static IConfigSource GetConfigSrc()
		{
			IniConfigSource src;
			// check if file exist, if not create it 
			if ( ! Directory.Exists(m_ConfigDirectoryPath) )
			{
				Directory.CreateDirectory(m_ConfigDirectoryPath);
			}
			
			if ( ! File.Exists( Path.Combine(m_ConfigDirectoryPath, m_ConfigFileName) ))
			{
				
				src = new IniConfigSource();
				src.Save( Path.Combine(m_ConfigDirectoryPath, m_ConfigFileName) );
			}
			else
			{
				src = new IniConfigSource( Path.Combine(m_ConfigDirectoryPath, m_ConfigFileName) );
			}
			//Add config that do not exist
			if ( src.Configs["Server"] == null )
				src.AddConfig("Server");
			if ( ! src.Configs["Server"].Contains("Server") )
				src.Configs["Server"].Set("Server","127.0.0.1");
			if ( !src.Configs["Server"].Contains("Database") )
				src.Configs["Server"].Set("Database","supos");
			if ( !src.Configs["Server"].Contains("User Id") )
				src.Configs["Server"].Set("User Id","supos");
			if ( !src.Configs["Server"].Contains("Port") )
				src.Configs["Server"].Set("Port","5432");
			src.Save();
			return src;
		}
	}
}
