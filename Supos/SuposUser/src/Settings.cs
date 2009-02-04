// Settings.cs created with MonoDevelop
// User: xavier at 15:26 4/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Supos.Core;

namespace SuposUser {

	public class SettingsHandler {
		static string settingsFile;
		static XmlSerializer settingsSerializer = new XmlSerializer (typeof (Settings));
		public static Settings Settings;
		
		static SettingsHandler ()
		{
			string rootDir = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);
			Path = System.IO.Path.Combine (rootDir, "supos");
			settingsFile = System.IO.Path.Combine (Path, "usersettings.xml");
			if (File.Exists (settingsFile)) {
				try {
					using (Stream s = File.OpenRead (settingsFile)) {
						Settings = (Settings) settingsSerializer.Deserialize (s);
					}
				} catch {
					Settings = new Settings ();
				}
			} else
				Settings = new Settings ();

//			if (Settings.preferred_font_family.Length == 0)
//				Settings.preferred_font_family = "Sans";
//			if (Settings.preferred_font_size <= 0)
//				Settings.preferred_font_size = 100;
		}
		
		public static void CheckUpgrade ()
		{
			// no new version
			int version = 1; // TODO : version?
			if (Settings.LastSeenVersion == version)
				return;
			
			// new install
			if (! File.Exists (settingsFile)) {
				Settings.LastSeenVersion = version;
				Save ();
				return;
			}
		}

		public static void Save ()
		{
			EnsureSettingsDirectory ();
			using (FileStream fs = File.Create (settingsFile)){
				settingsSerializer.Serialize (fs, Settings);
			}
		}
		
		// these can be used for other types of settings too
		public static string Path;
		public static void EnsureSettingsDirectory ()
		{
			DirectoryInfo d = new DirectoryInfo (Path);
			if (!d.Exists)
				d.Create ();
		}
		
	}
		
	public class Settings {
		// public to allow serialization	
		public int LastSeenVersion = -1;
		public DbSettings dbSettings = new DbSettings();

		
	}
}
