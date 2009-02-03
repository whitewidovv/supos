// DatabasePreferences.cs created with MonoDevelop
// User: xavier at 01:10 3/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Text.RegularExpressions;
using Gtk;

namespace Supos.Gui
{
	
	
	public class FormDatabasePreferences : Gtk.Table
	{
		private Label typeLabel, serverLabel, portLabel, userLabel, passLabel, dbLabel, mediaLabel;
		private ComboBox typeCombo;
		private Entry serverEntry, portEntry, userEntry, passEntry, dbEntry, mediaEntry;
		private CheckButton passCheck;
		private Button dbButton, mediaButton;
		
		static private Regex configrex = new Regex(@"factory=(?<factory>[^;]*);(?<constr>[^$]*)");
		//static private Regex pgrex = new Regex(@"Server=(?<server>[^;]*);Port=(?<port>[^;]*);User ID=(?<user>[^;]*);Password=(?<pass>[^;]*);Database=(?<database>[^;]*)");
		static private Regex sqliterex = new Regex(@"Data Source=(?<datasource>[^$]*)");
		
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
			portEntry = new Entry();
			userEntry = new Entry();
			passEntry = new Entry();
			dbEntry = new Entry();
			mediaEntry = new Entry();
			passCheck = new CheckButton("Save password");
			dbButton= new Button(Stock.Open);
			mediaButton= new Button(Stock.Open);
						
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
			
			this.Attach(typeLabel, 0, 1, 0, 1);
			this.Attach(serverLabel, 0, 1, 1, 2);
			this.Attach(portLabel, 0, 1, 2, 3);
			this.Attach(userLabel, 0, 1, 3, 4);
			this.Attach(passLabel, 0, 1, 4, 5);
			this.Attach(dbLabel, 0, 1, 5, 6);
			this.Attach(mediaLabel, 0, 1, 6, 7);
			
			this.Attach(typeCombo, 1, 3, 0, 1);
			
			this.Attach(serverEntry, 1, 3, 1, 2);
			this.Attach(portEntry, 1, 3, 2, 3);
			this.Attach(userEntry, 1, 3, 3, 4);
			this.Attach(passEntry, 1, 2, 4, 5);
			this.Attach(dbEntry, 1, 2, 5, 6);
			this.Attach(mediaEntry, 1, 2, 6, 7);
			
			this.Attach(passCheck, 2, 3, 4, 5);			
			this.Attach(dbButton, 2, 3, 5, 6);		
			this.Attach(mediaButton, 2, 3, 6, 7);
			
		}
		
		public void SetFromConfig(string confstring)
		{
			if ( configrex.IsMatch(confstring) )
			{
				Match confmatch = configrex.Match(confstring);
				if (confmatch.Groups["factory"] != null)
				{
					switch( confmatch.Groups["factory"].Value ) {
					case "Sqlite" :
						TypeComboSetActive("SQLite");
						if( sqliterex.IsMatch(confmatch.Groups["constr"].Value) ) {
							System.Console.WriteLine("sqlitematch 1");
							Match constrmatch = sqliterex.Match(confmatch.Groups["constr"].Value);
							if( constrmatch.Groups["datasource"] != null) {								
								System.Console.WriteLine("sqlitematch 2");
								dbEntry.Text = constrmatch.Groups["datasource"].Value;
							}
						}
						break;
					case "Npgsql" :
						TypeComboSetActive("PostgreSQL");
						//TODO other fields
						break;
					default :
						break;
						
					}
				}
			}
		}
		
		public string GetConfig()
		{
			return "";
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
		
		private void OnTypeComboChanged (object sender, EventArgs e)
		{
			string selected = ((ComboBox)sender).ActiveText;
			switch (selected) {
			case "SQLite" :
				dbButton.Sensitive = true;
				serverEntry.Sensitive = false;
				portEntry.Sensitive = false;
				userEntry.Sensitive = false;
				passEntry.Sensitive = false;
				passCheck.Sensitive = false;
				break;
			case "PostgreSQL" :
				dbButton.Sensitive = false;
				serverEntry.Sensitive = true;
				portEntry.Sensitive = true;
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
