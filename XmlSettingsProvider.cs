﻿using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// A FileSettingProvider that stores settings in XML format.
    /// </summary>
    public class XmlSettingsProvider : FileSettingsProvider
    {
        /// <summary>
        /// Gets the provider name.
        /// </summary>
        public override string Name
        {
            get { return "XmlSettingsProvider"; }
        }

        /// <summary>
        /// Gets the property values from storage.
        /// </summary>
        /// <param name="context">The settings context.</param>
        /// <param name="collection">A collection containing the names of the settings to get.</param>
        /// <returns>A collection containing the setting values.</returns>
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            var values = new SettingsPropertyValueCollection();
            XDocument doc = null;
            try
            {
                doc = XDocument.Load(Location);
            }
            catch
            {
                doc = new XDocument(new XElement("Settings"));
            }
            var root = doc.Element("Settings") ?? new XElement("Settings");
            foreach (SettingsProperty setting in collection)
            {
                SettingsPropertyValue val = new SettingsPropertyValue(setting);
                try
                {
                    var elem = root.Element(setting.Name);
                    if (elem == null || elem.IsEmpty)
                    {
                        val.SerializedValue = setting.DefaultValue;
                    }
                    else if (setting.SerializeAs == SettingsSerializeAs.Xml)
                    {
                        val.SerializedValue = @"<?xml version=""1.0"" encoding=""utf-16""?>" + elem.FirstNode.ToString();
                    }
                    else
                    {
                        using (var reader = XmlReader.Create(new StringReader(elem.ToString())))
                        {
                            reader.MoveToContent();
                            val.SerializedValue = reader.ReadElementContentAsString();
                        }
                    }
                }
                catch
                {
                    val.SerializedValue = setting.DefaultValue;
                }
                values.Add(val);
            }
            return values;
        }

        /// <summary>
        /// Saves the settings to file.
        /// </summary>
        /// <param name="context">The settings context.</param>
        /// <param name="collection">A collection containing the setting values.</param>
        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            var dir = Path.GetDirectoryName(Location);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            using (var writer = new XmlTextWriter(Location, Encoding.UTF8))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Settings");

                foreach (SettingsPropertyValue setting in collection)
                {
                    if (setting.UsingDefaultValue)
                        continue;
                    writer.WriteStartElement(setting.Name);
                    if (setting.Property.SerializeAs == SettingsSerializeAs.Xml)
                    {
                        var rawXml = setting.SerializedValue.ToString();
                        var xmlDeclaration = new Regex(@"^<\? *xml.*\?>");
                        rawXml = xmlDeclaration.Replace(rawXml, string.Empty).Trim('\r', '\n');
                        writer.WriteRaw(rawXml);
                    }
                    else
                    {
                        writer.WriteString(setting.SerializedValue.ToString());
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Flush();
            }
        }
    }
}
