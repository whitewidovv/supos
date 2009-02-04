// CellRenderFunctions.cs created with MonoDevelop
// User: xavier at 08:47 6/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using Gtk;
using Gdk;
using Supos.Core;

namespace Supos
{
	
	
	static public class CellRenderFunctions
	{
		const int iconsize = 75;
		
		static public  void RenderName(Gtk.CellLayout cell_layout, Gtk.CellRenderer cell, Gtk.TreeModel tree_model, Gtk.TreeIter iter)
		{
			CellRendererText text = (CellRendererText)cell;
			DataRow row = (DataRow)tree_model.GetValue(iter, 0);
			if(row != null)
			{
				text.Text = row["Name"].ToString();
			}
		}
		
		static public  void RenderIcon(Gtk.CellLayout cell_layout, Gtk.CellRenderer cell, Gtk.TreeModel tree_model, Gtk.TreeIter iter)
		{
			CellRendererPixbuf pix = (CellRendererPixbuf)cell;
			DataRow row = (DataRow)tree_model.GetValue(iter, 0);
			if(row != null)
			{
				byte[] icon = SuposDb.GetMedia( row["icon"].ToString() );
				if ( icon != null )
					pix.Pixbuf = new Gdk.Pixbuf( icon ).ScaleSimple(iconsize, iconsize, Gdk.InterpType.Bilinear);
				else
					pix.Pixbuf = null;
			}
		}
		
		static public  void RenderOrder(Gtk.CellLayout cell_layout, Gtk.CellRenderer cell, Gtk.TreeModel tree_model, Gtk.TreeIter iter)
		{
			CellRendererText celltxt = (CellRendererText)cell;
			SuposDataSet.OrdersRow order = (SuposDataSet.OrdersRow)tree_model.GetValue(iter, 0);
			if (order != null)
			{
				celltxt.Markup = "<b>" + order.CustomersRow.Name + ": </b>" + order.Id;				
			}				
		}
		
		static public  void RenderTax(Gtk.CellLayout cell_layout, Gtk.CellRenderer cell, Gtk.TreeModel tree_model, Gtk.TreeIter iter)
		{
			CellRendererText celltxt = (CellRendererText)cell;
			SuposDataSet.TaxesRow tax = (SuposDataSet.TaxesRow)tree_model.GetValue(iter, 0);
			if (tax != null)
			{
				celltxt.Markup = "<b>" + tax.Name + ": </b> " + (tax.Rate*100) + "%";				
			}
			else
				celltxt.Markup = "<b>None</b>";		
		}
		
		static public  void RenderCustomer(Gtk.CellLayout cell_layout, Gtk.CellRenderer cell, Gtk.TreeModel tree_model, Gtk.TreeIter iter)
		{
			CellRendererText celltxt = (CellRendererText)cell;
			SuposDataSet.CustomersRow customer = (SuposDataSet.CustomersRow)tree_model.GetValue(iter, 0);
			if (customer != null)
			{
				celltxt.Markup = "<b>" + customer.Name + "</b>";				
			}
		}

		static public  void RenderPayment(Gtk.CellLayout cell_layout, Gtk.CellRenderer cell, Gtk.TreeModel tree_model, Gtk.TreeIter iter)
		{
			CellRendererText celltxt = (CellRendererText)cell;
			SuposDataSet.PaymentsRow payment = (SuposDataSet.PaymentsRow)tree_model.GetValue(iter, 0);
			if (payment != null)
			{
				celltxt.Markup = "<b>" + payment.Name + "</b>";
				celltxt.Sensitive = payment.Allowed;
			}
			else
			{
				celltxt.Markup = "<b>None</b>";
				celltxt.Sensitive = true;
			}
		}
		
		
		
		static public void RenderOrderDetailProduct(TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter)
		{
			CellRendererText celltxt = (CellRendererText)cell;
			SuposDataSet.OrderDetailsRow row = (SuposDataSet.OrderDetailsRow)tree_model.GetValue(iter, 0);
			if(row != null)
				celltxt.Text = row.ProductsRow.Name;
		}
		
		static public void RenderOrderDetailTax(TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter)
		{
			SuposDataSet.OrderDetailsRow row = (SuposDataSet.OrderDetailsRow)tree_model.GetValue(iter, 0);
			if(row != null)
				(cell as Gtk.CellRendererText).Text = (row.TaxesRow.Rate * row.Price).ToString();
		}
		
		static public void RenderOrderDetailPrice(TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter)
		{
			SuposDataSet.OrderDetailsRow row = (SuposDataSet.OrderDetailsRow)tree_model.GetValue(iter, 0);
			if(row != null)
				(cell as Gtk.CellRendererText).Text = row["Price"].ToString();
		}
		
		static public void RenderOrderDetailPriceTaxInc(TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter)
		{
			SuposDataSet.OrderDetailsRow row = (SuposDataSet.OrderDetailsRow)tree_model.GetValue(iter, 0);
			if(row != null)
				(cell as Gtk.CellRendererText).Text = ((1+row.TaxesRow.Rate)*row.Price).ToString();
		}
		
		static public void RenderOrderDetailQuantity(TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter)
		{
			SuposDataSet.OrderDetailsRow row = (SuposDataSet.OrderDetailsRow)tree_model.GetValue(iter, 0);
			if(row != null)
				(cell as Gtk.CellRendererText).Text = row["Quantity"].ToString();
		}
		
		static public void RenderOrderDetailTotalPrice(TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter)
		{
			SuposDataSet.OrderDetailsRow row = (SuposDataSet.OrderDetailsRow)tree_model.GetValue(iter, 0);
			if(row != null)
				(cell as Gtk.CellRendererText).Text = (row.Price*row.Quantity).ToString();
		}
		
		static public void RenderOrderDetailTotalTax(TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter)
		{
			SuposDataSet.OrderDetailsRow row = (SuposDataSet.OrderDetailsRow)tree_model.GetValue(iter, 0);
			if(row != null)
				(cell as Gtk.CellRendererText).Text = (row.TaxesRow.Rate * row.Price * row.Quantity).ToString();
		}
		
		static public void RenderOrderDetailTotalPriceTaxInc(TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter)
		{
			SuposDataSet.OrderDetailsRow row = (SuposDataSet.OrderDetailsRow)tree_model.GetValue(iter, 0);
			if(row != null)
				(cell as Gtk.CellRendererText).Text = ( (1+row.TaxesRow.Rate) * row.Price * row.Quantity).ToString();
		}
	}
}
