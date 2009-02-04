// MainWindow.cs created with MonoDevelop
// User: xavier at 18:23 19/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using System.Data;
using Gtk;
using Supos.Core;
using Supos.Gui;

namespace SuposUser
{
	public class MainWindow: Gtk.Window
	{	
		private ActionGroup actgroup = null;
		private UIManager uim = null;
		
		private VBox mainBox;		
		private HPaned mainPaned;
		private ViewNameIcon catview;
		private ViewNameIcon prodview;
		private ViewOrderEdit orderview;
		
		private bool fullscreen = false;
		
		private SuposDb database;
		
		public MainWindow (): base (Gtk.WindowType.Toplevel)
		{
			// this window
			this.Title= "Supos";
			this.DeleteEvent += OnDeleteEvent;
			// main vbox
			mainBox = new VBox(false, 0);
			this.Add(mainBox);
			// actiongroup and uimanager stuff (menubar)
			actgroup = new ActionGroup ("TestActions");
			SetUpActionGroup();			
			uim = new UIManager ();
			uim.InsertActionGroup (actgroup, 0);
			this.AddAccelGroup(uim.AccelGroup);
			SetUpUiManager();
			Gtk.Widget menubar = uim.GetWidget("/MenuBar");
			mainBox.PackStart(menubar, false, false, 0);
			actgroup.GetAction("disconnect").Sensitive=false;
			// main panned view
			mainPaned = new HPaned();
			mainPaned.Sensitive = false;
			mainPaned.Name = "toucharea";			
			mainBox.PackStart(mainPaned, true, true, 0);
			// order editing view
			orderview = new ViewOrderEdit();
			mainPaned.Pack2(orderview, false, false);
			// categories product paned view
			HPaned hpan2;
			hpan2 = new HPaned();
			mainPaned.Pack1(hpan2, true, false);
			// categories view	
			catview = new ViewNameIcon();
			catview.DataMember="Categories";
			catview.SelectionChanged += this.OnCatSelectionChanged;
			catview.WidthRequest= 200;
			hpan2.Pack1(catview, false, false);
			// products view
			prodview = new ViewNameIcon();
			prodview.DataMember = "Products";
			prodview.RowActivated += this.OnProdRowActivated;			
			prodview.WidthRequest= 400;
			hpan2.Pack2(prodview, true, false);
			// status bar
			Statusbar statusbar;
			statusbar = new Statusbar();
			mainBox.PackStart(statusbar, false, false, 0);
			// clock
			Clock clock;
			clock = new Clock();
			clock.BorderWidth = 6;			
			statusbar.PackStart(clock, false, false, 0);
			// END build interface
			
			this.ApplyViewPreferences(SettingsHandler.Settings.viewSettings);
			
		}
		
		private void SetUpActionGroup()
		{
			ActionEntry[] entries = new ActionEntry[] {
				new ActionEntry ("FileMenuAction", null, "_File", null, null, null),
				new ActionEntry ("EditMenuAction", null, "_Edit", null, null, null),
				new ActionEntry ("ViewMenuAction", null, "_View", null, null, null),
				new ActionEntry ("HelpMenuAction", null, "_Help", null, null, null),
				// File
				new ActionEntry ("connect", Stock.Connect, null, "<control>C", "Connect to Database", new EventHandler (OnConnect)),
				new ActionEntry ("disconnect", Stock.Disconnect, null, "<control>D", "Disconnect from Database", new EventHandler (OnDisconnect)),
				new ActionEntry ("quit", Stock.Quit, null, "<control>Q", "Quit the application", new EventHandler (OnQuit)),
				// Edit
				new ActionEntry ("preferences", Stock.Preferences, null, "<control>P", "Set application preferences", new EventHandler (OnPreferences)),
				// View
				new ActionEntry ("fullscreen", Stock.Fullscreen, null, "<control>F", "Go fullscreen", new EventHandler (OnFullScreen)),
				// Help
				new ActionEntry ("about", Stock.About, null, "<control>A", "Information about the application", new EventHandler (OnAbout)),
			};
			actgroup.Add (entries);
		}
		
