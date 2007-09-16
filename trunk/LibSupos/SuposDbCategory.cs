using System;
using System.Collections;
using Gdk;

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
			Icon = new SuposIcon();
		}

		public SuposDbCategory( string filename )
		{	
			Icon = new SuposIcon( filename );
		}
		
		public SuposDb DataBase
		{
			get 
			{
				return m_DataBase;
			}
			
			set
			{
				m_DataBase = value;
			}
		}
		
		public int Id
		{
			get 
			{
				return m_Id;
			}
			
			set
			{
				m_Id = value;
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
			
			set
			{
				m_Icon = value;
			}
		}
		
		/*
		public bool WriteChangeToDb ()
		{
			return false;
		}
		*/
		
		public bool Remove()
		{
			return false;
		}
	}

}