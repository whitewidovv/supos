using System;
using System.Collections;
using System.Data;
using Gdk;
using Npgsql;

namespace LibSupos
{
	public class SuposDbCategory
	{
		private int m_Id = 0;
		private string m_Name = null;
		private SuposIcon m_Icon = new SuposIcon();
		private SuposDb m_DataBase = null;
		
		//private ArrayList m_Products;
		
		public SuposDbCategory()
		{	
			m_Icon = new SuposIcon();
		}

		public SuposDbCategory( string filename )
		{	
			m_Icon = new SuposIcon( filename );
		}
		
		public SuposDbCategory(SuposDb db, int id)
		{	
			m_DataBase = db;
			m_Id = id;
			m_Icon = new SuposIcon();
		}
		
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
			command = new NpgsqlCommand("INSERT INTO categories(id, name, icon) VALUES(currval('categories_id_seq'), :name, :bytesData)", db.Connection);
			NpgsqlParameter name_param = new NpgsqlParameter ( ":name", DbType.String );
			NpgsqlParameter icon_param = new NpgsqlParameter ( ":bytesData", DbType.Binary );
			name_param.Value = Name;
			icon_param.Value = Icon.FileBuffer;
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
			db.Categories.Add(this);
			return true;
			
		}
		
		public bool Remove()
		{
			return false;
		}
	}

}