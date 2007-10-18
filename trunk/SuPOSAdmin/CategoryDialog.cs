// /home/xavier/Projects/SuPOS/SuposAdmin/CategoryDialog.cs created with MonoDevelop
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

namespace SuposAdmin
{
	public class CategoryDialog
	{
		private Gtk.Image m_IconImage = new Gtk.Image();
		private bool m_IconChanged = false;
		private string m_IconFileName = null;
		private SuposCategory m_Category = null;
		
		[Widget] private Gtk.Dialog categorydialog;
		[Widget] private Gtk.Entry identry;
		[Widget] private Gtk.Entry nameentry;
		[Widget] private Gtk.Button iconbutton;
		[Widget] private Gtk.Button okbutton;
		
		
		public CategoryDialog()
		{
			Catalog.Init("suposadmin","./locale");
			Glade.XML gxml = new Glade.XML (null, "suposadmin.glade", "categorydialog", "suposadmin");
			gxml.Autoconnect (this);
			m_Category  = new SuposCategory();
			iconbutton.Add(m_IconImage);
			m_IconImage.Show();
			
		}
		
		public CategoryDialog(SuposCategory category)
		{
			Catalog.Init("suposadmin","./locale");
			Glade.XML gxml = new Glade.XML (null, "suposadmin.glade", "categorydialog", "suposadmin");
			gxml.Autoconnect (this);
			if ( category != null)
			{
				m_Category = category;
				identry.Text = m_Category.Id.ToString();
				nameentry.Text = m_Category.Name;
				m_IconImage.Pixbuf = m_Category.Icon.GetPixbuf();
				if ( m_IconImage.Pixbuf != null)
					m_IconImage.Pixbuf = m_IconImage.Pixbuf.ScaleSimple(50, 50, InterpType.Bilinear );	
			}
			else
			{
				m_Category = new SuposCategory();
			}
			iconbutton.Add(m_IconImage);
			m_IconImage.Show();
			
		}
		
		public int Run()
		{
			return categorydialog.Run();
		}
		
		public void Destroy()
		{
			categorydialog.Destroy();
		}
		
		public SuposCategory Category
		{
			get
			{
				return m_Category;
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
				m_IconImage.Pixbuf = new Pixbuf ( dlg.Filename, 50, 50 );
				m_IconChanged = true;
				m_IconFileName = dlg.Filename;
			}
			if ( (ResponseType)result == ResponseType.No )
			{
				m_IconImage.Pixbuf.Dispose(); 
				m_IconImage.Pixbuf = null;
				m_IconChanged = true;
				m_IconFileName = null;
			}
			dlg.Destroy();
		}
		
		private void OnOkClicked ( object sender, EventArgs a )
		{
			if ( m_Category != null )
			{
				m_Category.Name = nameentry.Text;
				if(m_IconChanged)
				{
					if (m_IconFileName != null)
						m_Category.Icon.Set(m_IconFileName);
					else
						m_Category.Icon.Clear();
				}
			}
			
		}
		
		private void OnActivate ( object sender, EventArgs a )
		{
			okbutton.Click();
		}
	}
}
