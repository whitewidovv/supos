// 

using System;
using System.Collections;
using Gdk;
using Gtk;
using Libsupos;

namespace suposadmin
{
	
	
	public partial class ProductsViewWidget : MainViewWidget
	{
		private SuposDb m_DataBase = null;
		private ListStore m_Store = null;
		
		enum ProductColumn {Id, Icon, Name, Category, Tax, Price, PriceTaxInc, Data};
		
		public ProductsViewWidget(SuposDb Db)
		{
			this.Build();
			m_DataBase = Db;
			productstreeview.Selection.Mode = Gtk.SelectionMode.Multiple;
			m_Store = new ListStore ( typeof(string), typeof(Gdk.Pixbuf), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(SuposProduct) );
			productstreeview.Model = m_Store;
			Gtk.TreeViewColumn ProductIdColumn = new Gtk.TreeViewColumn ();
			Gtk.TreeViewColumn ProductIconColumn = new Gtk.TreeViewColumn ();
			Gtk.TreeViewColumn ProductNameColumn = new Gtk.TreeViewColumn ();
			Gtk.TreeViewColumn ProductCategoryColumn = new Gtk.TreeViewColumn ();
			Gtk.TreeViewColumn ProductTaxColumn = new Gtk.TreeViewColumn ();
			Gtk.TreeViewColumn ProductPriceColumn = new Gtk.TreeViewColumn ();
			Gtk.TreeViewColumn ProductPriceTaxIncColumn = new Gtk.TreeViewColumn ();
			ProductIdColumn.Title = "ID";
			ProductIconColumn.Title = "Icon";
			ProductNameColumn.Title = "Name";
			ProductCategoryColumn.Title = "Category";
			ProductTaxColumn.Title = "Default Tax";
			ProductPriceColumn.Title = "Sell Price ";
			ProductPriceTaxIncColumn.Title = "Sell Price TI";
			productstreeview.AppendColumn (ProductIdColumn);
			productstreeview.AppendColumn (ProductIconColumn);
			productstreeview.AppendColumn (ProductNameColumn);
			productstreeview.AppendColumn (ProductCategoryColumn);
			productstreeview.AppendColumn (ProductTaxColumn);
			productstreeview.AppendColumn (ProductPriceColumn);
			productstreeview.AppendColumn (ProductPriceTaxIncColumn);
			CellRendererText ProductIdCell = new Gtk.CellRendererText ();
			CellRendererPixbuf ProductIconCell = new Gtk.CellRendererPixbuf ();
			CellRendererText ProductNameCell = new Gtk.CellRendererText ();
			CellRendererText ProductCategoryCell = new Gtk.CellRendererText ();
			CellRendererText ProductTaxCell = new Gtk.CellRendererText ();
			CellRendererText ProductPriceCell = new Gtk.CellRendererText ();
			CellRendererText ProductPriceTaxIncCell = new Gtk.CellRendererText ();
			ProductIdColumn.PackStart(ProductIdCell, true);
			ProductIconColumn.PackStart(ProductIconCell, true);
			ProductNameColumn.PackStart(ProductNameCell, true);
			ProductCategoryColumn.PackStart(ProductCategoryCell, true);
			ProductTaxColumn.PackStart(ProductTaxCell, true);
			ProductPriceColumn.PackStart(ProductPriceCell, true);
			ProductPriceTaxIncColumn.PackStart(ProductPriceTaxIncCell, true);
			ProductIdColumn.AddAttribute (ProductIdCell, "text", (int)ProductColumn.Id );
			ProductIconColumn.AddAttribute (ProductIconCell, "pixbuf", (int)ProductColumn.Icon );
			ProductNameColumn.AddAttribute (ProductNameCell, "text", (int)ProductColumn.Name );
			ProductCategoryColumn.AddAttribute (ProductCategoryCell, "text", (int)ProductColumn.Category );
			ProductTaxColumn.AddAttribute (ProductTaxCell, "text", (int)ProductColumn.Tax );
			ProductPriceColumn.AddAttribute (ProductPriceCell, "text", (int)ProductColumn.Price );
			ProductPriceTaxIncColumn.AddAttribute (ProductPriceTaxIncCell, "text", (int)ProductColumn.PriceTaxInc );
			ProductIdColumn.SortColumnId = (int)ProductColumn.Id;
			ProductNameColumn.SortColumnId = (int)ProductColumn.Name;
			ProductCategoryColumn.SortColumnId = (int)ProductColumn.Category;
			ProductTaxColumn.SortColumnId = (int)ProductColumn.Tax;
			ProductPriceColumn.SortColumnId = (int)ProductColumn.Price;
			ProductPriceTaxIncColumn.SortColumnId = (int)ProductColumn.PriceTaxInc;
			m_CreateProductView();
		}
		
