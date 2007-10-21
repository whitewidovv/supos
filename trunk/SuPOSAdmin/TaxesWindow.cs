// /home/xavier/Projects/SuPOS/SuposAdmin/TaxesWindow.cs
// User: xavier at 15:29 18/10/2007
//

using System;
using System.Collections;
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
			TaxRateColumn.Title = "Rate %";
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
			m_CreateTaxView();
		}
		
		//****************************************
		// Show window
		//****************************************
		public void Show()
		{
			taxeswindow.ShowAll();
		}
		
		//******************************************
		// Clear TreeView
		//******************************************
		private void m_ClearView()
		{
			if ( m_Store != null)
			{
				m_Store.Clear();
			}
		}
		
		//******************************************
		// Taxes in TreeView
		//******************************************
		private void m_CreateTaxView()
		{
			if ( m_DataBase != null)
			{	
				ArrayList taxes = m_DataBase.GetTaxes();
				if ( taxes != null )
				{
					foreach (SuposTax tax in taxes )
					{
						m_Store.AppendValues(tax.Id.ToString(), tax.Name, tax.Rate.ToString(), tax);
					}
				}
			}
		}
		
		//********************************************
		// CALLBACKS
		//********************************************
		#pragma warning disable 0169
		private void OnAddClicked (object sender, EventArgs a)
		{
			TaxDialog dlg = new TaxDialog();
			int result = dlg.Run();
			if ( (ResponseType)result == ResponseType.Ok)
			{
				TreeIter iter;
				m_DataBase.AddTax( dlg.Tax );
				// Update view
				iter = m_Store.AppendValues(dlg.Tax.Id.ToString(), dlg.Tax.Name, dlg.Tax.Rate.ToString(), dlg.Tax);
				// Select new inserted row
				taxestreeview.Selection.SelectIter( iter );
			}
			dlg.Destroy();
			
		}
		
		
		private void OnModifyClicked (object sender, EventArgs a)
		{
			TreeIter iter;
			TreeModel model;
			TreePath[] path_array = taxestreeview.Selection.GetSelectedRows(out model);
			
			if ( path_array.Length>0  )
			{		
				model.GetIter(out iter, path_array[0]);
			    SuposTax tax = (SuposTax) model.GetValue (iter, (int)TaxColumn.Data );
			    TaxDialog dlg = new TaxDialog( tax );
				int result = dlg.Run();
				if ( (ResponseType)result == ResponseType.Ok)
				{
					tax.ApplyChange();
					// Update of the row
					model.SetValue(iter, (int)TaxColumn.Id, tax.Id.ToString() );
					model.SetValue(iter, (int)TaxColumn.Name, tax.Name);
					model.SetValue(iter, (int)TaxColumn.Rate, tax.Rate.ToString() );
					//model.EmitRowChanged(path_array[0], iter);
				}
				dlg.Destroy();
			}
		}

	
		private void OnRefreshClicked (object sender, EventArgs a)
		{
			m_ClearView();
			m_CreateTaxView();
		}
		
		
		private void OnDeleteClicked (object sender, EventArgs a)
		{
			TreeIter iter;
			TreeModel model;
			ArrayList rowlist = new ArrayList();
			// boucle sur la selection
			TreePath[] path_array = taxestreeview.Selection.GetSelectedRows(out model);
			foreach ( TreePath path in path_array )
			{		
				model.GetIter(out iter, path);
				SuposTax tax = (SuposTax) model.GetValue(iter, (int)TaxColumn.Data );
			    m_DataBase.Remove(tax); // remove from DB
				rowlist.Add( new TreeRowReference(model, path) ); //mark row for deletion
			}
			// Delete marked rows
			ListStore store = (ListStore)model;
			foreach ( TreeRowReference row in rowlist)
			{
				store.GetIter(out iter, row.Path);
				store.Remove(ref iter);
			}
		}
		
		
	}
}
