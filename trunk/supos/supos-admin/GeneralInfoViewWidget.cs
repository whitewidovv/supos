// 

using System;
using Gtk;
using Libsupos;

namespace suposadmin
{
	
	
	public partial class GeneralInfoViewWidget : MainViewWidget
	{
		private TextBuffer m_AddressBuffer = null;
		private SuposGeneralInfo m_GeneralInfo;
		
		public GeneralInfoViewWidget( SuposDb Db )
		{
			this.Build();
			m_AddressBuffer = addresstextview.Buffer;
			m_GeneralInfo = Db.GeneralInfo;
			nameentry.Text = m_GeneralInfo.Name;
			m_AddressBuffer.Text = m_GeneralInfo.Address;
			phoneentry.Text = m_GeneralInfo.Phone;
			faxentry.Text = m_GeneralInfo.Fax;
		}

		protected virtual void OnApply (object sender, System.EventArgs e)
		{
			m_GeneralInfo.Name = nameentry.Text;
			m_GeneralInfo.Address = m_AddressBuffer.Text;
			m_GeneralInfo.Phone = phoneentry.Text;
			m_GeneralInfo.Fax = faxentry.Text;
			m_GeneralInfo.ApplyChange();
		}
	}
}
