// TicketSimpleView.cs created with MonoDevelop
// User: xavier at 20:19 5/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using Gtk;

namespace Supos
{
	
	
	public class ViewOrderDetails : ViewBase
	{
		
		public ViewOrderDetails() : base()
		{
			CellRenderer productrenderer = new CellRendererText();
			CellRenderer taxrenderer = new CellRendererText();
			taxrenderer.Xalign = 1;
			CellRenderer pricerenderer = new CellRendererText();
			pricerenderer.Xalign = 1;
			CellRenderer quantrenderer = new CellRendererText();
			quantrenderer.Xalign = 1;
			CellRenderer totpricerenderer = new CellRendererText();
			totpricerenderer.Xalign = 1;
			CellRenderer tottaxrenderer = new CellRendererText();
			tottaxrenderer.Xalign = 1;
			CellRenderer totpricettcrenderer = new CellRendererText();
			totpricettcrenderer.Xalign = 1;
			
			TreeViewColumn prodcol = new TreeViewColumn();
			prodcol.Title = "Product";
			prodcol.PackStart( productrenderer,true);
			prodcol.SetCellDataFunc(productrenderer, (TreeCellDataFunc)CellRenderFunctions.RenderOrderDetailProduct);
			prodcol.Expand = true;
			treeview.AppendColumn(prodcol);
			treeview.AppendColumn("Unit Price", pricerenderer, (TreeCellDataFunc)CellRenderFunctions.RenderOrderDetailPrice);
			treeview.AppendColumn("Unit Tax", taxrenderer, (TreeCellDataFunc)CellRenderFunctions.RenderOrderDetailTax);			
			treeview.AppendColumn("Unit Price TTC", taxrenderer, (TreeCellDataFunc)CellRenderFunctions.RenderOrderDetailPriceTaxInc);
			treeview.AppendColumn("Quantity", quantrenderer, (TreeCellDataFunc)CellRenderFunctions.RenderOrderDetailQuantity);
			treeview.AppendColumn("Price", totpricerenderer, (TreeCellDataFunc)CellRenderFunctions.RenderOrderDetailTotalPrice);
			treeview.AppendColumn("Tax", tottaxrenderer, (TreeCellDataFunc)CellRenderFunctions.RenderOrderDetailTotalTax);
			treeview.AppendColumn("Price TTC", totpricettcrenderer, (TreeCellDataFunc)CellRenderFunctions.RenderOrderDetailTotalPriceTaxInc);
		
			this.DataMember="OrderDetails";
			this.Show();
		}
		
		
		
	}
}
