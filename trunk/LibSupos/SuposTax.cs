// /home/xavier/Projects/SuPOS/LibSupos/SuposTax.cs
// User: xavier at 15:42 18/10/2007
//

using System;
using System.Data;
using Npgsql;

namespace LibSupos
{
	
	public class SuposTax
	{
		private int m_Id = 0;
		private string m_Name = null;
		private float m_Rate = 0;
		private SuposDb m_DataBase = null;
		
		
		//******************************
		// Constructors
		//******************************
		public SuposTax()
		{
		}
		
		public SuposTax(SuposDb db, int id)
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
		
		public float Rate
		{
			get 
			{
				return m_Rate;
			}
		}
		
		//******************************************
		// Add (insert) the category to a data base
		//******************************************
		public bool InsertIntoDb(SuposDb db)
		{
			if ( m_DataBase != null || m_Id !=0 || db == null)
			{
				return false;
			}
			// Get next ID
			NpgsqlCommand command = new NpgsqlCommand("SELECT nextval('taxes_id_seq')", db.Connection);
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
			command = new NpgsqlCommand("INSERT INTO taxes(id, name, rate) VALUES(currval('categories_id_seq'), :name, :rate)", db.Connection);
			NpgsqlParameter name_param = new NpgsqlParameter ( ":name", DbType.String );
			NpgsqlParameter rate_param = new NpgsqlParameter ( ":rate", DbType.Double );
			name_param.Value = m_Name;
			rate_param.Value = m_Rate;
			command.Parameters.Add(name_param);
			command.Parameters.Add(rate_param);
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
			db.Taxes.Add(this);
			return true;
			
		}
		
		//*****************************
		// Write change in DB
		//*****************************
		public bool ApplyChange()
		{
			NpgsqlCommand command = new NpgsqlCommand("UPDATE taxes SET name=:name, rate=:rate WHERE id=:id", m_DataBase.Connection);
			NpgsqlParameter name_param = new NpgsqlParameter ( ":name", DbType.String );
			NpgsqlParameter rate_param = new NpgsqlParameter ( ":icon", DbType.Double );
			NpgsqlParameter id_param = new NpgsqlParameter ( ":id", DbType.Int64 );
			name_param.Value = m_Name;
			rate_param.Value = m_Rate;
			id_param.Value = m_Id;
			command.Parameters.Add(name_param);
			command.Parameters.Add(rate_param);
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
			return true;
		}
		
		
	}
}
