// ComboBoxCustomers.cs created with MonoDevelop
// User: xavier at 10:03 6/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace Supos
{
	
	
	public class ComboBoxCustomers : ComboBoxBase
	{
		
		public ComboBoxCustomers() : base()
		{
			CellRendererText cell = new CellRendererText();
			this.PackStart(cell, true);
			this.SetCellDataFunc(cell, CellRenderFunctions.RenderCustomer );
			DataMember="Customers";
		}
	}
}
