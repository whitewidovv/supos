using System;
using System.Collections;
using System.Data;
using Gdk;
using Npgsql;

// FIXME remove Db -> SuposCategory
namespace LibSupos
{
	public class SuposCategory
	{
		private int m_Id = 0;
		private string m_Name = null;
		private SuposIcon m_Icon = new SuposIcon();
		private SuposDb m_DataBase = null;
		
		//private ArrayList m_Products;
		
		//******************************
		// Constructors
		//******************************
		public SuposCategory()
		{	
			m_Icon = new SuposIcon();
		}

		public SuposCategory( string filename )
		{	
			m_Icon = new SuposIcon( filename );
		}
		
		public SuposCategory(SuposDb db, int id)
		{	
			m_DataBase = db;
			m_Id = id;
			m_Icon = new SuposIcon();
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
			NpgsqlCommand command = new NpgsqlCommand("SELECT nextval('categories_id_seq')", db.Connection);
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
			command = new NpgsqlCommand("INSERT INTO categories(id, name, icon) VALUES(currval('categories_id_seq'), :name, :icon)", db.Connection);
			NpgsqlParameter name_param = new NpgsqlParameter ( ":name", DbType.String );
			NpgsqlParameter icon_param = new NpgsqlParameter ( ":icon", DbType.Binary );
			name_param.Value = m_Name;
			icon_param.Value = m_Icon.FileBuffer;
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
			this.m_DataBase = db;
			db.Categories.Add(this);
			return true;
			
		}
		
		//*****************************
		// Write change in DB
		//*****************************
		public bool ApplyChange()
		{
			NpgsqlCommand command = new NpgsqlCommand("UPDATE categories SET name=:name, icon=:icon WHERE id=:id", m_DataBase.Connection);
			NpgsqlParameter name_param = new NpgsqlParameter ( ":name", DbType.String );
			NpgsqlParameter icon_param = new NpgsqlParameter ( ":icon", DbType.Binary );
			NpgsqlParameter id_param = new NpgsqlParameter ( ":id", DbType.Int64 );
			name_param.Value = m_Name;
			icon_param.Value = m_Icon.FileBuffer;
			id_param.Value = m_Id;
			command.Parameters.Add(name_param);
			command.Parameters.Add(icon_param);
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