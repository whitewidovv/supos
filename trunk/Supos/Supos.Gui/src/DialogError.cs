// DialogError.cs created with MonoDevelop
// User: xavier at 18:25 4/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace Supos.Gui
{
	
	
	public class DialogError : Gtk.Dialog
	{
		public DialogError(string message, Exception e, Window parent) : base("Error", parent, DialogFlags.Modal, Stock.Ok, ResponseType.Ok)
		{
			HBox hbox = new HBox();
			Image icon = new Image(Stock.DialogError,IconSize.Dialog);
			Label label = new Label(message);
			Expander exp = new Expander("Details");
			ScrolledWindow sw = new ScrolledWindow();
			TextView tview = new TextView();
			
			hbox.BorderWidth = 6;
			hbox.Spacing = 6;
			label.SetAlignment(0f, 0.5f);
			exp.BorderWidth = 6;
			tview.Buffer.Text = e.Message;
			tview.Buffer.Text += "\n";
			tview.Buffer.Text += e.StackTrace;
			
			sw.Add(tview);
			exp.Add(sw);
			hbox.PackStart(icon, false, false, 0);
			hbox.PackStart(label, true, true, 0);
			this.VBox.PackStart(hbox, false, false, 0);
			this.VBox.PackStart(exp, true, true, 0);
			this.ShowAll();
			
		}
	}
}
