// DialogPreferencesBase.cs created with MonoDevelop
// User: xavier at 01:25 3/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;
using Supos.Core;

namespace Supos.Gui
{
	
	
	public class DialogPreferencesBase : Gtk.Dialog
	{
		private Notebook notebook;
		private FormDatabasePreferences dbForm;
		
		public DialogPreferencesBase(Window parent) : base("Preferences", parent, DialogFlags.Modal, Stock.Ok, ResponseType.Ok, Stock.Cancel, ResponseType.Cancel)
		{
			notebook = new Notebook();
			
			dbForm = new FormDatabasePreferences();
			dbForm.BorderWidth = 6;
			notebook.AppendPage(dbForm, new Label("Database") );
			
			this.VBox.PackStart(notebook, true, true, 6);
			this.ShowAll();
		}
		
		public void LoadDatabaseSettings(DbSettings config)
		{
			dbForm.LoadSettings(config);
		}
		
		public void ApplyDatabaseSettings(DbSettings config)
		{
			dbForm.ApplySettings(config);
		}
		
	}
}
