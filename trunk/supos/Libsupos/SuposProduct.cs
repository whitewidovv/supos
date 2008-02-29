using System;
using System.Collections;
using System.Data;
using Gdk;
using Npgsql;

namespace Libsupos
{
	
	
	public class SuposProduct
	{
		private int m_Id = 0;
		private string m_Name = null;
		private int m_CategoryId = 0; 
		private int m_TaxId = 0;
		private float m_Price = 0;
		private SuposIcon m_Icon = new SuposIcon();
		private SuposDb m_DataBase = null;
		
		
		//******************************
		// Constructors
		//******************************
		public SuposProduct()
		{	
		}

		public SuposProduct(SuposDb db, int id)
		{	
			m_DataBase = db;
			m_Id = id;
		}
		
		//*************************
		// Properties
		//*************************
		public SuposDb DataBase
		{
			get 
			{
				return m_DataBase;
			}
		}
		
		public int Id
		{
			get 
			{
				return m_Id;
			}
		}
		
		public string Name
		{
			get 
			{
				return m_Name;
			}
			
			set
			{
				m_Name = value;
			}
		}
		
		public SuposIcon Icon
		{
			get 
			{
				return m_Icon;
			}
		}
		
		public int CategoryId
		{
			get 
			{
				return m_CategoryId;
			}
			
			set
			{
				m_CategoryId = value;
			}
		}
		
		public int TaxId
		{
			get 
			{
				return m_TaxId;
			}
			
			set
			{
				m_TaxId = value;
			}
		}
		
		public float Price
		{
			get 
			{
				return m_Price;
			}
			
			set
			{
				m_Price = value;
			}
		}
		
		public float PriceTaxInc
		{
			get 
			{
				if(Tax != null)
					return m_Price*(1+(Tax.Rate/100));
				else
					return m_Price;
			}
		}
		
		public SuposCategory Category
		{
			get
			{
				return m_DataBase.CategoryFromId( m_CategoryId );
			}
		}
		
		public SuposTax Tax
		{
			get
			{
				return m_DataBase.TaxFromId( m_TaxId );
			}
		}
		
		
		//******************************************
		// Add (insert) the product to a data base
		//******************************************
		public bool InsertIntoDb(SuposDb db)
		{
			if ( m_DataBase != null || m_Id !=0 || db == null)
			{
				return false;
			}
			// Get next ID
			NpgsqlCommand command = new NpgsqlCommand("SELECT nextval('products_id_seq')", db.Connection);
			try
			{
				m_Id = (int)(Int64)command.ExecuteScalar();
			}
			catch (Exception e)
			{
				Console.WriteLine( e.Message);
				return false;
			}
			command.Dispose();
			// Insert row
			command = new NpgsqlCommand("INSERT INTO products(id, name, icon, category, tax, price) VALUES(currval('products_id_seq'), :name, :icon, :category, :tax, :price)", db.Connection);
			NpgsqlParameter name_param = new NpgsqlParameter ( ":name", DbType.String );
			NpgsqlParameter icon_param = new NpgsqlParameter ( ":icon", DbType.Binary );
			NpgsqlParameter category_param = new NpgsqlParameter ( ":category", DbType.Int32 );
			NpgsqlParameter tax_param = new NpgsqlParameter ( ":tax", DbType.Int32 );
			NpgsqlParameter price_param = new NpgsqlParameter ( ":price", NpgsqlTypes.NpgsqlDbType.Numeric ); //NpgsqlTypes.NpgsqlDbType.Numeric
			name_param.Value = m_Name;
			icon_param.Value = m_Icon.FileBuffer;
			category_param.Value = m_CategoryId;
			tax_param.Value = m_TaxId;
			price_param.Value = m_Price;
			command.Parameters.Add(name_param);
			command.Parameters.Add(icon_param);
			command.Parameters.Add(category_param);
			command.Parameters.Add(tax_param);
			command.Parameters.Add(price_param);
			try
			{
				command.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				Console.WriteLine( e.Message);
				return false;
			}
			this.m_DataBase = db;
			db.Products.Add(this);
			return true;
			
		}
		
		//*****************************
		// Write change in DB
		//*****************************
		public bool ApplyChange()
		{
			NpgsqlCommand command = new NpgsqlCommand("UPDATE products SET name=:name, icon=:icon, category=:category, tax=:tax, price=:price WHERE id=:id", m_DataBase.Connection);
			NpgsqlParameter id_param = new NpgsqlParameter ( ":id", DbType.Int32 );
			NpgsqlParameter name_param = new NpgsqlParameter ( ":name", DbType.String );
			NpgsqlParameter icon_param = new NpgsqlParameter ( ":icon", DbType.Binary );
			NpgsqlParameter category_param = new NpgsqlParameter ( ":category", DbType.Int32 );
			NpgsqlParameter tax_param = new NpgsqlParameter ( ":tax", DbType.Int32 );
			NpgsqlParameter price_param = new NpgsqlParameter ( ":price", NpgsqlTypes.NpgsqlDbType.Numeric );
			id_param.Value = m_Id;
			name_param.Value = m_Name;
			icon_param.Value = m_Icon.FileBuffer;
			category_param.Value = m_CategoryId;
			tax_param.Value = m_TaxId;
			price_param.Value = m_Price;
			command.Parameters.Add(id_param);
			command.Parameters.Add(name_param);
			command.Parameters.Add(icon_param);
			command.Parameters.Add(category_param);
			command.Parameters.Add(tax_param);
			command.Parameters.Add(price_param);
			try
			{
				command.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				Console.WriteLine( e.Message );
				return false;
			}
			return true;
		}
		
		
	}
}
