using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WpfHelpers
{
    public abstract class FileSettingsProvider : SettingsProvider
    {
        protected Dictionary<string, string> ReplaceTokens { get; private set; }
        private string defaultAppName;
        private string location;

        public FileSettingsProvider()
        {
            ReplaceTokens = new Dictionary<string, string>();
            var entryAssembly = Assembly.GetEntryAssembly();
            defaultAppName = entryAssembly.GetName().Name;
            ApplicationName = defaultAppName;
        }

        public override sealed string ApplicationName
        {
            get
            {
                string appName;
                if (!ReplaceTokens.TryGetValue("AppName", out appName))
                {
                    ReplaceTokens.Add("AppName", defaultAppName);
                    appName = defaultAppName;
                }
                return appName;
            }
            set
            {
                if (ReplaceTokens.ContainsKey("AppName"))
                    ReplaceTokens["AppName"] = defaultAppName;
                else
                    ReplaceTokens.Add("AppName", defaultAppName);
            }
        }

        /// <summary>
        /// Gets or sets the settings storage location.
        /// </summary>
        public string Location
        {
            get
            {
                if (location != null)
                    return location;
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationName, "settings.xml");
            }
            set
            {
                location = value;
            }
        }
        
        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(ApplicationName, config);
            if (config == null)
                return;
            string location = config["location"];
            if (location != null)
            {
                Regex regex = new Regex(@"{(\w+)}");
                var matches = regex.Replace(location, m =>
                {
                    var key = m.Groups[1].Value;
                    Environment.SpecialFolder specialFolder;
                    if (Enum.TryParse(key, true, out specialFolder))
                    {
                        return Environment.GetFolderPath(specialFolder);
                    }
                    string replacement;
                    if (ReplaceTokens.TryGetValue(key, out replacement))
                        return replacement;
                    return m.Value;
                });
                Location = location;
            }
        }
    }
}