		private void SetUpUiManager()
		{
			string ui_info = 
			"<ui>" +
			"  <menubar name='MenuBar'>\n" +
			"    <menu name=\"file\" action=\"FileMenuAction\">\n" +
			"      <menuitem name=\"connect\" action=\"connect\" />\n" +
			"      <menuitem name=\"disconnet\" action=\"disconnect\" />\n" +
			"      <separator name=\"separator1\" />\n" +
			"      <menuitem name=\"quit\" action=\"quit\" />\n" +
			"    </menu>\n" +
			"    <menu name=\"edit\" action=\"EditMenuAction\">\n" +
			"      <menuitem name=\"preferences\" action=\"preferences\" />\n" +
			"    </menu>\n" +
			"    <menu name=\"view\" action=\"ViewMenuAction\">\n" +
			"      <menuitem name=\"fullscreen\" action=\"fullscreen\" />\n" +
			"    </menu>\n" +
			"    <menu name=\"help\" action=\"HelpMenuAction\">\n" +
			"      <menuitem name=\"about\" action=\"about\" />\n" +
			"    </menu>\n" +
			"  </menubar>\n" +
			"  <toolbar name=\"toolbar\">\n" +
			"  </toolbar>\n" +
			"</ui>";
			uim.AddUiFromString (ui_info);
		}
		
		protected void ApplyViewPreferences( ViewSettings config)
		{
			string rcstring = "style \"touch-style\"{font_name = \"";
			rcstring += config.Font;
			rcstring += "\"} widget \"*.toucharea.*\" style \"touch-style\"";
			Gtk.Rc.ParseString( rcstring);
			this.ResetRcStyles();
		}
		
		private void Connect()
		{
			if( database == null) {
				try {
					database = new SuposDb(SettingsHandler.Settings.dbSettings);
					database.Fill();
				} catch (Exception e) {
					DialogError dlg = new DialogError( "Error while connecting/loading",e, this);
					dlg.Run();
					dlg.Destroy();
					Disconnect();
					return;
				}
				catview.DataSource=database;
				catview.SelectFrist();
				prodview.DataSource = database;
				orderview.DataSource = database;
				mainPaned.Sensitive = true;
				actgroup.GetAction("connect").Sensitive=false;
				actgroup.GetAction("disconnect").Sensitive=true;
			}
		}
		
		private void Disconnect()
		{
			if( database != null) {
				database = null;
				catview.DataSource = null;
				prodview.DataSource = null;
				orderview.DataSource = null;
				mainPaned.Sensitive = false;
				actgroup.GetAction("connect").Sensitive=true;
				actgroup.GetAction("disconnect").Sensitive=false;
			}
		}
		
		// Callbacks
		protected void OnQuit (object obj, EventArgs args)
		{
			Application.Quit ();
		}
		
		protected void OnPreferences (object obj, EventArgs args)
		{
			DialogPreferences dialog = new DialogPreferences(this);
			dialog.LoadSettings( SettingsHandler.Settings );
			int result = dialog.Run();
			if( result == (int)ResponseType.Ok)
			{
				dialog.ApplySettings( SettingsHandler.Settings );
				SettingsHandler.Save();
				this.ApplyViewPreferences(SettingsHandler.Settings.viewSettings);
			}			
			dialog.Destroy();
		}
		
		protected void OnAbout (object obj, EventArgs args)
		{
		}
		
		protected void OnFullScreen (object obj, EventArgs args)
		{
			if(this.fullscreen) {
				this.Unfullscreen();
				this.fullscreen = false;
			}
			else {
				this.Fullscreen();
				this.fullscreen = true;
			}
		}
		
		protected void OnConnect (object obj, EventArgs args)
		{
			Connect();
		}
		
		protected void OnDisconnect (object obj, EventArgs args)
		{
			Disconnect();
		}
		
		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}

		protected virtual void OnCatSelectionChanged (object sender, System.EventArgs e)
		{
			prodview.FilterColumn= "CategoryId";
			DataRow row = (sender as ViewNameIcon).SelectedRow;
			if (row != null)
				prodview.FilterValue = row["Id"].ToString();
		}

		protected virtual void OnProdRowActivated (object sender, System.EventArgs e)
		{
			Util.DumpRow((sender as ViewNameIcon).SelectedRow );
			SuposDataSet.ProductsRow product = (SuposDataSet.ProductsRow)(sender as ViewNameIcon).SelectedRow;
			SuposDataSet.OrdersRow order = orderview.ActiveOrder;
			SuposDataSet.OrderDetailsRow detail = database.AddProductInOrder( order, product);
			if( detail != null)
			{
				orderview.Reload();
				orderview.SelectDetail(detail);
			}
		}
		
		
	}
}