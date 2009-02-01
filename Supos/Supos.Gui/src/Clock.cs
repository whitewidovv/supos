// Clock.cs created with MonoDevelop
// User: xavier at 02:36 6/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using GLib;
using Gtk;

namespace Supos
{
	
	
	public class Clock : VBox
	{
		private Label datelabel;
		private Label timelabel;
		
		public Clock() : base()
		{
			datelabel = new Label();
			datelabel.UseMarkup = true;
			timelabel = new Label();
			timelabel.UseMarkup = true;
			GLib.Timeout.Add(1000, new TimeoutHandler(update_time) );
			
			this.PackStart(datelabel);
			this.PackStart(timelabel);
		}
		
		private bool update_time()
		{
			DateTime time = DateTime.Now;
			datelabel.Markup = String.Format( "<b>{0}</b>",time.ToString("dd/MM/yyyy") );
			timelabel.Text = String.Format( "{0}",time.ToString("HH:mm:ss") );
			return true;
		}
		
		
	}
}
