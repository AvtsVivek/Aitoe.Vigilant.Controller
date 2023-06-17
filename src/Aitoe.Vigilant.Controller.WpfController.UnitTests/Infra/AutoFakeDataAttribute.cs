using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Ploeh.AutoFixture.AutoFakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.WpfController.UnitTests.Infra
{
    public class AutoFakeDataAttribute : AutoDataAttribute
    {
        public AutoFakeDataAttribute() : base(new Fixture().Customize(new AutoFakeItEasyCustomization()))
        {

        }
    }

}
