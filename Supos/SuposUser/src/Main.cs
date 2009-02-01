// Main.cs created with MonoDevelop
// User: xavier at 18:23 19/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using Gtk;


namespace Supos
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			
			Gtk.Rc.ParseString("style \"touch-style\"{font_name = \"Sans 12\"} widget \"*.toucharea.*\" style \"touch-style\"");
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}