// 

using System;
using System.Collections;
using Gdk;
using Gtk;
using Libsupos;

namespace suposadmin
{
	
	
	public partial class CategoriesViewWidget : MainViewWidget
	{
		private SuposDb m_DataBase = null;
		private ListStore m_Store = null;
		
		enum CategoryColumn {Id, Icon, Name, Data};
		
		public CategoriesViewWidget(SuposDb Db)
		{
			this.Build();
			m_DataBase = Db;
			
			categoriestreeview.Selection.Mode = Gtk.SelectionMode.Multiple;
			
			m_Store = new ListStore ( typeof(string), typeof(Gdk.Pixbuf), typeof(string), typeof(SuposCategory) );
			categoriestreeview.Model = m_Store;
			Gtk.TreeViewColumn CategoryIdColumn = new Gtk.TreeViewColumn ();
			Gtk.TreeViewColumn CategoryIconColumn = new Gtk.TreeViewColumn ();
			Gtk.TreeViewColumn CategoryNameColumn = new Gtk.TreeViewColumn ();
			CategoryIdColumn.Title = "ID";
			CategoryIconColumn.Title = "Icon";
			CategoryNameColumn.Title = "Name";
			categoriestreeview.AppendColumn (CategoryIdColumn);
			categoriestreeview.AppendColumn (CategoryIconColumn);
			categoriestreeview.AppendColumn (CategoryNameColumn);
			CellRendererText CategoryIdCell = new Gtk.CellRendererText ();
			CellRendererPixbuf CategoryIconCell = new Gtk.CellRendererPixbuf ();
			CellRendererText CategoryNameCell = new Gtk.CellRendererText ();
			CategoryIdColumn.PackStart(CategoryIdCell, true);
			CategoryIconColumn.PackStart(CategoryIconCell, true);
			CategoryNameColumn.PackStart(CategoryNameCell, true);
			CategoryIdColumn.AddAttribute (CategoryIdCell, "text", (int)CategoryColumn.Id );
			CategoryIconColumn.AddAttribute (CategoryIconCell, "pixbuf", (int)CategoryColumn.Icon );
			CategoryNameColumn.AddAttribute (CategoryNameCell, "text", (int)CategoryColumn.Name );
			CategoryIdColumn.SortColumnId = (int)CategoryColumn.Id;
			CategoryNameColumn.SortColumnId = (int)CategoryColumn.Name;
			m_CreateCategoryView();
		}
		
		private void m_CreateCategoryView()
		{
			if ( m_DataBase != null)
			{	
				ArrayList categories = m_DataBase.Categories;
				if ( categories != null )
				{
					foreach (SuposCategory category in categories )
					{
						Pixbuf pb = category.Icon.GetPixbuf();
						if ( pb != null )
								pb = pb.ScaleSimple( 50, 50, Gdk.InterpType.Bilinear );
						m_Store.AppendValues(category.Id.ToString(), pb, category.Name, category);
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
			CategoryDialog dlg = new CategoryDialog();
			int result = dlg.Run();
			if ( (ResponseType)result == ResponseType.Ok)
			{
				TreeIter iter;
				if( m_DataBase.AddCategory(dlg.Category) )
				{
					// Update view
					Pixbuf pb = dlg.Category.Icon.GetPixbuf();
					if ( pb != null )
							pb = pb.ScaleSimple( 50, 50, Gdk.InterpType.Bilinear );
					iter = m_Store.AppendValues(dlg.Category.Id.ToString(), pb, dlg.Category.Name, dlg.Category);
					// Select new inserted row
					categoriestreeview.Selection.SelectIter( iter );
				}
			}
			dlg.Destroy();
		}

		public override void ModifyProperties ()
		{
			TreeIter iter;
			TreeModel model;
			TreePath[] path_array = categoriestreeview.Selection.GetSelectedRows(out model);
			
			if ( path_array.Length>0  )
			{		
				model.GetIter(out iter, path_array[0]);
			    SuposCategory cat = (SuposCategory) model.GetValue (iter, (int)CategoryColumn.Data );
			    CategoryDialog dlg = new CategoryDialog( cat );
				int result = dlg.Run();
				if ( (ResponseType)result == ResponseType.Ok)
				{
					if( cat.ApplyChange() )
					{
						// Update of the row
						model.SetValue(iter, (int)CategoryColumn.Id, cat.Id.ToString() );
						model.SetValue(iter, (int)CategoryColumn.Name, cat.Name);
						Pixbuf pb = cat.Icon.GetPixbuf();
						if ( pb != null )
						{
							pb = pb.ScaleSimple( 50, 50, Gdk.InterpType.Bilinear );
							model.SetValue(iter, (int)CategoryColumn.Icon, pb);
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
			TreePath[] path_array = categoriestreeview.Selection.GetSelectedRows(out model);
			foreach ( TreePath path in path_array )
			{		
				model.GetIter(out iter, path);
				SuposCategory cat = (SuposCategory) model.GetValue(iter, (int)CategoryColumn.Data );
			    if ( m_DataBase.Remove(cat) )
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
			m_CreateCategoryView();
		}
	}
}
	
