// CellRendererNameIcon.cs created with MonoDevelop
// User: xavier at 08:53 6/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;
using Medsphere.Widgets;

namespace Supos
{
	
	
	public class CellRendererNameIcon : BoxCellRenderer
	{
		
		
		public CellRendererNameIcon() : base()
		{
			PackStart (new PixbufCellRenderer ());
			PackStart (new TextCellRenderer ());
		}
	}
}
