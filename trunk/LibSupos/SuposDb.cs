using System;
using System.Collections;
using System.Data;
using Npgsql;

namespace LibSupos
{
	public class SuposDb
	{	
		private ArrayList m_Categories = null;
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
			}
			catch (Exception e)
			{
				Console.WriteLine ( e.Message );
				return false;
			}
			m_Categories = new ArrayList();
			while(reader.Read()) 
			{	
				SuposDbCategory tmpcat = new SuposDbCategory(this, (int)reader["id"] );
				tmpcat.Name = reader["name"].ToString();
				if ( !reader["icon"].GetType().Equals( typeof(System.DBNull) ) )
				{
					tmpcat.Icon.FileBuffer = (byte[])reader["icon"];
				}
				m_Categories.Add(tmpcat);
			}
			return true;
		}
		

		//***********************************************
		// Return the category list (null if not loaded)
		//***********************************************
		public ArrayList GetCategories ()
		{	
			return m_Categories;
		}
		
		//***********************************************
		// Add a category to DB
		//***********************************************
		public bool AddCategory ( SuposDbCategory category )
		{
			if ( category == null)
			{
				return false;
			}
			return category.InsertIntoDb(this);
		}
		
		
	}
}