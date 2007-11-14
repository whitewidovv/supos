// /home/xavier/Projects/SuPOS/SuPOS/MainWindow.cs
// User: xavier at 22:43 3/11/2007
//

using System;
using Mono.Unix;
using Gdk;
using Gtk;
using Glade;

namespace Supos
{
	
	
	public class MainWindow
	{
		
		public MainWindow()
		{
			Catalog.Init("supos","./locale");
			Glade.XML gxml = new Glade.XML (null, "supos.glade", "mainwindow", "supos");
			gxml.Autoconnect (this);
		}
		
		//*****************************************
		// CALLBACKS
		//*****************************************
		#pragma warning disable 0169
		private void OnWindowDeleteEvent (object sender, DeleteEventArgs a)
		{
			//m_Disconnect();
			Application.Quit ();
			a.RetVal = true;
		}
	}
}
