// CategoriesView.cs created with MonoDevelop
// User: xavier at 01:46 4/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using Gtk;

namespace Supos
{
	
	
	public class ViewBase : Gtk.VBox
	{
		private StoreBase store;
		private bool updownvisible = false;
		
		protected Gtk.Button upbutton;
		protected Gtk.Button downbutton;
		protected Gtk.ScrolledWindow swindow;
		protected Gtk.TreeView treeview;
		
		public ViewBase() : base()
		{	
			upbutton = new Button(Gtk.Stock.GoUp);
			upbutton.Clicked += OnUpClicked;
			downbutton = new Button(Gtk.Stock.GoDown);
			downbutton.Clicked += OnDownClicked;
			swindow = new ScrolledWindow();
			treeview = new TreeView();
			treeview.Selection.Changed += OnSelectionChanged;
			treeview.RowActivated += OnRowActivated;
			swindow.Add(treeview);
			this.PackStart(upbutton, false, false, 0);
			this.PackStart(swindow, true, true, 0);
			this.PackStart(downbutton, false, false, 0);
			
			store = new StoreBase();
			treeview.Model=store.ViewModel;
			
			ShowAll();
		}
		
		public string FilterColumn
		{
			get{ return store.FilterColumn;}
			set { store.FilterColumn = value;}
		}
		
		public string FilterValue
		{
			get{ return store.FilterValue;}
			set { store.FilterValue = value;}
		}
		
		public SuposDb DataSource
		{
			get { return store.DataSource; }
			set { store.DataSource = value; }
		}
			
		public string DataMember
		{
			get { return store.DataMember; }
			set { store.DataMember = value; }
		}
		
		public bool UpDownVisible
		{
			get { return updownvisible; }
			set
			{
				upbutton.Visible = value;
				downbutton.Visible = value;
				updownvisible = value;
			}
		}
		
		public bool HeaderVisible
		{
			get { return treeview.HeadersVisible; }
			set { treeview.HeadersVisible = value; }
		}

		public DataRow GetSelectedRow()
		{
			TreeIter iter = TreeIter.Zero;
			TreePath[] paths = treeview.Selection.GetSelectedRows();
			if( paths.Length >0 )
			{
				store.ViewModel.GetIter( out iter, paths[0]);
				DataRow row = (DataRow)store.ViewModel.GetValue( iter, 0 );
				return row;
			}
			else
				return null;
		}
		
		
		public void Reload()
		{
			DataRow selected = this.GetSelectedRow();
			this.store.Reload();
			this.Select(selected);
		}
		
		public void Select ( DataRow row)
		{
			if( row != null && row.RowState != DataRowState.Detached)
			{
				SelectFromId( row["Id"].ToString() );
			}
		}
		
		public void SelectFirst()
		{
			TreeIter iter = new TreeIter();
			iter = TreeIter.Zero;
			store.ViewModel.GetIterFirst(out iter);
			treeview.Selection.SelectIter(iter);
		}
			
		public void SelectFromId( string strid )
		{
			TreeIter iter = new TreeIter();
			iter = TreeIter.Zero;
			this.store.ViewModel.GetIterFirst( out iter);
			do
			{
				DataRow tmprow = (DataRow) store.ViewModel.GetValue(iter,0);
				if( tmprow != null )
				{
					if( strid == tmprow["Id"].ToString() )
					{	
						treeview.Selection.SelectIter(iter);
						
						TreePath path = store.ViewModel.GetPath(iter);
						treeview.ScrollToCell( path, null, false, 0, 0);
						return;
					}
				}
				else if ( strid == "" )
				{
					treeview.Selection.SelectIter(iter);
					return;
				}	
			} while( store.ViewModel.IterNext( ref iter) );
		}
		
		public event System.EventHandler SelectionChanged;
		
		protected virtual void OnSelectionChanged (object o, EventArgs args)
		{
			if ( SelectionChanged != null )
			{
				SelectionChanged(this, args);
			}
		}
		
		public event System.EventHandler RowActivated;
		protected virtual void OnRowActivated (object o, Gtk.RowActivatedArgs args)
		{
			if ( RowActivated != null )
			{
				RowActivated(this, new System.EventArgs() );
			}
		}
		
		protected virtual void OnUpClicked (object sender, System.EventArgs e)
		{
			swindow.Vadjustment.Value -= swindow.Vadjustment.StepIncrement;
		}

		protected virtual void OnDownClicked (object sender, System.EventArgs e)
		{
			if (swindow.Vadjustment.Value < swindow.Vadjustment.Upper-swindow.Vadjustment.PageSize)
				swindow.Vadjustment.Value += swindow.Vadjustment.StepIncrement;
		}
		
	
		
	}
}
