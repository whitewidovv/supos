// FormOrder.cs created with MonoDevelop
// User: xavier at 05:44 6/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;
using Supos.Core;

namespace Supos.Gui
{
	
	
	public class FormOrder : Gtk.Table
	{
		private SuposDb ds;
		
		private Label labelid;
		private Label labelcust;
		private Label labelpay;
		private Label labeltax ;
		
		private Entry entryid;
		private ComboBoxCustomers combocust;
		private ComboBoxPayments combopay;
		private ComboBoxTaxes combotax;
		
		public FormOrder() : base(4,2, false)
		{
			labelid = new Label("Id :");
			labelid.SetAlignment(0, (float)0.5);
			labelcust = new Label("Customer :");
			labelcust.SetAlignment(0, (float)0.5);
			labelpay = new Label("Payment :");
			labelpay.SetAlignment(0, (float)0.5);
			labeltax = new Label("Tax :");
			labeltax.SetAlignment(0, (float)0.5);
			
			entryid = new Entry();
			entryid.Sensitive = false;
			
			combocust = new ComboBoxCustomers();
			combopay = new ComboBoxPayments();
			combopay.NoneRow = true;
			combotax = new ComboBoxTaxes();
			combotax.NoneRow = true;
			
			this.ColumnSpacing = 6;
			Attach(labelid, 0, 1, 0, 1);
			Attach(labelcust, 0, 1, 1, 2);
			Attach(labelpay, 0, 1, 2, 3);
			Attach(labeltax, 0, 1, 3, 4);

			Attach(entryid, 1, 2, 0, 1);
			Attach(combocust, 1, 2, 1, 2);
			Attach(combopay, 1, 2, 2, 3);
			Attach(combotax, 1, 2, 3, 4);
			
			this.ShowAll();
		}
		
		public SuposDb DataSource
		{
			get { return ds; }
			set
			{
				ds = value;
				combotax.DataSource = ds;
				combocust.DataSource = ds;
				combopay.DataSource = ds;
			}
		}
		
		public object CustomerId
		{
			get { return combocust.GetActiveId(); }
		}
		
		public object PaymentId
		{
			get { return combopay.GetActiveId(); }
		}
		
		public object TaxId
		{
			get { return combotax.GetActiveId(); }
		}
		
		public void SelectFirsts()
		{
			combocust.SelectFirst();
			combopay.SelectFirst();
			combotax.SelectFirst();
		}
		
		public void SetDataFromOrder( SuposDataSet.OrdersRow order)
		{
			if( order == null)
				return;
			entryid.Text = order.Id;
			combocust.SelectFromId( order.CustomerId.ToString() );
//			if( order["PaymentId"] != System.DBNull.Value )
				combopay.SelectFromId( order["PaymentId"].ToString() );
//			else
//				combopay.SelectFirst();
//			if( order["TaxId"] != System.DBNull.Value )
				combotax.SelectFromId( order["TaxId"].ToString() );
//			else
//				combotax.SelectFirst();
		}
		
	}
}
