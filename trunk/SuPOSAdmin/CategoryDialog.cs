// /home/xavier/Projects/SuPOS/SuPOSAdmin/CategoryDialog.cs created with MonoDevelop
// User: xavier at 15:29 26/07/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Mono.Unix;
using Gtk;
using Glade;
using Gdk;

using LibSupos;

namespace SuPOSAdmin
{
	public class CategoryDialog
	{
		private Gtk.Image iconimage = new Gtk.Image();
		private Pixbuf iconpixbuf = null;
		public SuposDbCategory Category = null;
		
		[Widget] public Gtk.Dialog categorydialog;
		[Widget] public Gtk.Entry identry;
		[Widget] public Gtk.Entry nameentry;
		[Widget] private Gtk.Button iconbutton;
		
		
		public CategoryDialog()
		{
			Catalog.Init("suposadmin","./locale");
			Glade.XML gxml = new Glade.XML (null, "suposadmin.glade", "categorydialog", "suposadmin");
			gxml.Autoconnect (this);
			Category  = new SuposDbCategory();
		}
		
		public CategoryDialog(SuposDbCategory category)
		{
			Catalog.Init("suposadmin","./locale");
			Glade.XML gxml = new Glade.XML (null, "suposadmin.glade", "categorydialog", "suposadmin");
			gxml.Autoconnect (this);
			if ( category != null)
			{
				Category = category;
				identry.Text = Category.Id.ToString();
				nameentry.Text = Category.Name;
				iconimage.Pixbuf = Category.Icon.GetPixbuf();
				if ( iconimage.Pixbuf != null)
					iconimage.Pixbuf = iconimage.Pixbuf.ScaleSimple(50, 50, InterpType.Bilinear );
				iconbutton.Add(iconimage);
				iconimage.Show();
				
			}
			else
			{
				Category = new SuposDbCategory();
			}
		}
		
		//*********************************
		// CALLBACKS
		//*********************************
		#pragma warning disable 0169
		private void OnIconButtonClicked ( object sender, EventArgs a )
		{
			FileChooserDialog dlg = new FileChooserDialog(Catalog.GetString("Choose an icon "),
			                                              categorydialog,
			                                              FileChooserAction.Open,
			                                              Stock.Cancel, ResponseType.Cancel,
			                                              Stock.Clear, ResponseType.No,
			                                              Stock.Ok, ResponseType.Ok );
			dlg.SelectMultiple = false;
			dlg.UsePreviewLabel = false;
			FileFilter filter = new FileFilter();
			filter.Name = "Images";
			filter.AddPixbufFormats();
			dlg.AddFilter(filter);
			int result = dlg.Run();
			if ( (ResponseType)result == ResponseType.Ok )
			{
				iconpixbuf = new Pixbuf ( dlg.Filename, 50, 50 );
				iconimage.Pixbuf = iconpixbuf;
				iconbutton.Add(iconimage);
				iconimage.Show();
				if ( Category != null )
				{
					Category.Icon.Set( dlg.Filename );
				}
			}
			if ( (ResponseType)result == ResponseType.No )
			{
				if ( iconimage != null)
				{
					iconbutton.Remove ( iconimage );
				}
				iconimage = null;
				
				if ( Category != null)
				{
					Category.Icon.Clear();
				}
			}
			dlg.Destroy();
		}
		
		private void OnOkClicked ( object sender, EventArgs a )
		{
			if ( Category != null )
			{
				Category.Name = nameentry.Text;
			}
			
		}
	}
}
