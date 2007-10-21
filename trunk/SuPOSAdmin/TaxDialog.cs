// /home/xavier/Projects/SuPOS/SuPOSAdmin/TaxDialog.cs
// User: xavier at 11:44 19/10/2007
//

using System;
using Mono.Unix;
using Gtk;
using Glade;
using LibSupos;

namespace SuposAdmin
{
	
	
	public class TaxDialog
	{
		private SuposTax m_Tax= null;
		
		[Widget] private Gtk.Dialog taxdialog;
		[Widget] private Gtk.Entry identry;
		[Widget] private Gtk.Entry nameentry;
		[Widget] private Gtk.SpinButton ratespinbutton;
		
		public TaxDialog()
		{
			Catalog.Init("suposadmin","./locale");
			Glade.XML gxml = new Glade.XML (null, "suposadmin.glade", "taxdialog", "suposadmin");
			gxml.Autoconnect (this);
			m_Tax  = new SuposTax();
		}
		
		public TaxDialog(SuposTax tax)
		{
			Catalog.Init("suposadmin","./locale");
			Glade.XML gxml = new Glade.XML (null, "suposadmin.glade", "taxdialog", "suposadmin");
			gxml.Autoconnect (this);
			if ( tax != null)
			{
				m_Tax = tax;
				identry.Text = m_Tax.Id.ToString();
				nameentry.Text = m_Tax.Name;
				ratespinbutton.Value = m_Tax.Rate;	
			}
			else
			{
				m_Tax = new SuposTax();
			}			
		}
		
		public int Run()
		{
			return taxdialog.Run();
		}
		
		public void Destroy()
		{
			taxdialog.Destroy();
		}
		
		public SuposTax Tax
		{
			get
			{
				return m_Tax;
			}
		}
		
		
		//*********************************
		// CALLBACKS
		//*********************************
		#pragma warning disable 0169
		private void OnOkClicked ( object sender, EventArgs a )
		{
			if ( m_Tax != null )
			{
				m_Tax.Name = nameentry.Text;
				m_Tax.Rate = (float)ratespinbutton.Value;
			}
			
		}
	}
}
