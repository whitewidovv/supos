// DialogPreferences.cs created with MonoDevelop
// User: xavier at 20:04 4/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace SuposUser
{	
	public class DialogPreferences : Supos.Gui.DialogPreferencesBase
	{
		
		private Table tab = new Table(7, 2, false);
		private Label fontLabel = new Label("Font :");
		private FontButton fontButton = new FontButton();
		
		public DialogPreferences(Window parent) : base(parent)
		{
			
			tab.Attach(fontLabel, 0, 1, 0, 1);
			tab.Attach(fontButton, 1, 2, 0, 1);
			
			this.notebook.AppendPage(tab, new Label("View") );
			this.ShowAll();
		}
		
		public void LoadSettings( Settings config)
		{
			this.LoadDatabaseSettings( config.dbSettings );
			fontButton.SetFontName(config.viewSettings.Font);
		}
		
		public void ApplySettings( Settings config)
		{
			this.ApplyDatabaseSettings( config.dbSettings );
			config.viewSettings.Font = fontButton.FontName;
		}
	}
}
