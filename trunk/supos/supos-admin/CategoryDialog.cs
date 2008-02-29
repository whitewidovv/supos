// 

using System;
using Gdk;
using Gtk;
using Libsupos;

namespace suposadmin
{
	
	
	public partial class CategoryDialog : Gtk.Dialog
	{
		private Gtk.Image m_IconImage = new Gtk.Image();
		private bool m_IconChanged = false;
		private string m_IconFileName = null;
		private SuposCategory m_Category = null;
		
		public CategoryDialog()
		{
			this.Build();
			m_Category  = new SuposCategory();
			iconbutton.Add(m_IconImage);
			m_IconImage.Show();
		}
		public CategoryDialog(SuposCategory category)
		{
			this.Build();
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

		protected virtual void OnIconClicked (object sender, System.EventArgs e)
		{
			FileChooserDialog dlg = new FileChooserDialog("Choose an icon ",
			                                              this,
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

		protected virtual void OnOk (object sender, System.EventArgs e)
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
		
		public SuposCategory Category
		{
			get
			{
				return m_Category;
			}
		}
	}
}
