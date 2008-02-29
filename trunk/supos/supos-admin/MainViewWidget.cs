// 

using System;
using Gtk;

namespace suposadmin
{
	
	
	public class MainViewWidget : Gtk.Bin
	{
		
		public MainViewWidget()
		{
		}
		
		public virtual void AddNew (){}
		public virtual void ModifyProperties (){}
		public virtual void DeleteSelected (){}
		public virtual void RefreshView (){}
	}
}
