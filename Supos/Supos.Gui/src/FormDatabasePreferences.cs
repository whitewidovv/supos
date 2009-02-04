// DatabasePreferences.cs created with MonoDevelop
// User: xavier at 01:10 3/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Text.RegularExpressions;
using Gtk;
using Supos.Core;

namespace Supos.Gui
{
	
	
	public class FormDatabasePreferences : Gtk.Table
	{
		private Label typeLabel, serverLabel, portLabel, userLabel, passLabel, dbLabel, mediaLabel;
		private ComboBox typeCombo;
		private Entry serverEntry, userEntry, passEntry, dbEntry;
		private SpinButton portSpinButton;
		private CheckButton passCheck;
		private Button dbButton;
		private FileChooserButton mediaButton;
		
		public FormDatabasePreferences() : base(7, 3, false)
		{	
			typeLabel = new Gtk.Label("Type :");
			serverLabel = new Gtk.Label("Server :");
			portLabel = new Gtk.Label("Port :");
			userLabel = new Gtk.Label("Username :");
			passLabel = new Gtk.Label("Password :");
			dbLabel = new Gtk.Label("Database :");
			mediaLabel = new Gtk.Label("Medias path :");
			typeCombo = ComboBox.NewText();
			serverEntry = new Entry();
			portSpinButton = new SpinButton(0f,65536f,1f);
			userEntry = new Entry();
			passEntry = new Entry();
			dbEntry = new Entry();
			passCheck = new CheckButton("Save password");
			dbButton= new Button(Stock.Open);
			mediaButton= new FileChooserButton("Choose the media directory", FileChooserAction.SelectFolder);
						
			typeLabel.SetAlignment(0, (float)0.5);
			serverLabel.SetAlignment(0, (float)0.5);
			portLabel.SetAlignment(0, (float)0.5);
			userLabel.SetAlignment(0, (float)0.5);
			passLabel.SetAlignment(0, (float)0.5);
			dbLabel.SetAlignment(0, (float)0.5);
			mediaLabel.SetAlignment(0, (float)0.5);
			
			typeCombo.AppendText("SQLite");
			typeCombo.AppendText("PostgreSQL");
			typeCombo.Changed += OnTypeComboChanged;
			
			passEntry.Visibility = false;
			dbButton.Clicked += OnDbButton; 
			
			this.Attach(typeLabel, 0, 1, 0, 1);
			this.Attach(serverLabel, 0, 1, 1, 2);
			this.Attach(portLabel, 0, 1, 2, 3);
			this.Attach(userLabel, 0, 1, 3, 4);
			this.Attach(passLabel, 0, 1, 4, 5);
			this.Attach(dbLabel, 0, 1, 5, 6);
			this.Attach(mediaLabel, 0, 1, 6, 7);
			
			this.Attach(typeCombo, 1, 3, 0, 1);
			
			this.Attach(serverEntry, 1, 3, 1, 2);
			this.Attach(portSpinButton, 1, 3, 2, 3);
			this.Attach(userEntry, 1, 3, 3, 4);
			this.Attach(passEntry, 1, 2, 4, 5);
			this.Attach(dbEntry, 1, 2, 5, 6);
			
			this.Attach(passCheck, 2, 3, 4, 5);			
			this.Attach(dbButton, 2, 3, 5, 6);		
			this.Attach(mediaButton, 1, 3, 6, 7);
			
		}
		
		public void LoadSettings( DbSettings config)
		{
			TypeComboSetActive( config.DbType );
			serverEntry.Text = config.Server;
			portSpinButton.Value = config.Port;
			userEntry.Text = config.User;
			passEntry.Text = config.Password;
			dbEntry.Text = config.Database;
			mediaButton.SetCurrentFolder( config.MediaPath );
		}
		
		public void ApplySettings(DbSettings config)
		{
			config.DbType = typeCombo.ActiveText;
			config.Server = serverEntry.Text;
			config.Port = portSpinButton.ValueAsInt;
			config.User = userEntry.Text;
			config.Password = passEntry.Text;
			config.Database = dbEntry.Text;
			config.MediaPath = mediaButton.CurrentFolder;
		}
		
		private void TypeComboSetActive(string active)
		{
			TreeIter iter;
			typeCombo.Model.GetIterFirst(out iter);
			do {
				if( (string)typeCombo.Model.GetValue(iter, 0)== active ) {								
					typeCombo.SetActiveIter(iter);
					break;
				}
			}
			while (typeCombo.Model.IterNext(ref iter) );
		}
		
		private void OnDbButton(object sender, EventArgs e)
		{
			FileChooserDialog dlg = new FileChooserDialog("Choose the file to open",
                                      null,
                                      FileChooserAction.Open,
                                      "Cancel",ResponseType.Cancel,
                                      "Open",ResponseType.Accept);
			if( dlg.Run() == (int)ResponseType.Accept )
			{
				dbEntry.Text = dlg.Filename;
			}
			dlg.Destroy();
		}

		private void OnTypeComboChanged (object sender, EventArgs e)
		{
			string selected = ((ComboBox)sender).ActiveText;
			switch (selected) {
			case "SQLite" :
				dbButton.Sensitive = true;
				serverEntry.Sensitive = false;
				portSpinButton.Sensitive = false;
				userEntry.Sensitive = false;
				passEntry.Sensitive = false;
				passCheck.Sensitive = false;
				break;
			case "PostgreSQL" :
				dbButton.Sensitive = false;
				serverEntry.Sensitive = true;
				portSpinButton.Sensitive = true;
				userEntry.Sensitive = true;
				passEntry.Sensitive = true;
				passCheck.Sensitive = true;
				break;
			default :
				break;
			}
		}
	}
}
