// KeyPad.cs created with MonoDevelop
// User: xavier at 00:20 5/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace Supos.Gui
{
	
	
	public class KeyPad : Gtk.Table
	{
		private Button but1;
		private Button but2;
		private Button but3;
		private Button but4;
		private Button but5;
		private Button but6;
		private Button but7;
		private Button but8;
		private Button but9;
		private Button but0;
		private Button but00;
		private Button butdot;
		
		private Button butok;
		private Button butadd;
		private Button butrem;
		
		public KeyPad() : base(4, 4, true)
		{
			but1 = new Button();
			but2 = new Button();
			but3 = new Button();
			but4 = new Button();
			but5 = new Button();
			but6 = new Button();
			but7 = new Button();
			but8 = new Button();
			but9 = new Button();
			but0 = new Button();
			but00 = new Button();
			butdot = new Button();
			
			but1.Label="1";
			but1.Clicked += this.OnButton1Clicked;
			but1.CanFocus = false;
			but2.Label="2";
			but2.Clicked += this.OnButton2Clicked;
			but2.CanFocus = false;
			but3.Label="3";
			but3.Clicked += this.OnButton3Clicked;
			but3.CanFocus = false;
			but4.Label="4";
			but4.Clicked += this.OnButton4Clicked;
			but4.CanFocus = false;
			but5.Label="5";
			but5.Clicked += this.OnButton5Clicked;
			but5.CanFocus = false;
			but6.Label="6";
			but6.Clicked += this.OnButton6Clicked;
			but6.CanFocus = false;
			but7.Label="7";
			but7.Clicked += this.OnButton7Clicked;
			but7.CanFocus = false;
			but8.Label="8";
			but8.Clicked += this.OnButton8Clicked;
			but8.CanFocus = false;
			but9.Label="9";
			but9.Clicked += this.OnButton9Clicked;
			but9.CanFocus = false;
			but0.Label="0";
			but0.Clicked += this.OnButton0Clicked;
			but0.CanFocus = false;
			but00.Label="00";
			but00.Clicked += this.OnButton00Clicked;
			but00.CanFocus = false;
			butdot.Label=".";
			butdot.Clicked += this.OnButtonDotClicked;
			butdot.CanFocus = false;

			butok = new Button();
			butok.Add( new Image(Stock.Ok, IconSize.Button) );
			butok.Clicked += this.OnButtonOkClicked;
			butok.CanFocus = false;
			
			butadd = new Button();
			butadd.Add( new Image(Stock.Add, IconSize.Button) );
			butadd.Clicked += this.OnButtonAddClicked;
			butadd.CanFocus = false;
			
			butrem = new Button();
			butrem.Add( new Image(Stock.Remove, IconSize.Button) );
			butrem.Clicked += this.OnButtonRemClicked;
			butrem.CanFocus = false;
			
			Attach(but1, 0, 1, 0, 1);
			Attach(but2, 1, 2, 0, 1);
			Attach(but3, 2, 3, 0, 1);
			Attach(but4, 0, 1, 1, 2);
			Attach(but5, 1, 2, 1, 2);
			Attach(but6, 2, 3, 1, 2);
			Attach(but7, 0, 1, 2, 3);
			Attach(but8, 1, 2, 2, 3);
			Attach(but9, 2, 3, 2, 3);
			Attach(butdot, 0, 1, 3, 4);
			Attach(but0, 1, 2, 3, 4);
			Attach(but00, 2, 3, 3, 4);
			
			Attach(butrem, 3, 4, 0, 1);
			Attach(butadd, 3, 4, 1, 2);
			Attach(butok, 3, 4, 2, 4);
			
			this.ShowAll();
		}

		public enum KeyCode
		{
			Zero = 0,
			One = 1,
			Two = 2,
			Three = 3,
			Four = 4,
			Five = 5,
			Six = 6,
			Seven = 7,
			Eight = 8,
			Nine = 9,
			DoubleZero = 100,
			Dot = 101,
			Minus = 102,
			Plus = 103,
			Ok = 104
		}
		
		public delegate void ClickedEventHandler(object sender, KeypadEventArgs e);	
		public event ClickedEventHandler Clicked;
		public class KeypadEventArgs : EventArgs
		{
			public KeyCode code;
		}
		
		
		
		public void OnButton1Clicked(object sender, EventArgs args)
		{
			if( Clicked != null )
			{
				KeypadEventArgs newargs = new KeypadEventArgs();
				newargs.code = KeyCode.One;
				Clicked(this, newargs );
			}
		}
		
		public void OnButton2Clicked(object sender, EventArgs args)
		{
			if( Clicked != null )
			{
				KeypadEventArgs newargs = new KeypadEventArgs();
				newargs.code = KeyCode.Two;
				Clicked(this, newargs );
			}
		}
		
		public void OnButton3Clicked(object sender, EventArgs args)
		{
			if( Clicked != null )
			{
				KeypadEventArgs newargs = new KeypadEventArgs();
				newargs.code = KeyCode.Three;
				Clicked(this, newargs );
			}
		}
		
		public void OnButton4Clicked(object sender, EventArgs args)
		{
			if( Clicked != null )
			{
				KeypadEventArgs newargs = new KeypadEventArgs();
				newargs.code = KeyCode.Four;
				Clicked(this, newargs );
			}
		}
		
		public void OnButton5Clicked(object sender, EventArgs args)
		{
			if( Clicked != null )
			{
				KeypadEventArgs newargs = new KeypadEventArgs();
				newargs.code = KeyCode.Five;
				Clicked(this, newargs );
			}
		}
		
		public void OnButton6Clicked(object sender, EventArgs args)
		{
			if( Clicked != null )
			{
				KeypadEventArgs newargs = new KeypadEventArgs();
				newargs.code = KeyCode.Six;
				Clicked(this, newargs );
			}
		}
		
		public void OnButton7Clicked(object sender, EventArgs args)
		{
			if( Clicked != null )
			{
				KeypadEventArgs newargs = new KeypadEventArgs();
				newargs.code = KeyCode.Seven;
				Clicked(this, newargs );
			}
		}
		
		public void OnButton8Clicked(object sender, EventArgs args)
		{
			if( Clicked != null )
			{
				KeypadEventArgs newargs = new KeypadEventArgs();
				newargs.code = KeyCode.Eight;
				Clicked(this, newargs );
			}
		}
		
		public void OnButton9Clicked(object sender, EventArgs args)
		{
			if( Clicked != null )
			{
				KeypadEventArgs newargs = new KeypadEventArgs();
				newargs.code = KeyCode.Nine;
				Clicked(this, newargs );
			}
		}
		
		public void OnButton0Clicked(object sender, EventArgs args)
		{
			if( Clicked != null )
			{
				KeypadEventArgs newargs = new KeypadEventArgs();
				newargs.code = KeyCode.Zero;
				Clicked(this, newargs );
			}
		}
		
		public void OnButton00Clicked(object sender, EventArgs args)
		{
			if( Clicked != null )
			{
				KeypadEventArgs newargs = new KeypadEventArgs();
				newargs.code = KeyCode.DoubleZero;
				Clicked(this, newargs );
			}
		}
		
		
		public void OnButtonDotClicked(object sender, EventArgs args)
		{
			if( Clicked != null )
			{
				KeypadEventArgs newargs = new KeypadEventArgs();
				newargs.code = KeyCode.Dot;
				Clicked(this, newargs );
			}
		}
		
		public void OnButtonRemClicked(object sender, EventArgs args)
		{
			if( Clicked != null )
			{
				KeypadEventArgs newargs = new KeypadEventArgs();
				newargs.code = KeyCode.Minus;
				Clicked(this, newargs );
			}
		}
		
		public void OnButtonAddClicked(object sender, EventArgs args)
		{
			if( Clicked != null )
			{
				KeypadEventArgs newargs = new KeypadEventArgs();
				newargs.code = KeyCode.Plus;
				Clicked(this, newargs );
			}
		}
		
		public void OnButtonOkClicked(object sender, EventArgs args)
		{
			if( Clicked != null )
			{
				KeypadEventArgs newargs = new KeypadEventArgs();
				newargs.code = KeyCode.Ok;
				Clicked(this, newargs );
			}
		}
		
	}
}
