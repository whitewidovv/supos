// DialogOrder.cs created with MonoDevelop
// User: xavier at 05:59 6/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace Supos
{
	
	
	public class DialogOrder : Gtk.Dialog
	{
		private FormOrder form;
		
		public DialogOrder(Window parent) : base("Order", parent, DialogFlags.Modal, Stock.Ok, ResponseType.Ok, Stock.Cancel, ResponseType.Cancel)
		{
			form = new FormOrder();
			this.VBox.PackStart(form, true, true, 6);
			form.ShowAll();
		}
		
		public SuposDb DataSource
		{
			get { return form.DataSource; }
			set
			{
				form.DataSource = value;
			}
		}
		
		public FormOrder Form
		{
			get { return form; }
		}
	}
}
