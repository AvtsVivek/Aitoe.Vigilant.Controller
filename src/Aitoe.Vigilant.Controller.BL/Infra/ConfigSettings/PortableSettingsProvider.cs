﻿using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
//using System.Windows.Forms;
using System.Collections.Specialized;
using System.Xml;
using System.IO;

namespace Aitoe.Vigilant.Controller.BL.Infra.ConfigSettings
{
    public sealed class PortableSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        private const string _rootNodeName = "settings";
        private const string _localSettingsNodeName = "localSettings";
        private const string _globalSettingsNodeName = "globalSettings";
        private const string _className = "PortableSettingsProvider";
        private XmlDocument _xmlDocument;

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(Name, config);
        }

        public override string ApplicationName
        {
            get
            {
                return "aiSentinelMultiController";
                //return Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            }
            //get { return (System.Reflection.Assembly.GetExecutingAssembly().GetName().Name); }
            set { }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();

            foreach (SettingsProperty property in collection)
            {
                //SettingsPropertyValue value = new SettingsPropertyValue(property);
                //value.IsDirty = false;
                ////value.PropertyValue = string.Empty;
                //values.Add(value);

                values.Add(new SettingsPropertyValue(property)
                {
                    SerializedValue = GetValue(property)
                });
            }

            return values;
        }
        private string _filePath
        {
            get
            {
                var folderPath = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData), "aiSentinel");
                var filepath = Path.Combine(folderPath, string.Format("{0}.settings", ApplicationName));

                //return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),
                // string.Format("{0}.settings", ApplicationName));
                return filepath;
            }
        }

        private XmlNode _localSettingsNode
        {
            get
            {
                XmlNode settingsNode = GetSettingsNode(_localSettingsNodeName);
                XmlNode machineNode = settingsNode.SelectSingleNode(Environment.MachineName.ToLowerInvariant());

                if (machineNode == null)
                {
                    machineNode = _rootDocument.CreateElement(Environment.MachineName.ToLowerInvariant());
                    settingsNode.AppendChild(machineNode);
                }

                return machineNode;
            }
        }

        private XmlNode _globalSettingsNode
        {
            get { return GetSettingsNode(_globalSettingsNodeName); }
        }

        private XmlNode _rootNode
        {
            get { return _rootDocument.SelectSingleNode(_rootNodeName); }
        }

        private XmlDocument _rootDocument
        {
            get
            {
                if (_xmlDocument == null)
                {
                    try
                    {
                        _xmlDocument = new XmlDocument();
                        _xmlDocument.Load(_filePath);
                    }
                    catch (Exception)
                    {

                    }

                    if (_xmlDocument.SelectSingleNode(_rootNodeName) != null)
                        return _xmlDocument;

                    _xmlDocument = GetBlankXmlDocument();
                }

                return _xmlDocument;
            }
        }


        public override string Name
        {
            get { return _className; }
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            foreach (SettingsPropertyValue propertyValue in collection)
                SetValue(propertyValue);

            try
            {
                _rootDocument.Save(_filePath);
            }
            catch (Exception)
            {
                /* 
                 * If this is a portable application and the device has been 
                 * removed then this will fail, so don't do anything. It's 
                 * probably better for the application to stop saving settings 
                 * rather than just crashing outright. Probably.
                 */
            }
        }



        private void SetValue(SettingsPropertyValue propertyValue)
        {
            XmlNode targetNode = IsGlobal(propertyValue.Property)
               ? _globalSettingsNode
               : _localSettingsNode;

            XmlNode settingNode = targetNode.SelectSingleNode(string.Format("setting[@name='{0}']", propertyValue.Name));

            if (settingNode != null)
                settingNode.InnerText = propertyValue.SerializedValue.ToString();
            else
            {
                settingNode = _rootDocument.CreateElement("setting");

                XmlAttribute nameAttribute = _rootDocument.CreateAttribute("name");
                nameAttribute.Value = propertyValue.Name;

                settingNode.Attributes.Append(nameAttribute);
                settingNode.InnerText = propertyValue.SerializedValue.ToString();

                targetNode.AppendChild(settingNode);
            }
        }

        private string GetValue(SettingsProperty property)
        {
            XmlNode targetNode = IsGlobal(property) ? _globalSettingsNode : _localSettingsNode;
            XmlNode settingNode = targetNode.SelectSingleNode(string.Format("setting[@name='{0}']", property.Name));

            if (settingNode == null)
                return property.DefaultValue != null ? property.DefaultValue.ToString() : string.Empty;

            return settingNode.InnerText;
        }

        private bool IsGlobal(SettingsProperty property)
        {
            foreach (DictionaryEntry attribute in property.Attributes)
            {
                if ((Attribute)attribute.Value is SettingsManageabilityAttribute)
                    return true;
            }

            return false;
        }

        private XmlNode GetSettingsNode(string name)
        {
            XmlNode settingsNode = _rootNode.SelectSingleNode(name);

            if (settingsNode == null)
            {
                settingsNode = _rootDocument.CreateElement(name);
                _rootNode.AppendChild(settingsNode);
            }

            return settingsNode;
        }

        public XmlDocument GetBlankXmlDocument()
        {
            XmlDocument blankXmlDocument = new XmlDocument();
            blankXmlDocument.AppendChild(blankXmlDocument.CreateXmlDeclaration("1.0", "utf-8", string.Empty));
            blankXmlDocument.AppendChild(blankXmlDocument.CreateElement(_rootNodeName));

            return blankXmlDocument;
        }

        public void Reset(SettingsContext context)
        {
            _localSettingsNode.RemoveAll();
            _globalSettingsNode.RemoveAll();

            _xmlDocument.Save(_filePath);
        }

        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
        {
            // do nothing
            return new SettingsPropertyValue(property);
        }

        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        {
        }
    }
}
