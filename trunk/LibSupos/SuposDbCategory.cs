using System;
using System.Collections;
using Gdk;

namespace LibSupos
{
	public class SuposDbCategory
	{
		public int Id = 0;
		public string Name = null;
		public SuposIcon Icon = new SuposIcon();
		public SuposDb DataBase = null;
		
		//private ArrayList m_Products;
		
		public SuposDbCategory()
		{	
			Icon = new SuposIcon();
		}

		public SuposDbCategory( string filename )
		{	
			Icon = new SuposIcon( filename );
		}
		
	}

}