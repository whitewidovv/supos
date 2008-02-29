using System;
using Gtk;
using Libsupos;
using Nini.Config;
using suposadmin;

public partial class MainWindow: Gtk.Window
{	
	private SuposDb m_DataBase = null;
	private SuposConfig m_Config = null;
	private IConfigSource m_ConfigSrc = null;
	
	private MainViewWidget m_MainWidget = null;
	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		m_ConfigSrc = Util.GetConfigSrc();
		
		m_MainWidget = new DisconnectedViewWidget();
		mainpaned.Add2(m_MainWidget);
		m_SetDisconnected();
	}

	private void m_Connect()
	{
		if ( m_DataBase == null)
		{
			m_Config = new SuposConfig( m_ConfigSrc );
			// Prompt for user and password if not saved
			if ( m_Config.Password == null )
			{
				LoginDialog dlg = new LoginDialog();
				dlg.UserEntry.Text = m_Config.User;
				int result = dlg.Run();
				if ( result != (int)ResponseType.Ok)
				{
					dlg.Destroy();
					return;
				}	
				m_Config.User = dlg.UserEntry.Text;
				m_Config.Password = dlg.PassEntry.Text;
				dlg.Destroy();
			}
			// Try to connect and load
			m_DataBase = new SuposDb( m_Config.GetConnectionString() );
			try
			{
				m_DataBase.Connect();
				m_DataBase.LoadGeneralInfo();
				m_DataBase.LoadCategories();
				m_DataBase.LoadTaxes();
				m_DataBase.LoadProducts();
			}
			catch (Exception e)
			{
				MsgDlg(e.Message);
				m_Disconnect();
				m_DataBase=null;
				return;
			}
			m_SetConnected();
		}
	}
	
	
	private void m_Disconnect()
	{
		if (m_DataBase != null && m_DataBase.Opened)
		{
			m_DataBase.Disconnect();
		}
		m_DataBase = null;
		m_SetDisconnected();
	}
	
	private void m_SetConnected()
	{
		connect.Sensitive=false;
		disconnect.Sensitive=true;
		generalinfobutton.Sensitive=true;
		taxesbutton.Sensitive=true;
		categoriesbutton.Sensitive=true;
		productsbutton.Sensitive=true;
		addnew.Sensitive=true;
		properties.Sensitive=true;
		delete.Sensitive=true;
		refresh.Sensitive=true;
		m_MainWidget.Sensitive = true;
	}
	
	private void m_SetDisconnected()
	{
		connect.Sensitive=true;
		disconnect.Sensitive=false;
		generalinfobutton.Sensitive=false;
		taxesbutton.Sensitive=false;
		categoriesbutton.Sensitive=false;
		productsbutton.Sensitive=false;
		addnew.Sensitive=false;
		properties.Sensitive=false;
		delete.Sensitive=false;
		refresh.Sensitive=false;
		
		mainpaned.Remove(m_MainWidget);
		m_MainWidget.Destroy();
		m_MainWidget = new DisconnectedViewWidget();
		mainpaned.Add2(m_MainWidget);
		m_MainWidget.Sensitive = false;
	}
		
	private void MsgDlg(string text)
	{
		MessageDialog dlg = new MessageDialog( null, 
		                                      Gtk.DialogFlags.DestroyWithParent,
		                                      Gtk.MessageType.Info,
		                                      Gtk.ButtonsType.Close,
		                                      text );
		dlg.Run();
		dlg.Destroy();
	}

	// Callbacks
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		m_Disconnect();
		Application.Quit ();
		a.RetVal = true;
	}

	protected virtual void OnConnect (object sender, System.EventArgs e)
	{
		m_Connect();
	}

	protected virtual void OnDisconnect (object sender, System.EventArgs e)
	{
		m_Disconnect();
	}

	protected virtual void OnExit (object sender, System.EventArgs e)
	{
		m_Disconnect();
		Application.Quit ();
	}

	protected virtual void OnGeneralInfoClicked (object sender, System.EventArgs e)
	{
		if ( !(m_MainWidget is GeneralInfoViewWidget))
		{
			mainpaned.Remove(m_MainWidget);
			m_MainWidget.Destroy();
			m_MainWidget = new GeneralInfoViewWidget( m_DataBase );
			mainpaned.Add2(m_MainWidget);
		}
	}
	
	protected virtual void OnTaxesClicked (object sender, System.EventArgs e)
	{
		if ( !(m_MainWidget is TaxesViewWidget))
		{
			mainpaned.Remove(m_MainWidget);
			m_MainWidget.Destroy();
			m_MainWidget = new TaxesViewWidget(m_DataBase);
			mainpaned.Add2(m_MainWidget);
		}
	}
	
	protected virtual void OnCategoriesClicked (object sender, System.EventArgs e)
	{
		if ( !(m_MainWidget is CategoriesViewWidget))
		{
			mainpaned.Remove(m_MainWidget);
			m_MainWidget.Destroy();
			m_MainWidget = new CategoriesViewWidget(m_DataBase);
			mainpaned.Add2(m_MainWidget);
		}
	}
	
	protected virtual void OnProductsClicked (object sender, System.EventArgs e)
	{
		if ( !(m_MainWidget is ProductsViewWidget))
		{
			mainpaned.Remove(m_MainWidget);
			m_MainWidget.Destroy();
			m_MainWidget = new ProductsViewWidget(m_DataBase);
			mainpaned.Add2(m_MainWidget);
		}
	}

	protected virtual void OnAdd (object sender, System.EventArgs e)
	{
		if ( m_MainWidget != null )
		{
			m_MainWidget.AddNew();
		}
	}

	protected virtual void OnProperties (object sender, System.EventArgs e)
	{
		if ( m_MainWidget != null )
		{
			m_MainWidget.ModifyProperties();
		}
	}

	protected virtual void OnDelete (object sender, System.EventArgs e)
	{
		if ( m_MainWidget != null )
		{
			m_MainWidget.DeleteSelected();
		}
	}

	protected virtual void OnRefresh (object sender, System.EventArgs e)
	{
		if ( m_MainWidget != null )
		{
			m_MainWidget.RefreshView();
		}
	}

	protected virtual void OnPreferences (object sender, System.EventArgs e)
	{
		SettingsDialog dlg = new SettingsDialog( m_ConfigSrc );
		dlg.Run();
		dlg.Destroy();
	}
}