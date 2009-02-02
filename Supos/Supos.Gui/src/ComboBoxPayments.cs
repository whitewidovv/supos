// ComboBoxPayments.cs created with MonoDevelop
// User: xavier at 10:21 6/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;
using Supos.Core;

namespace Supos.Gui
{
	
	
	public class ComboBoxPayments : ComboBoxBase
	{
		
		public ComboBoxPayments() : base()
		{
			CellRendererText cell = new CellRendererText();
			this.PackStart(cell, true);
			this.SetCellDataFunc(cell, CellRenderFunctions.RenderPayment );
			DataMember = "Payments";
		}
	}
}
