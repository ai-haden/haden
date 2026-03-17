using System;
using System.IO;

namespace Haden.Library
{
    public class HadenCore
    {
        public DesiredAttribute DesiredAttribute { get; set; }
        public SettingsDictionary GlobalSettings;
        public bool Authorization { get; set; }
        public double DifferenceMagnitude { get; set; }
        public int WhirlDuration { get; set; }


        public HadenCore()
        {
            // Initialize with global settings from the config file.
            GlobalSettings = new SettingsDictionary();
            LoadSettings();
            Authorization = Convert.ToBoolean(GlobalSettings.GrabSetting("authorization"));
            DifferenceMagnitude = Convert.ToDouble(GlobalSettings.GrabSetting("differencemagnitude"));
            WhirlDuration = Convert.ToInt32(GlobalSettings.GrabSetting("whirlduration"));
            //DesiredAttribute = GlobalSettings.GrabSetting("desiredattribute");
        }

        public void LoadSettings()
        {
            var primaryPath = Path.Combine(AppContext.BaseDirectory, "config", "Settings.xml");
            if (File.Exists(primaryPath))
            {
                GlobalSettings.LoadSettings(primaryPath);
                return;
            }

            var fallbackPath = Path.Combine(Environment.CurrentDirectory, "config", "Settings.xml");
            GlobalSettings.LoadSettings(fallbackPath);
        }
    }
}
