// /home/xavier/Projects/SuPOS/SuPOSAdmin/CategoriesWindows.cs
// User: xavier at 12:20 11/10/2007
//

using System;
using System.Collections;
using Mono.Unix;
using Gdk;
using Gtk;
using Glade;
using LibSupos;

namespace SuPOSAdmin
{
	
	
	public class CategoriesWindow
	{
		private SuposDb m_DataBase = null;
		private ListStore m_Store = null;
		[Widget] protected Gtk.Window categorieswindow;
		[Widget] protected Gtk.TreeView categoriestreeview;
		
		enum CategoryColumn {Id, Icon, Name, Data};
		
		
		//*****************************************
		// Constructor
		//*****************************************
		public CategoriesWindow(SuposDb Db)
		{
			Catalog.Init("suposadmin","./locale");
			Glade.XML gxml = new Glade.XML (null, "suposadmin.glade", "categorieswindow", "suposadmin");
			gxml.Autoconnect (this);
			
			m_DataBase = Db;
			
			categoriestreeview.Selection.Mode = Gtk.SelectionMode.Multiple;
			
			m_Store = new ListStore ( typeof(string), typeof(Gdk.Pixbuf), typeof(string), typeof(SuposDbCategory) );
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
		
		//****************************************
		// Show window
		//****************************************
		public void Show()
		{
			categorieswindow.Show();
		}
		
		//******************************************
		// Clear TreeView
		//******************************************
		private void m_ClearView()
		{
			if ( m_Store != null)
			{
				m_Store.Clear();
			}
		}
		
		//******************************************
		// Categories in MainTreeView
		//******************************************
		private void m_CreateCategoryView()
		{
			if ( m_DataBase != null)
			{	
				ArrayList categories = m_DataBase.GetCategories();
				if ( categories != null )
				{
					foreach (SuposDbCategory category in categories )
					{
						Pixbuf pb = category.Icon.GetPixbuf();
						if ( pb != null )
								pb = pb.ScaleSimple( 50, 50, Gdk.InterpType.Bilinear );
						m_Store.AppendValues(category.Id.ToString(), pb, category.Name, category);
					}
				}
			}
		}
		
		//********************************************
		// CALLBACKS
		//********************************************
		#pragma warning disable 0169
		private void OnAddClicked (object sender, EventArgs a)
		{
			CategoryDialog dlg = new CategoryDialog();
			int result = dlg.categorydialog.Run();
			if ( result == 1)
			{
				m_DataBase.AddCategory( dlg.Category );
				//update view
				m_ClearView();
				m_CreateCategoryView();
				
			}
			dlg.categorydialog.Destroy();
			
		}
		
		
		private void OnModifyClicked (object sender, EventArgs a)
		{
			TreeIter iter;
			TreeModel model;
			TreePath[] path_array = categoriestreeview.Selection.GetSelectedRows(out model);
			
			if ( path_array.Length>0  )
			{		
				model.GetIter(out iter, path_array[0]);
			    SuposDbCategory cat = (SuposDbCategory) model.GetValue (iter, (int)CategoryColumn.Data );
			    CategoryDialog dlg = new CategoryDialog( cat );
				int result = dlg.categorydialog.Run();
				if ( result == 1)
				{
					cat.ApplyChange();
					//TODO clean update of the view
					m_ClearView();
					m_CreateCategoryView();
				}
				dlg.categorydialog.Destroy();
			}
		}

	
		private void OnDeleteClicked (object sender, EventArgs a)
		{
			TreeIter iter;
			TreeModel model;
			TreePath[] path_array = categoriestreeview.Selection.GetSelectedRows(out model);
			
			foreach ( TreePath path in path_array )
			{		
				model.GetIter(out iter, path);
				SuposDbCategory cat = (SuposDbCategory) model.GetValue(iter, (int)CategoryColumn.Data );
				//Console.WriteLine(cat.Name);
			    m_DataBase.Remove(cat);
			}
			//update view
			m_ClearView();
			m_CreateCategoryView();
		}		
		
	} // public class CategoriesWindows
		
} // namespace SuPOSAdmin
