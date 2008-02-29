// 

using System;
using Libsupos;

namespace suposadmin
{
	
	
	public partial class TaxDialog : Gtk.Dialog
	{
		private SuposTax m_Tax= null;
		
		public TaxDialog()
		{
			this.Build();
			m_Tax  = new SuposTax();
		}
		
		public TaxDialog(SuposTax Tax)
		{
			this.Build();
			if ( Tax != null)
			{
				m_Tax = Tax;
				identry.Text = m_Tax.Id.ToString();
				nameentry.Text = m_Tax.Name;
				ratespinbutton.Value = m_Tax.Rate;	
			}
			else
			{
				m_Tax = new SuposTax();
			}
		}
		
		public SuposTax Tax
		{
			get
			{
				return m_Tax;
			}
		}

		protected virtual void OnOkClicked (object sender, System.EventArgs e)
		{
			if ( m_Tax != null )
			{
				m_Tax.Name = nameentry.Text;
				m_Tax.Rate = (float)ratespinbutton.Value;
			}
		}

		
	}
}
