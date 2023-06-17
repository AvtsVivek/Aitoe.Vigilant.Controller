using System;
using System.Configuration;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Aitoe.Vigilant.Controller.WpfController.Infra
{
    public static class Utils
    {
        public static string GetConfigLocation(int configParam)
        {
            ConfigurationUserLevel configLevel = ConfigurationUserLevel.None;
            switch (configParam)
            {
                case 1:
                    configLevel = ConfigurationUserLevel.None;
                    break;
                case 2:
                    configLevel = ConfigurationUserLevel.PerUserRoaming;
                    break;
                case 3:
                    configLevel = ConfigurationUserLevel.PerUserRoamingAndLocal;
                    break;
            }
            var configPath = GetDefaultExeConfigPath(configLevel);
            return configPath;
        }
        private static string GetDefaultExeConfigPath(ConfigurationUserLevel userLevel)
        {
            try
            {
                var UserConfig = ConfigurationManager.OpenExeConfiguration(userLevel);
                return UserConfig.FilePath;
            }
            catch (ConfigurationException e)
            {
                return e.Filename;
            }
        }
        public static T GetFirstParentForChild<T>(DependencyObject child) where T : class
        {
            if (child == null) { return null; }

            var candidate = child as T;
            return candidate ?? GetFirstParentForChild<T>(VisualTreeHelper.GetParent(child));
        }

        public static void RegisterDataTemplate(Type viewModelType, Type viewType)
        {
            var template = CreateTemplate(viewModelType, viewType);

            var key = template.DataTemplateKey;
            Application.Current.Resources.Add(key, template);
        }

        private static DataTemplate CreateTemplate(Type viewModelType, Type viewType)
        {
            //const string xamlTemplate = "<DataTemplate DataType=\"{{x:Type vm:{0}}}\"><v:{1} /><Label Content=\"Hare\" /></DataTemplate>";
            const string xamlTemplate = "<DataTemplate DataType=\"{{x:Type vm:{0}}}\"><v:{1} /></DataTemplate>";
            var xaml = String.Format(xamlTemplate, viewModelType.Name, viewType.Name, viewModelType.Namespace, viewType.Namespace);

            var context = new ParserContext();

            context.XamlTypeMapper = new XamlTypeMapper(new string[0]);
            context.XamlTypeMapper.AddMappingProcessingInstruction("vm", viewModelType.Namespace, viewModelType.Assembly.FullName);
            context.XamlTypeMapper.AddMappingProcessingInstruction("v", viewType.Namespace, viewType.Assembly.FullName);

            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("vm", "vm");
            context.XmlnsDictionary.Add("v", "v");

            var template = (DataTemplate)XamlReader.Parse(xaml, context);
            return template;
        }
    }
}
