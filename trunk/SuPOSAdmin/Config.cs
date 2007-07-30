// /home/xavier/Projects/SuPOS/SuPOSAdmin/Config.cs created with MonoDevelop
// User: xavier at 12:57 25/07/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Nini.Config;

namespace SuPOSAdmin
{
	
	
	public class Config
	{
		private string m_ConfigFolder = null;
		private IConfigSource m_DbConfigSource = null;
		private bool m_IsUserConfigured = false;
		private bool m_IsPassConfigured = false;
		private string m_User = null;
		
		
		//******************************************
		// Constructor
		//******************************************
		public Config()
		{
			m_ConfigFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/.supos";
			try
			{
				m_DbConfigSource = new IniConfigSource( m_ConfigFolder + "/supos-admin.ini" );
			}
			catch (Exception e)
			{
				Console.WriteLine( e.Message );
			}
		}
		
		//******************************************
		// Build Connection string from config file
		//******************************************
		public string GetConnectionString()
		{
			string tmp_str = null;
			if (m_DbConfigSource == null)
				return null;
			// Server and DB 
			string srv_str = m_DbConfigSource.Configs["Server"].Get("Server");
			string db_str = m_DbConfigSource.Configs["Server"].Get("Database");
			if ( srv_str.Length <= 0 || db_str.Length <= 0 )
			{
				Console.WriteLine("No Configurtion for SERVER or DATABASE !");
				return null;
			}
			tmp_str += "Server=" + srv_str + ";";
			tmp_str += "Database=" + db_str + ";";
			// User and password
			string user_str = m_DbConfigSource.Configs["Server"].Get("User Id");
			string pass_str = m_DbConfigSource.Configs["Server"].Get("Password");
			if ( user_str.Length > 0 )
			{
				m_IsUserConfigured = true;
				m_User = user_str;
				if ( pass_str.Length > 0 )
				{
					m_IsPassConfigured = true;
					tmp_str += "User Id=" + user_str + ";";
					tmp_str += "Password=" + pass_str + ";";
				}
				else
					m_IsPassConfigured = false;
			}
			else
				m_IsUserConfigured = false;
			// TODO support more argument	
			return tmp_str;
		}
		
		//************************************************************
		// Tell if the USER and PASSWORD are stored in the config file
		//************************************************************
		public bool IsPassConfigured()
		{
			return m_IsPassConfigured;
		}
		
		public bool IsUserConfigured()
		{
			return m_IsUserConfigured;
		}
		
		public string GetUser()
		{
			return m_User;
		}
	}
}
