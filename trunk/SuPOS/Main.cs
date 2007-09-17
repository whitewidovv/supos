// /home/xavier/Projects/SuPOS/SuPOS/Main.cs created with MonoDevelop
// User: xavier at 22:18 24/07/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
// project created on 24/07/2007 at 22:18
using System;
using Gtk;
using Glade;

public class GladeApp
{
	public static void Main (string[] args)
	{
		new GladeApp (args);
	}

	public GladeApp (string[] args) 
	{
		Application.Init ();

		Glade.XML gxml = new Glade.XML (null, "supos.glade", "mainwindow", null);
		gxml.Autoconnect (this);
		Application.Run ();
	}

	// Connect the Signals defined in Glade
	private void OnWindowDeleteEvent (object sender, DeleteEventArgs a) 
	{
		Application.Quit ();
		a.RetVal = true;
	}
}

