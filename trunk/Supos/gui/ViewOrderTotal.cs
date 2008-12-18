// ViewOrderTotal.cs created with MonoDevelop
// User: xavier at 15:30 8/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace Supos
{
	
	
	public class ViewOrderTotal : Frame
	{
		private Entry entryprice;
		private Entry entrytax;
		private Entry entryttcprice;
		
		public ViewOrderTotal() : base()
		{
			HBox hb = new HBox();
			
			Label label2 = new Label("Price:");
			Label label3 = new Label("Tax:");
			Label label4 = new Label("TTC:");
			
			
			entryprice = new Entry();
			entryprice.WidthRequest = 50;
			entryprice.IsEditable = false;
			entrytax = new Entry();
			entrytax.WidthRequest = 50;
			entrytax.IsEditable = false;
			entryttcprice = new Entry();
			entryttcprice.WidthRequest = 50;
			entryttcprice.IsEditable = false;
			
			this.Label = "Total";
			this.Add(hb);
			hb.BorderWidth = 6;
			//hb.PackStart(label1);
			hb.PackStart(label2, false, true, 6);
			hb.PackStart(entryprice, true, true, 0);
			hb.PackStart(label3, false, true, 6);
			hb.PackStart(entrytax, true, true, 0);
			hb.PackStart(label4, false, true, 6);
			hb.PackStart(entryttcprice, true, true, 0);
		}
		
		public void SetOrder(SuposDataSet.OrdersRow order)
		{
			OrderTotal tot = Util.GetOrderTotal(order);
			this.entryprice.Text = tot.TotPrice.ToString();
			this.entrytax.Text = tot.TaxAmount.ToString();
			this.entryttcprice.Text = tot.TotPriceTaxInc.ToString();
		}
		
	}
}
