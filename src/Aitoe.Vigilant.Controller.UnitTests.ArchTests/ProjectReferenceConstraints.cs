using Aitoe.Vigilant.Controller.DL.Infra;
using Aitoe.Vigilant.Controller.WpfController;
using System;
using System.Reflection;
using Xunit;
using System.Linq;
namespace Aitoe.Vigilant.Controller.UnitTests.ArchTests
{
    public class ProjectReferenceConstraints
    {
        [Fact]
        public void PresentationShouldNotReferenceDataLayer()
        {
            var presentationRepresentative = typeof(AppWpfController);
            var dataLayerRepresentative = typeof(DlRepresentative);
            var references = presentationRepresentative.Assembly.GetReferencedAssemblies();
            var dataLayerAssemblyName = dataLayerRepresentative.Assembly.GetName();
            var presentationAssemblyName = presentationRepresentative.Assembly.GetName().Name;
            Assert.False(references.Any(a => AssemblyName.ReferenceMatchesDefinition(dataLayerAssemblyName, a)), string.Format("{0} should not be referenced by {1}", dataLayerAssemblyName.Name, presentationAssemblyName));
        }
    }
}
