// /home/xavier/Projects/SuPOS/SuPOSAdmin/MainWindow.cs created with MonoDevelop
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

namespace SuPOSAdmin
{
	
	
	public class MainWindow: Gtk.Window
	{
		private SuposDb m_DataBase = null;
		private Config m_Config = null;
		private uint m_StatusContextId;
		private ListStore m_MainStore = null;
		[Widget] protected Gtk.Window mainwindow;
		[Widget] protected Gtk.Statusbar statusbar;
		[Widget] protected Gtk.ComboBox combobox;
		[Widget] protected Gtk.TreeView maintreeview;
		
		
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
			combobox.Sensitive = false;
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
				m_DataBase.LoadCategories(); // TODO check result
				combobox.Sensitive = true;
				combobox.Active = 0;
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
			m_ClearView();
			combobox.Active = -1;
			combobox.Sensitive = false;
		}
		
	
		//******************************************
		// Clear MainTreeView
		//******************************************
		private void m_ClearView()
		{
			foreach ( TreeViewColumn c in maintreeview.Columns)
			{
				maintreeview.RemoveColumn(c);
				c.Destroy();
			}
			if ( m_MainStore != null)
			{
				m_MainStore.Clear();
				m_MainStore.Dispose();
			}
			m_MainStore = null;
		}
		
		//******************************************
		// Categories in MainTreeView
		//******************************************
		private void m_CreateCategoryView()
		{
			if ( m_DataBase != null)
			{
				m_MainStore = new ListStore ( typeof(Gdk.Pixbuf), typeof(string), typeof(SuposDbCategory) );
				maintreeview.Model = m_MainStore;
				Gtk.TreeViewColumn CategoryIconColumn = new Gtk.TreeViewColumn ();
				Gtk.TreeViewColumn CategoryNameColumn = new Gtk.TreeViewColumn ();
				CategoryIconColumn.Title = "Icon";
				CategoryNameColumn.Title = "Name";
				maintreeview.AppendColumn (CategoryIconColumn);
				maintreeview.AppendColumn (CategoryNameColumn);
				CellRendererPixbuf CategoryIconCell = new Gtk.CellRendererPixbuf ();
				CellRendererText CategoryNameCell = new Gtk.CellRendererText ();
				CategoryIconColumn.PackStart(CategoryIconCell, true);
				CategoryNameColumn.PackStart(CategoryNameCell, true);
				CategoryIconColumn.AddAttribute (CategoryIconCell, "pixbuf", 0);
				CategoryNameColumn.AddAttribute (CategoryNameCell, "text", 1);
				
				ArrayList categories = m_DataBase.GetCategories();
				if ( categories != null )
				{
					foreach (SuposDbCategory category in categories )
					{
						Pixbuf pb = category.Icon.GetPixbuf();
						if ( pb != null )
								pb = pb.ScaleSimple( 50, 50, Gdk.InterpType.Bilinear );
						m_MainStore.AppendValues(pb, category.Name, category);
					}
				}
			}
		}
		
		//********************************************
		// 
		//********************************************
		private void m_AddCategory()
		{
			CategoryDialog dlg = new CategoryDialog();
			int result = dlg.categorydialog.Run();
			if ( result == 1)
			{
				m_DataBase.AddCategory( dlg.Category );
				//update view
				m_ClearView();
				m_CreateCategoryView();
				
			}
			dlg.categorydialog.Destroy();
			
		}
		
		//*********************************************
		// 
		//*********************************************
		private void m_ModifyCategory()
		{
			TreeIter iter;
			TreeModel model;

			if ( maintreeview.Selection.GetSelected (out model, out iter) )
			{
			    SuposDbCategory cat = (SuposDbCategory) model.GetValue (iter, 2);
				//string cat = (string) model.GetValue (iter, 1);
				Console.Write(cat.Id);
				Console.WriteLine(cat.Name);
			    CategoryDialog dlg = new CategoryDialog( cat );
				int result = dlg.categorydialog.Run();
				if ( result == 1)
				{
					//update view
					m_ClearView();
					m_CreateCategoryView();
				}
				dlg.categorydialog.Destroy();
			}
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
		}
		
		private void OnMenuQuitActivated (object sender, EventArgs a) 
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
		
		private void OnVueSelectionChanged (object sender, EventArgs a)
		{
			if ( sender.GetType().Equals( typeof(Gtk.ComboBox) ) )
			{
				ComboBox box = (ComboBox) sender;
				switch (box.Active)
				{
				case 0:
					m_ClearView();
					m_CreateCategoryView();
					break;
				case 1:
					Console.WriteLine("Products");
					m_ClearView();
					break;
				case 2:
					Console.WriteLine("Orders");
					m_ClearView();
					break;
				
				}
				
			}
		}
		
		private void OnAddClicked (object sender, EventArgs a) 
		{
			switch (combobox.Active)
			{
			case 0:
				m_AddCategory();
				break;
			case 1:
				Console.WriteLine("AddProducts");
				break;
			}
			
		}
		
		private void OnPropertiesClicked (object sender, EventArgs a) 
		{
			switch (combobox.Active)
			{
			case 0:
				m_ModifyCategory();
				break;
			case 1:
				Console.WriteLine("ModProducts");
				break;
				/**/
				
			}
		}
	}
}
