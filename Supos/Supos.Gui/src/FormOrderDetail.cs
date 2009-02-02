// FormOrderDetail.cs created with MonoDevelop
// User: xavier at 03:45 10/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;
using Supos.Core;

namespace Supos.Gui
{
	
	
	public class FormOrderDetail : Gtk.Table
	{
		private SuposDb ds;
		
		
		private ComboBoxTaxes combotax;
		protected Gtk.SpinButton spinprice;
		protected Gtk.SpinButton spinquant;
		
		public FormOrderDetail() : base(3, 2, false)
		{
			
			
			Label labeltax;
			labeltax = new Label("Tax :");
			labeltax.SetAlignment(0, 0.5f);
			
			Label labelprice;
			labelprice = new Label("Price :");
			labelprice.SetAlignment(0, 0.5f);
			
			Label labelquant;
			labelquant = new Label("Quantity :");
			labelquant.SetAlignment(0, 0.5f);
			
			combotax = new ComboBoxTaxes();
			
			spinprice = new SpinButton(0.0f, (double)System.Decimal.MaxValue, 0.01f);
			spinprice.Digits = 2;
			
			spinquant = new SpinButton(0.0f, (double)System.Int64.MaxValue, 1.0f);
			spinquant.Digits = 0;
			
			this.BorderWidth = 6;
			this.ColumnSpacing = 6;
			
			this.Attach(labeltax, 0, 1, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			this.Attach(labelprice, 0, 1, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			this.Attach(labelquant, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			this.Attach(combotax, 1, 2, 0, 1, AttachOptions.Expand|AttachOptions.Fill, AttachOptions.Expand|AttachOptions.Fill, 0, 0);
			this.Attach(spinprice, 1, 2, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			this.Attach(spinquant, 1, 2, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			
			this.ShowAll();
		}
		
		public SuposDb DataSource
		{
			get { return ds; }
			set
			{
				ds = value;
				combotax.DataSource = ds;
			}
		}
		
		public object TaxId
		{
			get { return combotax.GetActiveId(); }
		}
		
		public Decimal Price
		{
			get { return (Decimal)spinprice.Value; }
			set { spinprice.Value = (double)value; }
		}
		
		public Int64 Quantity
		{
			get { return (Int64)spinquant.Value; }
			set { spinquant.Value = value; }
		}
		
		public void SetDataFromOrderDetail( SuposDataSet.OrderDetailsRow detail)
		{
			if( detail == null)
				return;
			combotax.SelectFromId( detail.TaxId.ToString() );
			spinprice.Value = (double)detail.Price;
			spinquant.Value = (double)detail.Quantity;
		}
	}
}
