using System;
using System.Collections;
using System.Data;
using Npgsql;

namespace LibSupos
{
	public class SuposDb
	{	
		// UNDONE Detect disconnections 
		private ArrayList m_Categories = null;
		private ArrayList m_Taxes = null;
		private ArrayList m_Products = null;
		private string m_ConnectionString = null;
		private NpgsqlConnection m_Connection = null;
		
		
		//***************************************
		// Constructor
		//***************************************
		public SuposDb(string ConnectionString)
		{	
			m_ConnectionString = ConnectionString;
		}
		
		//***************************************
		// Properties
		//***************************************
		public NpgsqlConnection Connection
		{
			get 
			{
				return m_Connection;
			}
		}
		
		public ArrayList Categories
		{
			get
			{
				return m_Categories;
			}
		}
		
		public ArrayList Taxes
		{
			get
			{
				return m_Taxes;
			}
		}
		
		public ArrayList Products
		{
			get
			{
				return m_Products;
			}
		}
		
		//***************************************
		// Connect
		//***************************************
		public void Connect()
		{
			m_Connection = new NpgsqlConnection(m_ConnectionString);
			m_Connection.Open();
		}
		
		
		
		//***************************************
		// Disconnect
		//***************************************
		public void Disconnect()
		{
			if ( m_Connection == null)
			{
			    m_Connection.Close();
			}
			m_Connection = null;
			//UNDONE unload categories, tax, ... (free arraylist)
		}
		
		
		
		//***************************************
		// Load Categories in memory
		//***************************************
		public bool LoadCategories()
		{	
			if ( m_Categories != null)
			{
				return false;
			}
			NpgsqlCommand Cmd = new NpgsqlCommand( "SELECT id, name, icon FROM categories", m_Connection );
			NpgsqlDataReader reader;
			try
			{
				reader = Cmd.ExecuteReader();
				m_Categories = new ArrayList();
				while(reader.Read()) 
				{	
					SuposCategory tmpcat = new SuposCategory(this, (int)reader["id"] );
					tmpcat.Name = reader["name"].ToString();
					if ( !reader["icon"].GetType().Equals( typeof(System.DBNull) ) )
					{
						tmpcat.Icon.FileBuffer = (byte[])reader["icon"];
					}
					m_Categories.Add(tmpcat);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine ( e.Message );
				return false;
			}
			
			return true;
		}
		
		//***************************************
		// Load Taxes in memory
		//***************************************
		public bool LoadTaxes()
		{	
			if ( m_Taxes != null)
			{
				return false;
			}
			NpgsqlCommand Cmd = new NpgsqlCommand( "SELECT id, name, rate FROM taxes", m_Connection );
			NpgsqlDataReader reader;
			try
			{
				reader = Cmd.ExecuteReader();
				m_Taxes = new ArrayList();
				while(reader.Read()) 
				{	
					SuposTax tmptax = new SuposTax(this, (int)reader["id"] );
					tmptax.Name = reader["name"].ToString();
					if( reader["rate"].GetType() != typeof(DBNull) )
					{
						tmptax.Rate = (float)reader["rate"];
					}
					m_Taxes.Add(tmptax);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine ( e.Message );
				return false;
			}
			
			return true;
		}
		
		
		//***************************************
		// Load Products in memory
		//***************************************
		public bool LoadProducts()
		{	
			if ( m_Products != null)
			{
				return false;
			}
			NpgsqlCommand Cmd = new NpgsqlCommand( "SELECT id, icon, name, category, tax, price FROM products", m_Connection );
			NpgsqlDataReader reader;
			try
			{
				reader = Cmd.ExecuteReader();
				m_Products = new ArrayList();
				while(reader.Read()) 
				{	
					SuposProduct tmpproduct = new SuposProduct(this, (int)reader["id"] );
					tmpproduct.Name = reader["name"].ToString();
					if ( reader["category"].GetType() != typeof(System.DBNull) )
					{
						tmpproduct.CategoryId = (int)reader["category"];
					}
					if ( reader["tax"].GetType() != typeof(System.DBNull) )
					{
						tmpproduct.TaxId = (int)reader["tax"];
					}
					if ( reader["price"].GetType() != typeof(System.DBNull) )
					{
							tmpproduct.Price = (float)(Decimal)reader["price"]; // TODO Check conversion
					}
					if ( reader["icon"].GetType() != typeof(System.DBNull) )
					{
						tmpproduct.Icon.FileBuffer = (byte[])reader["icon"];
					}
					m_Products.Add(tmpproduct);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine ( e.Message );
				return false;
			}
			
			return true;
		}
		
		//***********************************************
		// Add a category to DB
		//***********************************************
		public bool AddCategory ( SuposCategory category )
		{
			if ( category == null)
			{
				return false;
			}
			return category.InsertIntoDb(this);
		}
		
		//***********************************************
		// Add a tax to DB
		//***********************************************
		public bool AddTax ( SuposTax tax )
		{
			if ( tax == null)
			{
				return false;
			}
			return tax.InsertIntoDb(this);
		}
		
		//***********************************************
		// Add a product to DB
		//***********************************************
		public bool AddProduct ( SuposProduct product )
		{
			if ( product == null)
			{
				return false;
			}
			return product.InsertIntoDb(this);
		}
		
		//*****************************
		// Remove the category from DB 
		//*****************************
		public bool Remove(SuposCategory category)
		{
			NpgsqlCommand command = new NpgsqlCommand("DELETE FROM categories WHERE id=:id", m_Connection);
			NpgsqlParameter id_param = new NpgsqlParameter ( ":id", DbType.Int32 );
			id_param.Value = category.Id;
			command.Parameters.Add(id_param);
			try
			{
				command.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				Console.WriteLine( e.Message );
				return false;
			}
			m_Categories.Remove(category);
			return true;
		}
		
		//*****************************
		// Remove the tax from DB 
		//*****************************
		public bool Remove(SuposTax tax)
		{
			NpgsqlCommand command = new NpgsqlCommand("DELETE FROM taxes WHERE id=:id", m_Connection);
			NpgsqlParameter id_param = new NpgsqlParameter ( ":id", DbType.Int32 );
			id_param.Value = tax.Id;
			command.Parameters.Add(id_param);
			try
			{
				command.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				Console.WriteLine( e.Message );
				return false;
			}
			m_Taxes.Remove(tax);
			return true;
		}
		
		//*****************************
		// Remove the product from DB 
		//*****************************
		public bool Remove(SuposProduct product)
		{
			NpgsqlCommand command = new NpgsqlCommand("DELETE FROM products WHERE id=:id", m_Connection);
			NpgsqlParameter id_param = new NpgsqlParameter ( ":id", DbType.Int32 );
			id_param.Value = product.Id;
			command.Parameters.Add(id_param);
			try
			{
				command.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				Console.WriteLine( e.Message );
				return false;
			}
			m_Products.Remove(product);
			return true;
		}
		
		//******************************
		//
		//******************************
		public SuposTax TaxFromId(int id)
		{
			foreach (SuposTax tax in m_Taxes)
			{
				if( tax.Id == id )
					return tax;
			}
			return null;
		}
		
		//******************************
		//
		//******************************
		public SuposCategory CategoryFromId(int id)
		{
			foreach (SuposCategory category in m_Categories)
			{
				if( category.Id == id )
					return category;
			}
			return null;
		}
	}
}