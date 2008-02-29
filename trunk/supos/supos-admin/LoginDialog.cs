// 

using System;

namespace suposadmin
{
	
	
	public partial class LoginDialog : Gtk.Dialog
	{
		
		public LoginDialog()
		{
			this.Build();
		}
		
		public Gtk.Entry UserEntry
		{
			get
			{
				return userentry;
			}
		}
		
		public Gtk.Entry PassEntry
		{
			get
			{
				return passentry;
			}
		}
	}
}
