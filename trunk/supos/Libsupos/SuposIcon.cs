using System;
using System.IO;
using Gdk;

namespace Libsupos
{
	public class SuposIcon
	{
		private static int m_MaxSize = 500;
		private byte[] m_FileBuffer = null;
		
		
		//*****************
		// Constructor
		//***************
		public SuposIcon ()
		{		
		}
		
		public SuposIcon( string filename )
		{
			Set(filename);
		}
		
		public SuposIcon ( byte[] filebuffer )
		{
			FileBuffer = filebuffer;
		}
		
		//**********************
		//  Properties
		//**********************
		public byte[] FileBuffer
		{
			get
			{
				return m_FileBuffer;
			}
			set
			{
				m_FileBuffer = value;
			}
		}
		
		public void Clear()
		{
			FileBuffer = null;
		}
		
		//*****************
		// Methods
		//*****************
		public bool Set (string filename )
		{
			if (!File.Exists(filename) )
			{
				return false;
			}
			Pixbuf pb = new Pixbuf(filename);
			if ( pb == null || pb.Height > m_MaxSize || pb.Width > m_MaxSize )
			{
				Console.WriteLine ("File not an image or too big");
				return false;
			}
			FileStream fs = new FileStream( filename, FileMode.Open, FileAccess.Read);
			BinaryReader br = new BinaryReader(new BufferedStream(fs));
			FileBuffer = br.ReadBytes((Int32)fs.Length);
			return true;
		}
		
		public Pixbuf GetPixbuf()
		{
			if ( FileBuffer != null )
			{
				Pixbuf pb = new Pixbuf( FileBuffer );
				return pb;
			}
			return null;
		}
	}
}
