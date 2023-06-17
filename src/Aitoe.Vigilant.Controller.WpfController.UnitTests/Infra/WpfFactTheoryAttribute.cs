using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Aitoe.Vigilant.Controller.WpfController.UnitTests.Infra
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("Aitoe.Vigilant.Controller.WpfController.UnitTests.Infra.WpfFactDiscoverer", "Aitoe.Vigilant.Controller.WpfController.UnitTests")]
    public class WpfFactAttribute : FactAttribute { }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("Aitoe.Vigilant.Controller.WpfController.UnitTests.Infra.WpfTheoryDiscoverer", "Aitoe.Vigilant.Controller.WpfController.UnitTests")]
    public class WpfTheoryAttribute : TheoryAttribute { }
}
