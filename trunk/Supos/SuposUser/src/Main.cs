// Main.cs created with MonoDevelop
// User: xavier at 18:23 19/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using Gtk;


namespace SuposUser
{
	class MainClass
	{
		public static void Main (string[] args)
		{		
			Application.Init ();
			MainWindow win = new MainWindow ();	
			
			win.ShowAll ();
			Application.Run ();
		}
		
		
	}
}