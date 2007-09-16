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
		private NpgsqlConnection m_SuposDbConnection = null;
		
		
		//***************************************
		// Constructor
		//***************************************
		public SuposDb(string ConnectionString)
		{	
			m_ConnectionString = ConnectionString;
		}
		
		
		//***************************************
		// Connect
		//***************************************
		public void Connect()
		{
			m_SuposDbConnection = new NpgsqlConnection(m_ConnectionString);
			m_SuposDbConnection.Open();
		}
		
		
		
		//***************************************
		// Disconnect
		//***************************************
		public void Disconnect()
		{
			if ( m_SuposDbConnection == null)
			{
			    m_SuposDbConnection.Close();
			}
			m_SuposDbConnection = null;
		}
		
		
		
		//***************************************
		// Load Categories in memory
		//***************************************
		public bool LoadCategories()
		{	
			if ( m_Categories == null)
			{
				NpgsqlCommand Cmd = new NpgsqlCommand( "SELECT id, name, icon FROM categories", m_SuposDbConnection );
				try
				{
					NpgsqlDataReader Reader = Cmd.ExecuteReader();
					m_Categories = new ArrayList();
					while(Reader.Read()) 
					{	
						SuposDbCategory tmpcat = new SuposDbCategory(  );
						tmpcat.DataBase = this;
						tmpcat.Id = (int)Reader["id"];
						tmpcat.Name = Reader["name"].ToString();
						if ( !Reader["icon"].GetType().Equals( typeof(System.DBNull) ) )
						{
							tmpcat.Icon.FileBuffer = (byte[])Reader["icon"];
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
			return false;
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
			if ( category != null && category.DataBase == null )
			{
				category.DataBase = this;
				NpgsqlCommand command = new NpgsqlCommand("INSERT INTO categories(name, icon) VALUES(:name, :bytesData)", m_SuposDbConnection);
				NpgsqlParameter name_param = new NpgsqlParameter ( ":name", DbType.String );
				NpgsqlParameter icon_param = new NpgsqlParameter ( ":bytesData", DbType.Binary );
				name_param.Value = category.Name;
				icon_param.Value = category.Icon.FileBuffer;
				command.Parameters.Add(name_param);
				command.Parameters.Add(icon_param);
				try
				{
					command.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					Console.WriteLine( e.Message);
					return false;
				}
				if ( m_Categories != null )
				{
					//TODO add ID to category
					//m_Categories.Add ( category );
				}
				return true;
			}
			return false;
		}
		
		
	}
}
//testsvn