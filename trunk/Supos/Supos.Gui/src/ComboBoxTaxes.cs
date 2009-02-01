// ComboboxTaxes.cs created with MonoDevelop
// User: xavier at 09:41 6/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace Supos
{
	
	
	public class ComboBoxTaxes : ComboBoxBase
	{
		
		public ComboBoxTaxes() : base()
		{
			CellRendererText cell = new CellRendererText();
			this.PackStart(cell, true);
			this.SetCellDataFunc(cell, CellRenderFunctions.RenderTax );
			DataMember="Taxes";
		}
	}
}
