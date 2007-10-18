// /home/xavier/Projects/SuPOS/SuposAdmin/TaxesWindow.cs
// User: xavier at 15:29 18/10/2007
//

using System;
using Mono.Unix;
using Gtk;
using Glade;
using LibSupos;

namespace SuposAdmin
{
	
	
	public class TaxesWindow
	{
		private SuposDb m_DataBase = null;
		private ListStore m_Store = null;
		[Widget] private Gtk.Window taxeswindow;
		[Widget] private Gtk.TreeView taxestreeview;
		
		enum TaxColumn {Id, Name, Rate, Data};
		
		
		//****************************
		// Constructor
		//****************************
		public TaxesWindow(SuposDb Db)
		{
			Catalog.Init("suposadmin","./locale");
			Glade.XML gxml = new Glade.XML (null, "suposadmin.glade", "taxeswindow", "suposadmin");
			gxml.Autoconnect (this);
			
			m_DataBase = Db;
			
			taxestreeview.Selection.Mode = Gtk.SelectionMode.Multiple;
			
			m_Store = new ListStore ( typeof(string), typeof(string), typeof(string), typeof(SuposTax) );
			taxestreeview.Model = m_Store;
			Gtk.TreeViewColumn TaxIdColumn = new Gtk.TreeViewColumn ();
			Gtk.TreeViewColumn TaxNameColumn = new Gtk.TreeViewColumn ();
			Gtk.TreeViewColumn TaxRateColumn = new Gtk.TreeViewColumn ();
			TaxIdColumn.Title = "ID";
			TaxNameColumn.Title = "Name";
			TaxRateColumn.Title = "Rate";
			taxestreeview.AppendColumn (TaxIdColumn);
			taxestreeview.AppendColumn (TaxNameColumn);
			taxestreeview.AppendColumn (TaxRateColumn);
			CellRendererText TaxIdCell = new Gtk.CellRendererText ();
			CellRendererText TaxNameCell = new Gtk.CellRendererText ();
			CellRendererText TaxRateCell = new Gtk.CellRendererText ();
			TaxIdColumn.PackStart(TaxIdCell, true);
			TaxNameColumn.PackStart(TaxNameCell, true);
			TaxRateColumn.PackStart(TaxRateCell, true);
			TaxIdColumn.AddAttribute (TaxIdCell, "text", (int)TaxColumn.Id );
			TaxNameColumn.AddAttribute (TaxNameCell, "text", (int)TaxColumn.Name );
			TaxRateColumn.AddAttribute (TaxRateCell, "text", (int)TaxColumn.Rate );
			TaxIdColumn.SortColumnId = (int)TaxColumn.Id;
			TaxNameColumn.SortColumnId = (int)TaxColumn.Name;
			TaxRateColumn.SortColumnId = (int)TaxColumn.Rate;
			//m_CreateTaxView();
		}
		
		//****************************************
		// Show window
		//****************************************
		public void Show()
		{
			taxeswindow.ShowAll();
		}
		
		
	}
}
