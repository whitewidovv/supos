// RowStore.cs created with MonoDevelop
// User: xavier at 23:01 3/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using Gtk;
using Supos.Core;

namespace Supos.Gui
{
	
	
	public class StoreBase
	{
		private ListStore store;
		private TreeModelFilter filter;
		private SuposDb datasource = null;
		private string table = null;
		private string filtercol = "";
		private string filterval = "";
		private bool nonerow = false;
		
		public StoreBase()
		{
			store = new ListStore( typeof (System.Data.DataRow) );
			filter = new TreeModelFilter(store, null);
			filter.VisibleFunc = FilterFunc;
		}
		
		public TreeModel ViewModel
		{
			get { return filter;}
		}
		
		public SuposDb DataSource
		{
			get { return datasource;}
			set
			{
				datasource = value;
				Reload();
			}
		}
		
		public string DataMember
		{
			get { return table;}
			set
			{
				table= value;
				Reload();
			}
		}
		
		public string FilterColumn
		{
			get{ return filtercol; }
			set
			{
				filtercol = value;
				filter.Refilter();
			}				
		}
		
		public bool NoneRow
		{
			get { return nonerow; }
			set { nonerow = value; }				
		}
		
		public string FilterValue
		{
			get{ return filterval; }
			set
			{
				filterval = value;
				filter.Refilter();
			}				
		}
		
		bool FilterFunc (TreeModel model, TreeIter iter)
		{
			if( filtercol.Length < 1)
				return true;
			DataRow row = model.GetValue(iter, 0) as DataRow;
			if( row==null )
				return true;
			string val = row[filtercol].ToString();
			if( val.CompareTo(filterval) == 0 )
				return true;
			else 
				return false; 
		}
		
		public int Reload ()
		{
			int rownum = 0;
			store.Clear();
			
			if (nonerow)
				store.Append();
			
			if (datasource!=null  && datasource.DataSet.Tables.Contains(table) )
			{
				foreach (DataRow row in datasource.DataSet.Tables[table].Rows )
				{
					if(row.RowState != DataRowState.Deleted)
						store.AppendValues(row);
				}
			}
			
			if ( Reloaded != null )
			{
				Reloaded(this, new System.EventArgs() );
			}
			
			return rownum;			
		}
		
		public event System.EventHandler Reloaded;
		
		public DataRow GetRow(TreeIter iter)
		{
			return filter.GetValue(iter, 0) as DataRow;
		}
		
//		public bool GetIterFirst(out TreeIter iter)
//		{
//			this.filter.ge
//			return this.store.GetIterFirst( out iter );
//		}
		
	}
}
