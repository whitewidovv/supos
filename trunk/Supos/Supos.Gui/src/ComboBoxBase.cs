// RowComboBox.cs created with MonoDevelop
// User: xavier at 00:27 5/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using Gtk;
using Supos.Core;

namespace Supos.Gui
{

	public class ComboBoxBase : Gtk.ComboBox
	{
		protected StoreBase store;
	
		public ComboBoxBase() : base()
		{
			store = new StoreBase();
			this.Model = store.ViewModel;
			
			this.ShowAll();
		}
		
		public SuposDb DataSource
		{
			get { return store.DataSource; }
			set { store.DataSource = value; }
		}
			
		protected string DataMember
		{
			get { return store.DataMember; }
			set { store.DataMember = value; }
		}
		
		public bool NoneRow
		{
			get { return store.NoneRow; }
			set { store.NoneRow = value; }				
		}
		
		public DataRow GetActiveRow()
		{
			TreeIter iter = TreeIter.Zero;
			if( GetActiveIter(out iter) )
			{
				DataRow row = (DataRow)store.ViewModel.GetValue( iter, 0 );
				return row;
			}
			else
				return null;
		}
		
		public object GetActiveId()
		{
			DataRow row = GetActiveRow();
			if( row != null)
			{
				return row["Id"];
			}
			else 
				return DBNull.Value;
		}
		
		public void Reload()
		{
			DataRow selected = this.GetActiveRow();
			this.store.Reload();
			this.Select(selected);
		}
		
		public void SelectFirst()
		{
			TreeIter iter = TreeIter.Zero;
			this.store.ViewModel.GetIterFirst(out iter);
			this.SetActiveIter(iter);
		}
		
		public void Select ( DataRow row)
		{
			if( row != null && row.RowState != DataRowState.Deleted && row.RowState!= DataRowState.Detached)
				SelectFromId( row["Id"].ToString() );
		}
		
		public void SelectFromId( string strid )
		{
			TreeIter iter = TreeIter.Zero;
			this.store.ViewModel.GetIterFirst( out iter);
			do
			{
				DataRow tmprow = (DataRow) store.ViewModel.GetValue(iter,0);
				if( tmprow != null )
				{
					if( strid == tmprow["Id"].ToString() )
					{
						this.SetActiveIter(iter);
						return;
					}
				}
				else if ( strid == "" )
				{
					this.SetActiveIter(iter);
					return;
				}	
			} while( store.ViewModel.IterNext( ref iter) );
		}
	}
}
