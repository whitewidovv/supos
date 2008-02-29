// 

using System;
using Gdk;
using Gtk;
using Libsupos;

namespace suposadmin
{
	
	
	public partial class ProductDialog : Gtk.Dialog
	{
		private Gtk.Image m_IconImage = new Gtk.Image();
		private bool m_IconChanged = false;
		private string m_IconFileName = null;
		private SuposProduct m_Product = null;
		private SuposDb m_DataBase = null;
		private ListStore categoriesstore = new ListStore(typeof(string), typeof(int));
		private ListStore taxesstore = new ListStore(typeof(string), typeof(string), typeof(int));
		
		enum ComboColumnCategory{ Name, Id };
		enum ComboColumnTax{ Name, Rate, Id };
		
		public ProductDialog(SuposDb database)
		{			
			this.Build();
			m_DataBase = database;
			this.FillCombos();
			this.SelectCombos();
			m_Product = new SuposProduct();
			iconbutton.Add(m_IconImage);
			m_IconImage.Show();
		}
		
		public ProductDialog(SuposDb database, SuposProduct product)
		{
			this.Build();
			m_DataBase = database;
			this.FillCombos();
			if ( product != null )
			{
				m_Product = product;
				this.SelectCombos();
				identry.Text = m_Product.Id.ToString();
				nameentry.Text = m_Product.Name;
				pricespinbutton.Value = product.Price;
				// priceti updated automaticaly due to callback
				m_IconImage.Pixbuf = m_Product.Icon.GetPixbuf();
				if ( m_IconImage.Pixbuf != null)
					m_IconImage.Pixbuf = m_IconImage.Pixbuf.ScaleSimple(50, 50, InterpType.Bilinear );	
			}
			else
			{
				m_Product = new SuposProduct();
			}
			iconbutton.Add(m_IconImage);
			m_IconImage.Show();
		}
		
		private void FillCombos()
		{
			// fill  category combo box
			CellRendererText categorycell = new CellRendererText();
			categorycombobox.PackStart( categorycell, false);
			categorycombobox.AddAttribute(categorycell, "text", (int)ComboColumnCategory.Name);
			categorycombobox.Model = categoriesstore;
			foreach( SuposCategory category in m_DataBase.Categories )
			{
				categoriesstore.AppendValues(category.Name, category.Id);
			}
			// fill  tax combo box
			CellRendererText taxcell = new CellRendererText();
			taxcombobox.PackStart( taxcell, false);
			taxcombobox.AddAttribute(taxcell, "text", (int)ComboColumnTax.Name);
			CellRendererText taxratecell = new CellRendererText();
			taxcombobox.PackEnd( taxratecell, false);
			taxcombobox.AddAttribute(taxratecell, "text", (int)ComboColumnTax.Rate);
			taxcombobox.Model = taxesstore;
			
			foreach( SuposTax tax in m_DataBase.Taxes )
			{
				taxesstore.AppendValues(tax.Name, tax.Rate.ToString(), tax.Id);
			}
		}
		
		private void SelectCombos()
		{
			if( m_Product != null)
			{
				//select first
				TreeIter iter = new TreeIter();
				if( categoriesstore.GetIterFirst(out iter) )
				{
					do
					{
						if( m_Product.CategoryId == (int)categoriesstore.GetValue(iter, (int)ComboColumnCategory.Id) )
							break;
					}
					while( categoriesstore.IterNext(ref iter) );
				}
				categorycombobox.SetActiveIter(iter);
				if( taxesstore.GetIterFirst(out iter) )
				{
					do
					{
						if( m_Product.TaxId == (int)taxesstore.GetValue(iter, (int)ComboColumnTax.Id) )
							break;
					}
					while( taxesstore.IterNext(ref iter) );
				}
				taxcombobox.SetActiveIter(iter);
			}
			else
			{
				//select first
				TreeIter iter = new TreeIter();
				categoriesstore.GetIterFirst(out iter);
				categorycombobox.SetActiveIter(iter);
				taxesstore.GetIterFirst(out iter);
				taxcombobox.SetActiveIter(iter);
			}
		}
		
		
		private int GetSelectedTaxID()
		{
			TreeIter iter = new TreeIter();
			if( taxcombobox.GetActiveIter(out iter) )
			{
				return (int) taxesstore.GetValue(iter, (int)ComboColumnTax.Id);
			}
			return -1;
		}
		
		private int GetSelectedCategoryID()
		{
			TreeIter iter = new TreeIter();
			if( categorycombobox.GetActiveIter(out iter) )
			{
				return (int) categoriesstore.GetValue(iter, (int)ComboColumnCategory.Id);
			}
			return -1;
		}
		
		public SuposProduct Product
		{
			get
			{
				return m_Product;
			}
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
			if ( m_Product != null )
			{
				m_Product.Name = nameentry.Text;
				m_Product.Price = (float) pricespinbutton.Value;
				m_Product.CategoryId = GetSelectedCategoryID();
				m_Product.TaxId = GetSelectedTaxID();
				if(m_IconChanged)
				{
					if (m_IconFileName != null)
						m_Product.Icon.Set(m_IconFileName);
					else
						m_Product.Icon.Clear();
				}
			}
		}
		
		private void OnTaxChanged ( object sender, EventArgs a )
		{
			OnPriceChanged(sender, a);
		}
		
		private void OnPriceChanged ( object sender, EventArgs a )
		{
			int id = GetSelectedTaxID();
			if ( id >=0 )
			{
				pricetispinbutton.Value = pricespinbutton.Value *(1 + m_DataBase.TaxFromId(id).Rate/100);
			}
		}
		
		private void OnPricetiChanged ( object sender, EventArgs a )
		{
			int id = GetSelectedTaxID();
			if ( id >=0 )
			{
				pricespinbutton.Value = pricetispinbutton.Value /(1 + m_DataBase.TaxFromId(id).Rate/100);
			}
		}
		
	}
}
