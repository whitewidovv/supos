// /home/xavier/Projects/SuPOS/SuposAdmin/MainWindow.cs created with MonoDevelop
// User: xavier at 23:09 24/07/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections;
using Mono.Unix;
using Gtk;
using Gdk;
using Glade;

using LibSupos;

namespace SuposAdmin
{
	
	
	public class MainWindow: Gtk.Window
	{
		private SuposDb m_DataBase = null;
		private Config m_Config = null;
		private uint m_StatusContextId;
		
		[Widget] private Gtk.Window mainwindow;
		[Widget] private Gtk.Statusbar statusbar;
		[Widget] private Gtk.VBox stockvbox;
		[Widget] private Gtk.ToolButton toolbuttondisconnect;
		[Widget] private Gtk.ToolButton toolbuttonconnect;
		
		
		//****************************************
		// Constructor
		//****************************************
		public MainWindow(): base ("")
		{
			Catalog.Init("suposadmin","./locale");
			Glade.XML gxml = new Glade.XML (null, "suposadmin.glade", "mainwindow", "suposadmin");
			gxml.Autoconnect (this);
			m_StatusContextId = statusbar.GetContextId("StatusMsg");
			m_StatusContextId = statusbar.GetContextId("ActionMsg");
			statusbar.Push( m_StatusContextId, "Disconnected");
			stockvbox.Sensitive = false;
			toolbuttondisconnect.Sensitive = false;
			m_Config = new Config();
		}
		
		//****************************************
		// Connect to DB
		//****************************************
		private void m_Connect()
		{
			if ( m_DataBase == null)
			{
				string con_str = m_Config.GetConnectionString();
				if ( con_str == null )
				{
					m_ShowErrorDlg("Check your config file (~/.supos/supos-admin.ini)");
					return;
				}
				// Prompt for pass if not saved
				if ( !m_Config.IsPassConfigured() )
				{
					LoginDialog dlg = new LoginDialog();
					if ( m_Config.IsUserConfigured() )
					{
						dlg.userentry.Text = m_Config.GetUser();
					}
					int result = dlg.logindialog.Run();
					if ( result != 1)
					{
						dlg.logindialog.Destroy();
						return;
					}	
					con_str += "User Id=" + dlg.userentry.Text + ";";
					con_str += "Password=" + dlg.passentry.Text + ";";
					dlg.logindialog.Destroy();
				}
				m_DataBase = new SuposDb( con_str );
				try
				{
					m_DataBase.Connect();
				}
				catch (Exception e)
				{
					m_ShowErrorDlg(e.Message);
					m_Disconnect();
					return;
				}
				// Connection OK
				statusbar.Push( m_StatusContextId, "Connected");
				toolbuttondisconnect.Sensitive = true;
				toolbuttonconnect.Sensitive = false;
				if( m_DataBase.LoadCategories() )
				{
					stockvbox.Sensitive = true;
				}
				
			}
		}
		
		//****************************************
		// Disconnect from DB
		//****************************************
		private void m_Disconnect()
		{
			if ( m_DataBase != null )
			{
				m_DataBase.Disconnect();
				m_DataBase = null;
			}
			statusbar.Push( m_StatusContextId, "Disconnected");
			toolbuttondisconnect.Sensitive = false;
			toolbuttonconnect.Sensitive = true;
			stockvbox.Sensitive = false;
			//m_ClearView();
		}
		
	
		
		
		//******************************************
		// Show simple error message
		//******************************************
		private void m_ShowErrorDlg(string text)
		{
			MessageDialog dlg = new MessageDialog( mainwindow, 
			                                      Gtk.DialogFlags.DestroyWithParent,
			                                      Gtk.MessageType.Error,
			                                      Gtk.ButtonsType.Close,
			                                      text );
			dlg.Run();
			dlg.Destroy();
		}
		
		//*****************************************
		// CALLBACKS
		//*****************************************
		#pragma warning disable 0169
		private void OnWindowDeleteEvent (object sender, DeleteEventArgs a)
		{
			m_Disconnect();
			Application.Quit ();
			a.RetVal = true;
			Console.WriteLine("MW");
		}
		
		private void OnExitClicked (object sender, EventArgs a) 
		{
			m_Disconnect();
			Application.Quit ();
		}
		
		private void OnConnectClicked (object sender, EventArgs a) 
		{
			m_Connect();
		}
		
		private void OnDisconnectClicked (object sender, EventArgs a) 
		{
			m_Disconnect();
		}
		
		private void OnCategoriesClicked (object sender, EventArgs a) 
		{
			// UNDONE show only one windows
			CategoriesWindow win = new CategoriesWindow(m_DataBase);
			win.Show();
		}
		
		private void OnTaxesClicked (object sender, EventArgs a) 
		{
			// UNDONE show only one windows
			TaxesWindow win = new TaxesWindow(m_DataBase);
			win.Show();
		}
	}
}
