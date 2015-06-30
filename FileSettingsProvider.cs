using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// An abstract SettingsProvider class that allows you to control where the settings file is saved.
    /// </summary>
    public abstract class FileSettingsProvider : SettingsProvider
    {
        private string applicationName;
        private string location;

        /// <summary>
        /// Creates a new <see cref="FileSettingsProvider"/> instance.
        /// </summary>
        public FileSettingsProvider()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            applicationName = entryAssembly.GetName().Name;
        }

        /// <summary>
        /// Gets or sets the name of the currently running application.
        /// </summary>
        public override string ApplicationName
        {
            get { return applicationName; }
            set { }
        }

        /// <summary>
        /// Gets or sets the settings storage location.
        /// </summary>
        public virtual string Location
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
        
        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="name">The firendly name of the provider.</param>
        /// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name ?? Name ?? "FileSettingsProvider", config);
            if (config == null)
                return;

            string appName = config["ApplicationName"];
            if (appName != null)
                applicationName = appName;

            string location = config["Location"];
            if (location != null)
                Location = location;
        }
    }
}
