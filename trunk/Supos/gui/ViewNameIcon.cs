// BaseIconView.cs created with MonoDevelop
// User: xavier at 03:56 6/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using Gtk;
using Medsphere.Widgets;

namespace Supos
{
	
	
	public class ViewNameIcon : Gtk.VBox
	{
		private StoreBase store;
		private bool updownvisible = false;
		protected Gtk.Button upbutton;
		protected Gtk.Button downbutton;
		protected Gtk.ScrolledWindow swindow;
		protected IconLayout layout;
		
		public ViewNameIcon() : base()
		{
			upbutton = new Button(Gtk.Stock.GoUp);
			upbutton.Clicked += OnUpClicked;
			downbutton = new Button(Gtk.Stock.GoDown);
			downbutton.Clicked += OnDownClicked;
			swindow = new ScrolledWindow();
			layout = new IconLayout();
			
			CellRendererNameIcon cell = new CellRendererNameIcon();
			layout.CellRenderer = cell;
			layout.CellDataFunc = CellRenderFunctions.RenderNameIcon;
			layout.LayoutAffinity = LayoutAffinity.Vertical;
			layout.LayoutMode = LayoutMode.Grid;
			layout.SelectionMode = Gtk.SelectionMode.Browse;
			layout.SelectionChanged += OnSelectionChanged;
			layout.ItemActivated += OnRowActivated;
			swindow.Add(layout);
			swindow.HscrollbarPolicy = PolicyType.Never;
			swindow.VscrollbarPolicy = PolicyType.Automatic;
			this.PackStart(upbutton, false, false, 0);
			this.PackStart(swindow, true, true, 0);
			this.PackStart(downbutton, false, false, 0);
			
			store = new StoreBase();
			layout.Model=store.ViewModel;
			
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

		public void SelectFrist()
		{
			TreeIter iter = TreeIter.Zero;
			store.ViewModel.GetIterFirst(out iter);
			layout.SelectPath( store.ViewModel.GetPath(iter) );
		}

		public DataRow SelectedRow
		{
			get 
			{
				TreeIter iter = TreeIter.Zero;
				if( layout.SelectedItems.Length > 0)
				{
					store.ViewModel.GetIter( out iter, layout.SelectedItems[0]);
					return store.GetRow(iter);
				}
				return null;
			}
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
		protected virtual void OnRowActivated (object o, ItemActivatedArgs args)
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
