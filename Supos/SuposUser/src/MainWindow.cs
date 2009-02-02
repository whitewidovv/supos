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
	public partial class MainWindow: Gtk.Window
	{	
		private SuposDb database;
		private ViewNameIcon catview;
		private ViewNameIcon prodview;
		private ViewOrderEdit orderview;
		
		public MainWindow (): base (Gtk.WindowType.Toplevel)
		{
			
			// build interface
			VBox vbox;
			vbox = new VBox();

			Statusbar statusbar;
			statusbar = new Statusbar();
			//statusbar.BorderWidth = 6;
			
			HPaned hpan1;
			hpan1 = new HPaned();
			hpan1.Name = "toucharea";
			
			
			HPaned hpan2;
			hpan2 = new HPaned();
			
			Clock clock;
			clock = new Clock();
			clock.BorderWidth = 6;
				
			catview = new ViewNameIcon();
			prodview = new ViewNameIcon();
			orderview = new ViewOrderEdit();
			
			hpan2.Add1(catview);
			hpan2.Add2(prodview);
			hpan1.Add1(hpan2);
			hpan1.Add2(orderview);
			statusbar.PackStart(clock, false, true, 0);
			vbox.PackStart(hpan1, true, true, 0);
			vbox.PackStart(statusbar, false, true, 0);
			this.Add(vbox);
			this.DeleteEvent += OnDeleteEvent;
			
			this.ShowAll();
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