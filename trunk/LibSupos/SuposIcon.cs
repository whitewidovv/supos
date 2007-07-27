// /home/xavier/Projects/SuPOS/LibSupos/SuposIcon.cs created with MonoDevelop
// User: xavier at 18:44 26/07/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;
using Gdk;

namespace LibSupos
{
	public class SuposIcon
	{
		private static int m_MaxSize = 500;
		
		public byte[] FileBuffer = null;
		
		public SuposIcon ()
		{
			
		}
		
		public SuposIcon( string filename )
		{
			if (File.Exists(filename) )
			{
				Pixbuf pb = new Pixbuf(filename);
				if ( pb == null || pb.Height > m_MaxSize || pb.Width > m_MaxSize )
				{
					Console.WriteLine ("File not an image or too big");
					return;
				}
				else
				{
					FileStream fs = new FileStream( filename, FileMode.Open, FileAccess.Read);
					BinaryReader br = new BinaryReader(new BufferedStream(fs));
					FileBuffer = br.ReadBytes((Int32)fs.Length);
				}
			}
		}
		
		public SuposIcon ( byte[] filebuffer )
		{
			FileBuffer = filebuffer;
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
		
		public byte[] GetFileBuffer()
		{
			return FileBuffer;
		}
	}
}
