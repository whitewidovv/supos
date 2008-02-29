using System;
using System.Data.Common;
using Nini.Config;

namespace Libsupos
{
	
	
	public class SuposConfig
	{
		private IConfigSource m_DbConfigSource = null;
		private DbConnectionStringBuilder m_builder = null;
		
		//******************************************
		// Constructor
		//******************************************
		public SuposConfig(IConfigSource src)
		{
			m_DbConfigSource = src;
			m_builder = new DbConnectionStringBuilder();
			
			if ( m_DbConfigSource.Configs["Server"].Contains("Server") )
			{
				m_builder.Add("Server", m_DbConfigSource.Configs["Server"].Get("Server"));
			}
			if ( m_DbConfigSource.Configs["Server"].Contains("Port") )
			{
				m_builder.Add("Port", m_DbConfigSource.Configs["Server"].Get("Port"));
			}
			if ( m_DbConfigSource.Configs["Server"].Contains("Database") )
			{
				m_builder.Add("Database", m_DbConfigSource.Configs["Server"].Get("Database"));
			}
			if ( m_DbConfigSource.Configs["Server"].Contains("User Id") )
			{
				m_builder.Add("User Id", m_DbConfigSource.Configs["Server"].Get("User Id"));
			}
			if ( m_DbConfigSource.Configs["Server"].Contains("Password") )
			{
				m_builder.Add("Password", m_DbConfigSource.Configs["Server"].Get("Password"));
			}
			// UNDONE Support more arguments
		}
		
		public string GetConnectionString()
		{
			return m_builder.ConnectionString;
		}
		
		public string User
		{
			get
			{
				if ( m_builder["User Id"] == null )
					return null;
				return m_builder["User Id"].ToString();
			}
			set
			{
				if ( m_builder["User Id"] == null )
					m_builder.Add("User Id","");
				m_builder["User ID"] = value;
			}
		}
		
		public string Password
		{
			get
			{
				if ( m_builder.ContainsKey("Password"))
					return m_builder["Password"].ToString();
				return null;
			}
			set
			{
				if ( ! m_builder.ContainsKey("Password") )
					m_builder.Add("Password","");
				m_builder["Password"] = value;
			}
		}
	}
}
