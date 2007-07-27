// /home/xavier/Projects/SuPOS/SuPOSAdmin/Main.cs created with MonoDevelop
// User: xavier at 22:22 24/07/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
// project created on 24/07/2007 at 22:22
using System;
using Gtk;
using Glade;

using SuPOSAdmin;

public class GladeApp
{
	public static void Main (string[] args)
	{
		new GladeApp (args);
	}

	public GladeApp (string[] args) 
	{
		Application.Init ();
		new MainWindow();
		Application.Run ();
	}

	
}

