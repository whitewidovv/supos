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
		
		private ViewNameIcon catview;
		private ViewNameIcon prodview;
		private ViewOrderEdit orderview;
		
		private SuposDb database;
		
		public MainWindow (): base (Gtk.WindowType.Toplevel)
		{
			// this window
			this.Title= "Supos";
			this.DeleteEvent += OnDeleteEvent;
			// main vbox
			VBox mainBox;
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
			// main panned view
			HPaned mainPaned;
			mainPaned = new HPaned();
			mainPaned.Name = "toucharea";			
			mainBox.PackStart(mainPaned, false, false, 0);
			// order editing view
			orderview = new ViewOrderEdit();
			mainPaned.Add2(orderview);
			// categories product paned view
			HPaned hpan2;
			hpan2 = new HPaned();
			mainPaned.Add1(hpan2);
			// categories view	
			catview = new ViewNameIcon();
			hpan2.Add1(catview);
			// products view
			prodview = new ViewNameIcon();
			hpan2.Add2(prodview);
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
				
			
			database = new SuposDb();
			database.Provider = new SuposDbProvider();
			database.Fill();
			
			catview.DataSource=database;
			catview.DataMember="Categories";
			catview.SelectionChanged += this.OnCatSelectionChanged;
			catview.SelectFrist();
			catview.WidthRequest= 100;
			
			prodview.DataSource = database;
			prodview.DataMember = "Products";
			prodview.RowActivated += this.OnProdRowActivated;			
			prodview.WidthRequest= 200;
			
			orderview.DataSource = database;
			
			this.ApplyViewPreferences();
			
		}
		
		private void SetUpActionGroup()
		{
			ActionEntry[] entries = new ActionEntry[] {
				new ActionEntry ("FileMenuAction", null, "_File", null, null, null),
				new ActionEntry ("EditMenuAction", null, "_Edit", null, null, null),
				new ActionEntry ("HelpMenuAction", null, "_Help", null, null, null),
				new ActionEntry ("quit", Stock.Quit, null, "<control>Q", "Quit the application", new EventHandler (OnQuit)),
				new ActionEntry ("preferences", Stock.Preferences, null, "<control>P", "Set application preferences", new EventHandler (OnPreferences)),
				new ActionEntry ("about", Stock.About, null, "<control>A", "Information about the application", new EventHandler (OnPreferences)),
			};
			actgroup.Add (entries);
		}
		
		private void SetUpUiManager()
		{
			string ui_info = 
			"<ui>" +
			"  <menubar name='MenuBar'>\n" +
			"    <menu name=\"file\" action=\"FileMenuAction\">\n" +
			"      <menuitem name=\"quit\" action=\"quit\" />\n" +
			"    </menu>\n" +
			"    <menu name=\"edit\" action=\"EditMenuAction\">\n" +
			"      <menuitem name=\"preferences\" action=\"preferences\" />\n" +
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
		
		protected void ApplyViewPreferences()
		{
			Gtk.Rc.ParseString("style \"touch-style\"{font_name = \"Sans 12\"} widget \"*.toucharea.*\" style \"touch-style\"");
			this.ResetRcStyles();
		}
		
		
		// Callbacks
		protected void OnQuit (object obj, EventArgs args)
		{
			Application.Quit ();
		}
		
		protected void OnPreferences (object obj, EventArgs args)
		{
			DialogPreferencesBase dialog = new DialogPreferencesBase(this);
			dialog.SetDatabaseSettingsFromConfig( System.Configuration.ConfigurationManager.AppSettings["ConnStr"] );
			int result = dialog.Run();
			if( result == (int)ResponseType.Ok)
			{
			}			
			dialog.Destroy();
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