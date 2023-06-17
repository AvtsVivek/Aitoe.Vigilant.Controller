using System.Windows;
using System.Windows.Controls;

namespace Aitoe.Vigilant.Controller.WpfController.Infra
{
    public class OddEvenTemplateSelector : DataTemplateSelector
    {
        public OddEvenTemplateSelector()
        {

        }
        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var control = Utils.GetFirstParentForChild<Window>(container);

            if (control == null)
                return null;

            var resource = "UnknownTemplate";
            try
            {
                var i = (int)item;
                resource = i % 2 == 0 ? "EvenTemplate" : "OddTemplate";
            }
            catch { }

            return (DataTemplate)control.FindResource(resource);
        }
    }
}
