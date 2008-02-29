// 

using System;
using Nini.Config;
using Libsupos;

namespace suposadmin
{
	
	
	public partial class SettingsDialog : Gtk.Dialog
	{
		private IConfigSource m_ConfigSrc = null;
		
		public SettingsDialog(IConfigSource ConfigSrc)
		{
			this.Build();
			savepasscheckbutton.Toggled += new EventHandler(OnSavepassToggled);
			m_ConfigSrc = ConfigSrc;
			serverentry.Text = m_ConfigSrc.Configs["Server"].Get("Server");
			portspinbutton.Value = m_ConfigSrc.Configs["Server"].GetInt("Port");
			dbentry.Text = m_ConfigSrc.Configs["Server"].Get("Database");
			loginentry.Text = m_ConfigSrc.Configs["Server"].Get("User Id");
			if ( m_ConfigSrc.Configs["Server"].Contains("Password") )
			{
				savepasscheckbutton.Active = true;
				passentry.Text = m_ConfigSrc.Configs["Server"].Get("Password");
			}
			else
			{
				savepasscheckbutton.Active = false;
			}
		}
		
		protected virtual void OnSavepassToggled (object obj, EventArgs args)
		{
			passentry.Sensitive = savepasscheckbutton.Active;
		}

		protected virtual void OnOk (object sender, System.EventArgs e)
		{
			m_ConfigSrc.Configs["Server"].Set("Server", serverentry.Text);
			m_ConfigSrc.Configs["Server"].Set("Port", portspinbutton.Value);
			m_ConfigSrc.Configs["Server"].Set("Database",dbentry.Text);
			m_ConfigSrc.Configs["Server"].Set("User Id", loginentry.Text);
			if ( savepasscheckbutton.Active )
				m_ConfigSrc.Configs["Server"].Set("Password", passentry.Text);
			else
				if (m_ConfigSrc.Configs["Server"].Contains("Password") )
					m_ConfigSrc.Configs["Server"].Remove("Password");
			m_ConfigSrc.Save();
			//m_ConfigSrc.s
		}
	}
}
