
using System;
using System.Data;
using Npgsql;

namespace Libsupos
{
	
	
	public class SuposGeneralInfo
	{
		private int m_Id;
		private string m_Name = null;
		private string m_Address = null;
		private string m_Phone = null;
		private string m_Fax = null;
		private SuposDb m_DataBase = null;
		
		public SuposGeneralInfo( SuposDb Db, int Id, string Name, string Address, string Phone, string Fax)
		{
			m_DataBase = Db;
			m_Id = Id;
			m_Name = Name;
			m_Address = Address;
			m_Phone = Phone;
			m_Fax = Fax;
			
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
		public string Address
		{
			get
			{
				return m_Address;
			}
			set
			{
				m_Address = value;
			}
		}
		public string Phone
		{
			get
			{
				return m_Phone;
			}
			set
			{
				m_Phone = value;
			}
		}
		public string Fax
		{
			get
			{
				return m_Fax;
			}
			set
			{
				m_Fax = value;
			}
		}
		
		public bool ApplyChange()
		{
			NpgsqlCommand command = new NpgsqlCommand("UPDATE generalinfo SET businessname=:name, address=:address, phone=:phone, fax=:fax WHERE id=:id", m_DataBase.Connection);
			NpgsqlParameter name_param = new NpgsqlParameter ( ":name", DbType.String );
			NpgsqlParameter address_param = new NpgsqlParameter ( ":address", DbType.String );
			NpgsqlParameter phone_param = new NpgsqlParameter ( ":phone", DbType.String );
			NpgsqlParameter fax_param = new NpgsqlParameter ( ":fax", DbType.String );
			NpgsqlParameter id_param = new NpgsqlParameter ( ":id", DbType.Int32 );
			name_param.Value = m_Name;
			address_param.Value = m_Address;
			phone_param.Value = m_Phone;
			fax_param.Value = m_Fax;
			id_param.Value = m_Id;
			command.Parameters.Add(name_param);
			command.Parameters.Add(address_param);
			command.Parameters.Add(phone_param);
			command.Parameters.Add(fax_param);
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