		private void m_CreateProductView()
		{
			if ( m_DataBase != null)
			{	
				ArrayList products = m_DataBase.Products;
				if ( products != null )
				{
					foreach (SuposProduct product in products )
					{
						Pixbuf pb = product.Icon.GetPixbuf();
						if ( pb != null )
								pb = pb.ScaleSimple( 50, 50, Gdk.InterpType.Bilinear );
						if( product.Category != null && product.Tax != null )
						{
							m_Store.AppendValues(product.Id.ToString(), pb, product.Name, product.Category.Name, product.Tax.Name, product.Price.ToString(), product.PriceTaxInc.ToString(), product);
						}
						else
						{
							Console.WriteLine("Impossible to load product because of wrong category and/or tax");
						}
					}
				}
			}
		}
		
		private void m_ClearView()
		{
			if ( m_Store != null)
			{
				m_Store.Clear();
			}
		}

		public override void AddNew ()
		{
			ProductDialog dlg = new ProductDialog(m_DataBase);
			int result = dlg.Run();
			if ( (ResponseType)result == ResponseType.Ok)
			{
				TreeIter iter;
				if( m_DataBase.AddProduct( dlg.Product ) )
				{
					// Update view
					Pixbuf pb = dlg.Product.Icon.GetPixbuf();
					if( pb != null )
							pb = pb.ScaleSimple( 50, 50, Gdk.InterpType.Bilinear );
					if( dlg.Product.Category != null && dlg.Product.Tax != null )
					{
						iter = m_Store.AppendValues(dlg.Product.Id.ToString(), pb, dlg.Product.Name, dlg.Product.Category.Name, dlg.Product.Tax.Name, dlg.Product.Price.ToString(), dlg.Product.PriceTaxInc.ToString(), dlg.Product);
						// Select new inserted row
						productstreeview.Selection.SelectIter( iter );
					}
					else
					{
							Console.WriteLine("Impossible to add product because of wrong category and/or tax");
					}
				}
			}
			dlg.Destroy();
		}

		public override void ModifyProperties ()
		{
			TreeIter iter;
			TreeModel model;
			TreePath[] path_array = productstreeview.Selection.GetSelectedRows(out model);
			
			if ( path_array.Length>0  )
			{		
				model.GetIter(out iter, path_array[0]);
			    SuposProduct product = (SuposProduct) model.GetValue (iter, (int)ProductColumn.Data );
			    ProductDialog dlg = new ProductDialog( m_DataBase, product );
				int result = dlg.Run();
				if ( (ResponseType)result == ResponseType.Ok)
				{
					if( product.ApplyChange() )
					{
						// Update of the row
						model.SetValue(iter, (int)ProductColumn.Id, product.Id.ToString() );
						model.SetValue(iter, (int)ProductColumn.Name, product.Name);
						model.SetValue(iter, (int)ProductColumn.Price, product.Price.ToString() );
						model.SetValue(iter, (int)ProductColumn.PriceTaxInc, product.PriceTaxInc.ToString() );
						Pixbuf pb = product.Icon.GetPixbuf();
						if ( pb != null )
						{
							pb = pb.ScaleSimple( 50, 50, Gdk.InterpType.Bilinear );
							model.SetValue(iter, (int)ProductColumn.Icon, pb);
						}
						if( dlg.Product.Category != null )
						{
							model.SetValue(iter, (int)ProductColumn.Category, product.Category.Name);
						}
						if( dlg.Product.Tax != null )
						{
							model.SetValue(iter, (int)ProductColumn.Tax, product.Tax.Name);
						}
					}
				}
				dlg.Destroy();
			}
		}

		public override void DeleteSelected ()
		{
			TreeIter iter;
			TreeModel model;
			ArrayList rowlist = new ArrayList();
			TreePath[] path_array = productstreeview.Selection.GetSelectedRows(out model);
			foreach ( TreePath path in path_array )
			{		
				model.GetIter(out iter, path);
				SuposProduct product = (SuposProduct) model.GetValue(iter, (int)ProductColumn.Data );
			    if( m_DataBase.Remove(product) )
				{
					rowlist.Add( new TreeRowReference(model, path) ); //mark row for deletion
				}
			}
			// Delete marked rows
			ListStore store = (ListStore)model;
			foreach ( TreeRowReference row in rowlist)
			{
				store.GetIter(out iter, row.Path);
				store.Remove(ref iter);
			}
		}

		public override void RefreshView ()
		{
			m_ClearView();
			m_CreateProductView();
		}
		
	}
}
