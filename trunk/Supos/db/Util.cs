// Util.cs created with MonoDevelop
// User: xavier at 02:13 7/11/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using System.Data;
using System.IO;
using System.Text;

namespace Supos
{
	public struct OrderTotal
	{
		public System.Decimal TaxAmount;
		public System.Decimal TotPrice;
		
		public System.Decimal TotPriceTaxInc
		{
			get { return Math.Round(TotPrice + TaxAmount, 2); }
		}
		
	}
	
	public static class Util
	{
		
		static public byte[] GetMedia( string Path, string Media )
		{
			string path = Path + "/" + Media;
			
			if (File.Exists(path) )
			{
				byte[] file = File.ReadAllBytes( path );			 
				return file;
			}
			else return null;
		}

		static public void DumpDataSet(DataSet ds)
		{
			foreach( DataTable tab in ds.Tables)
			{
				Util.DumpTable(tab);
				System.Console.WriteLine();
			}
		}
		
		static public void DumpTable( DataTable table)
		{
			System.Console.WriteLine("TABLE {0}:",table.TableName);
			for(int r = 0; r < table.Rows.Count; r++)
			{
                DataRow row = table.Rows[r];
                DumpRow(row);
            }
		}
		
		static public void DumpRow (DataRow row)
		{
			StringBuilder sb = new StringBuilder();
            for(int c = 0; c < row.ItemArray.Length; c++) {
                    string s = row[c].ToString();
                    sb.Append(s);
                    sb.Append(" ");
            }
            Console.WriteLine(sb.ToString());
		}
		
		static public void SetAutoincrementSeed( DataTable table, string column)
		{
			if ( table.Columns[column] == null )
				return;
			Type type = table.Columns[column].DataType;
			if ( type.ToString() == "System.Int16" || type.ToString() == "System.Int32" || type.ToString() == "System.Int64" ) 
			{
				DataRow maxrow = table.Select("Id=MAX(Id)")[0];
				Int64 seed = (Int64)(maxrow[column]) + 1;
				table.Columns[column].AutoIncrementSeed = seed;
			}
		}
		
		static public string GetIdStringNow()
		{
			return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fffffff");
		}
		
		static public OrderTotal GetOrderTotal( SuposDataSet.OrdersRow order)
		{
			OrderTotal result = new OrderTotal();
			if (order != null)
			{
				SuposDataSet.OrderDetailsRow[] details = (SuposDataSet.OrderDetailsRow[])order.GetChildRows( "FK_orders_OrderDetails" );
				result.TotPrice = 0;
				result.TaxAmount = 0;
				foreach( SuposDataSet.OrderDetailsRow detail in details)
				{
					result.TotPrice += detail.Price*detail.Quantity;
					result.TaxAmount += (Decimal)detail.TaxesRow.Rate * detail.Price;
				}
			}
			return result;
		}
		
	}
	
}