using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Aitoe.Vigilant.Controller.WpfController.Infra.ViewModelStateEnums;
using Aitoe.Vigilant.Controller.WpfController.UnitTests.Infra;
using Aitoe.Vigilant.Controller.WpfController.ViewModel;
using FakeItEasy;
using Ninject;
using Ninject.MockingKernel.FakeItEasy;
using Ploeh.AutoFixture;
using System;
using System.IO.Packaging;
using System.Windows;
using Xunit;

namespace Aitoe.Vigilant.Controller.WpfController.UnitTests.ViewModelUnitTests
{
    public class MultiControllerHomeViewModelTests
    {
        [WpfFact]
        public void LoginSuccess()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            var fakeAuthService = fakingKernel.Get<IAuthenticationService>();

            PackUriHelper.Create(new Uri("reliable://0"));
            new FrameworkElement();
            Application.ResourceAssembly = typeof(AppWpfController).Assembly;

            var fixture = new Fixture();
            var iCams = fixture.Create<int>();

            A.CallTo(() => fakeAuthService.IsLoginDetailSet()).Returns(true);
            A.CallTo(() => fakeAuthService.GetCamerasLicensed()).Returns(iCams);
            var mchvm = fakingKernel.Get<MultiControllerHomeViewModel>();
            var mcvm = fakingKernel.Get<MultiControllerViewModel>();
            var lvm = fakingKernel.Get<LoginViewModel>();

            lvm.LoginViewLoaded.Execute(null);
            mcvm.ViewLoaded.Execute(null);
            Assert.True(mchvm.LoginSuccessfull);
            Assert.IsType<MultiControllerViewModel>(mchvm.CurrentPageViewModel);
            Assert.Equal(mcvm.CamerasLicensed, iCams);
            Assert.Equal(lvm.CurrentLoginState, LoginState.LoginDetailsSet);
        }


        [WpfFact]
        public void LoginSuccessStateOfLoginViewModel()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            var fakeAuthService = fakingKernel.Get<IAuthenticationService>();

            PackUriHelper.Create(new Uri("reliable://0"));
            new FrameworkElement();
            Application.ResourceAssembly = typeof(AppWpfController).Assembly;

            var fixture = new Fixture();
            var iCams = fixture.Create<int>();

            A.CallTo(() => fakeAuthService.IsLoginDetailSet()).Returns(true);
            var mchvm = fakingKernel.Get<MultiControllerHomeViewModel>();
            var lvm = fakingKernel.Get<LoginViewModel>();

            lvm.UserId = fixture.Create<string>();
            lvm.Password = fixture.Create<string>();
            lvm.OrderId = fixture.Create<string>();

            lvm.LoginViewLoaded.Execute(null);
            Assert.Equal(lvm.CurrentLoginState, LoginState.LoginDetailsSet);
            Assert.True(lvm.Logout.CanExecute(null));
            Assert.False(lvm.Login.CanExecute(null));
        }

        [WpfFact]
        public void LoginFailureStateOfLoginViewModel()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            var fakeAuthService = fakingKernel.Get<IAuthenticationService>();

            PackUriHelper.Create(new Uri("reliable://0"));
            new FrameworkElement();
            Application.ResourceAssembly = typeof(AppWpfController).Assembly;

            var fixture = new Fixture();
            var iCams = fixture.Create<int>();

            A.CallTo(() => fakeAuthService.IsLoginDetailSet()).Returns(false);
            var mchvm = fakingKernel.Get<MultiControllerHomeViewModel>();
            var lvm = fakingKernel.Get<LoginViewModel>();

            lvm.UserId = fixture.Create<string>();
            lvm.Password = fixture.Create<string>();
            lvm.OrderId = fixture.Create<string>();

            lvm.LoginViewLoaded.Execute(null);
            Assert.Equal(lvm.CurrentLoginState, LoginState.InitialMessage);
            Assert.False(lvm.Logout.CanExecute(null));
            Assert.True(lvm.Login.CanExecute(null));
        }


        [WpfFact]
        public void LoginFailure()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<MultiControllerHomeViewModel>().ToSelf();
            fakingKernel.Bind<MultiControllerViewModel>().ToSelf();
            var fakeAuthService = fakingKernel.Get<IAuthenticationService>();

            PackUriHelper.Create(new Uri("reliable://0"));
            new FrameworkElement();
            Application.ResourceAssembly = typeof(AppWpfController).Assembly;

            var fixture = new Fixture();
            var iCams = fixture.Create<int>();

            A.CallTo(() => fakeAuthService.IsLoginDetailSet()).Returns(false);
            A.CallTo(() => fakeAuthService.GetCamerasLicensed()).Returns(iCams);
            var mchvm = fakingKernel.Get<MultiControllerHomeViewModel>();
            var mcvm = fakingKernel.Get<MultiControllerViewModel>();
            var lvm = fakingKernel.Get<LoginViewModel>();

            lvm.LoginViewLoaded.Execute(null);
            mcvm.ViewLoaded.Execute(null);
            Assert.False(mchvm.LoginSuccessfull);
            Assert.IsType<LoginViewModel>(mchvm.CurrentPageViewModel);
            Assert.Equal(lvm.CurrentLoginState, LoginState.InitialMessage);
        }
    }
}
