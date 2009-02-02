// OrdersCombobox.cs created with MonoDevelop
// User: xavier at 22:04 5/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using Gtk;
using Supos.Core;

namespace Supos.Gui
{
	
	
	public class ComboBoxOrders : ComboBoxBase
	{
		
		public ComboBoxOrders() : base()
		{
			CellRendererText cell = new CellRendererText();
			this.PackStart(cell, true);
			this.SetCellDataFunc(cell, CellRenderFunctions.RenderOrder );
			DataMember = "Orders";
		}
		
		
	}
}
