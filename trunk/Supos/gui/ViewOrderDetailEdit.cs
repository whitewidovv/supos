// ViewOrderDetailEdit.cs created with MonoDevelop
// User: xavier at 20:48 8/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Text.RegularExpressions;
using Gtk;

namespace Supos
{
	
	
	public class ViewOrderDetailEdit : Supos.FormOrderDetail
	{
		static System.Globalization.NumberFormatInfo ni = null;
		private Regex rex;
		
		private string activestring = "0";
		
		public ViewOrderDetailEdit() : base()
		{
			rex = new Regex(@"(?<int>\d*)?(?<sep>.)?(?<dec>\d*)?");
			ni = (System.Globalization.NumberFormatInfo)System.Globalization.CultureInfo.CurrentUICulture.NumberFormat.Clone();
			ni.NumberDecimalSeparator = ".";
			
			this.Resize(this.NRows, this.NColumns+1 );
						
			spinprice.FocusGrabbed += this.OnActive;
			spinprice.IsEditable = false;
			spinquant.FocusGrabbed += this.OnActive;
			spinquant.IsEditable = false;
			
			Button butclearprice;
			butclearprice = new Button();
			butclearprice.Add( new Image(Stock.Clear, IconSize.Button) );
			butclearprice.Clicked += OnClearPrice;
			butclearprice.CanFocus = false;
			
			Button butclearquant;
			butclearquant = new Button();
			butclearquant.Add( new Image(Stock.Clear, IconSize.Button) );
			butclearquant.Clicked += OnClearQuant;
			butclearquant.CanFocus = false;
			
			this.Attach(butclearprice,2,3,1,2,AttachOptions.Expand,AttachOptions.Fill,0,0);
			this.Attach(butclearquant,2,3,2,3,AttachOptions.Expand,AttachOptions.Fill,0,0);
			
			this.ShowAll();
		}
		
		public void SetActiveFromString(string val)
		{
			activestring = val;
			if(spinprice.IsFocus == true)
			{
				spinprice.Value = double.Parse(activestring, ni);
			}
			if(spinquant.IsFocus == true)
			{
				spinquant.Value = (Int64)double.Parse(activestring, ni);
			}
		}
		
		public void OnActive(object sender, EventArgs args)
		{
//			active = (Widget) sender;
			activestring = (sender as SpinButton).Value.ToString(ni);
		
			double parsed = double.Parse(activestring, ni);
			if (parsed == 0.0f)
			{
				activestring = "0";
			}		
			System.Console.WriteLine("ACTIVE : {0}", activestring);
		}
		
		
		public void OnClearPrice(object sender, EventArgs args)
		{
			this.Price = 0;
			spinprice.IsFocus = true;
			activestring = "0";
		}
		
		public void OnClearQuant(object sender, EventArgs args)
		{
			this.Quantity = 0;
			spinquant.IsFocus = true;
			activestring = "0";
		}
		
		public void HandleKeypad( KeyPad.KeyCode code)
		{
			string strinput = activestring;
			System.Console.WriteLine( "handleleypad: {0}",strinput);
			switch (code) {
			case KeyPad.KeyCode.DoubleZero:
				strinput += "00";
				break;
			case KeyPad.KeyCode.Dot:
				strinput += ".";
				break;
			case KeyPad.KeyCode.Zero:
			case KeyPad.KeyCode.One:
			case KeyPad.KeyCode.Two:
			case KeyPad.KeyCode.Three:
			case KeyPad.KeyCode.Four:
			case KeyPad.KeyCode.Five:
			case KeyPad.KeyCode.Six:
			case KeyPad.KeyCode.Seven:
			case KeyPad.KeyCode.Eight:
			case KeyPad.KeyCode.Nine:
				strinput += ((int)code).ToString();
				break;
			default:
				break;
			}
				
			 
			if ( rex.IsMatch(strinput) )
			{				
				string strres="";
				Match match = rex.Match(strinput);
				
				if (match.Groups["int"] != null && match.Groups["int"].Value!="" )
				{
					strres = match.Groups["int"].Value;
					if( code == KeyPad.KeyCode.Minus )
						strres = (double.Parse(strres)-1.0f).ToString();
					if( code == KeyPad.KeyCode.Plus )
						strres = (double.Parse(strres)+1.0f).ToString();
				}
				if (match.Groups["sep"] != null )
					{
						strres += match.Groups["sep"].Value;
						if (match.Groups["dec"] != null )
						{
							strres += match.Groups["dec"].Value;
						}
					}
				System.Console.WriteLine("result : {0}", strres);
				this.SetActiveFromString(strres);
			}
		}
		
	}
}
