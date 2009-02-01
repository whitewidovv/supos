// TicketView.cs created with MonoDevelop
// User: xavier at 01:20 5/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using Gtk;

namespace Supos
{
	
	
	public class ViewOrderEdit : VBox
	{
		private SuposDb ds;
		private ComboBoxOrders comboorder;
		private Button butneworder;
		private Button buteditorder;
		private Button butdelorder;
		private ViewOrderDetails view;
		private KeyPad pad;
		private ViewOrderTotal viewtot;
		private ViewOrderDetailEdit viewdet;
		
		public ViewOrderEdit() : base()
		{
			comboorder = new ComboBoxOrders();
			comboorder.Changed += OnOrderChanged;
			
			butneworder = new Button();
			butneworder.Add( new Image(Stock.New, IconSize.Button) );
			butneworder.Clicked += OnNewOrderClicked;
			
			buteditorder = new Button();
			buteditorder.Add( new Image(Stock.Edit, IconSize.Button) );
			buteditorder.Clicked += OnEditOrderClicked;

			butdelorder = new Button();
			butdelorder.Add( new Image(Stock.Delete, IconSize.Button) );
			butdelorder.Clicked += OnDeleteOrderClicked;

			HBox hb = new HBox();
			hb.PackStart(butdelorder, false, false, 0);
			hb.PackStart(comboorder,true, true, 0 );
			hb.PackStart(butneworder, false, false, 0 );
			hb.PackStart(buteditorder, false,false,0 );
			
			viewtot = new ViewOrderTotal();
			
			view = new ViewOrderDetails();
			view.HeaderVisible = true;
			view.FilterColumn="OrderId";			
			view.SelectionChanged += OnOrderDetailChanged;
			
			Frame frame = new Frame();
			frame.Label = "Oder item";
			viewdet = new ViewOrderDetailEdit();
			frame.Add(viewdet);
			
			pad = new KeyPad();
			pad.Clicked += OnPadClicked;
			
			PackStart(hb, false, true, 0);
			PackStart(viewtot, false, true, 3);
			PackStart(view, true, true, 3);
			PackStart(frame, false, false,3);
			PackStart(pad, false, true, 3);
		}
		
		public SuposDb DataSource
		{
			get { return ds; }
			set
			{
				ds = value;
				comboorder.DataSource = ds;
				comboorder.SelectFirst();
				view.DataSource = ds;
				viewdet.DataSource = ds;
			}
		}
		
		public SuposDataSet.OrdersRow ActiveOrder
		{
			get
			{ 
				SuposDataSet.OrdersRow order = (SuposDataSet.OrdersRow) this.comboorder.GetActiveRow();
				return order;
			}
		}
		
		public void SelectDetail( SuposDataSet.OrderDetailsRow detail)
		{
			view.Select(detail);
		}
		
		private void OnOrderChanged(object sender, EventArgs args)
		{
			SuposDataSet.OrdersRow order = (SuposDataSet.OrdersRow) this.comboorder.GetActiveRow();
			view.FilterValue = comboorder.GetActiveId().ToString();
			viewtot.SetOrder( order );
		}
		
		private void OnOrderDetailChanged(object sender, EventArgs args)
		{
			SuposDataSet.OrderDetailsRow detail = (SuposDataSet.OrderDetailsRow)this.view.GetSelectedRow();
			viewdet.SetDataFromOrderDetail(detail);
		}
		
		private void OnEditOrderClicked(object sender, EventArgs args)
		{			
			SuposDataSet.OrdersRow order = (SuposDataSet.OrdersRow) this.comboorder.GetActiveRow();
			if( order == null)
				return;
			DialogOrder dialog = new DialogOrder(null);
			dialog.DataSource = this.DataSource;
			dialog.Form.SetDataFromOrder (order);
			int result = dialog.Run();
			if( result == (int)ResponseType.Ok)
			{
				order["CustomerId"] = dialog.Form.CustomerId;
				order["PaymentId"] = dialog.Form.PaymentId;
				order["TaxId"] = dialog.Form.TaxId;
				this.ds.SaveOrders();
				this.comboorder.Reload();
			}
			dialog.Destroy();
		}
		
		private void OnNewOrderClicked(object sender, EventArgs args)
		{
			DialogOrder dialog = new DialogOrder(null);			
			dialog.DataSource = this.DataSource;
			dialog.Form.SelectFirsts();
			int result = dialog.Run();
			if( result == (int)ResponseType.Ok)
			{
				SuposDataSet.OrdersRow row = this.DataSource.NewOrder();
				row["CustomerId"] = dialog.Form.CustomerId;
				row["PaymentId"] = dialog.Form.PaymentId;
				row["TaxId"] = dialog.Form.TaxId;
				this.ds.AddOrder(row);
				this.ds.SaveOrders();
				this.comboorder.Reload();
				this.comboorder.Select(row);
			}
			dialog.Destroy();
		}
		
		private void OnDeleteOrderClicked(object sender, EventArgs args)
		{
			SuposDataSet.OrdersRow order = (SuposDataSet.OrdersRow) this.comboorder.GetActiveRow();
			if( order == null)
				return;
			Dialog dialog = new  MessageDialog(null, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Are you sure you want to delete this order ?");
			int result = dialog.Run();
			if( result == (int)ResponseType.Yes)
			{
				order.Delete();
				this.ds.SaveOrders();
				this.comboorder.Reload();
			}
			dialog.Destroy();
		}

		public void OnPadClicked(object sender, KeyPad.KeypadEventArgs args)
		{
			switch(args.code) {
			case KeyPad.KeyCode.Ok:
				ApplyDetailChange();
				break;
			default :
				this.viewdet.HandleKeypad(args.code);
				break;
			}
		}
		
		private void ApplyDetailChange()
		{
			SuposDataSet.OrderDetailsRow detail = (SuposDataSet.OrderDetailsRow)view.GetSelectedRow();
			if(detail!=null)
			{
				detail.Price= viewdet.Price;
				detail.TaxId= (Int64)viewdet.TaxId;
				detail.Quantity = viewdet.Quantity;
				view.Reload();
			}
		}
		
		public void Reload()
		{
			this.comboorder.Reload();
			this.view.Reload();
		}
	}
}
