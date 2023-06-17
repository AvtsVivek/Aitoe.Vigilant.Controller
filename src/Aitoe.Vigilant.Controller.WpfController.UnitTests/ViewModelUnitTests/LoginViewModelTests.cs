using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Aitoe.Vigilant.Controller.WpfController.UnitTests.Infra;
using Aitoe.Vigilant.Controller.WpfController.ViewModel;
using FakeItEasy;
using Ninject;
using Ninject.MockingKernel.FakeItEasy;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.IO.Packaging;
using System.Windows;
using Xunit;

namespace Aitoe.Vigilant.Controller.WpfController.UnitTests.ViewModelUnitTests
{
    public class LoginViewModelTests
    {       
        [WpfFact]
        public void LoginButtonCallsLoginOnAuthService()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<LoginViewModel>().ToSelf();
            var fakeAuthService = fakingKernel.Get<IAuthenticationService>();

            //PackUriHelper.Create(new Uri("reliable://0"));
            //new FrameworkElement();
            //Application.ResourceAssembly = typeof(AppWpfController).Assembly;

            var fixture = new Fixture();

            var lvm = fakingKernel.Get<LoginViewModel>();

            lvm.UserId = fixture.Create<string>();
            lvm.Password = fixture.Create<string>();
            lvm.OrderId = fixture.Create<string>();

            A.CallTo(() => fakeAuthService.IsLoginDetailSet()).Returns(false);
            lvm.Login.Execute(null);
            A.CallTo(() => fakeAuthService.Login(A<IAuthDetails>.Ignored)).MustHaveHappened();
            //Assert.True(mchVM.LoginSuccessfull);
        }

        [Theory]
        [AutoFakeData]
        public void SubmitButtonEnabledOnlyWhenAllThreeFieldsAreNotEmpty(LoginViewModel lvm)
        {
            Assert.True(lvm.Login.CanExecute(null));
        }

        [Theory]
        [AutoFakeData]
        public void SubmitButtonDisabledWhenOrderIdIsEmpty(LoginViewModel lvm)
        {
            lvm.OrderId = string.Empty;
            Assert.False(lvm.Login.CanExecute(null));
        }

        [Theory]
        [AutoFakeData]
        public void SubmitButtonDisabledWhenUserNameIsEmpty(LoginViewModel lvm)
        {
            lvm.UserId = string.Empty;
            Assert.False(lvm.Login.CanExecute(null));
        }

        [Theory]
        [AutoFakeData]
        public void SubmitButtonDisabledWhenPasswordIsEmpty(LoginViewModel lvm)
        {
            lvm.Password = string.Empty;
            Assert.False(lvm.Login.CanExecute(null));
        }

        [Theory]
        [AutoFakeData]
        public void SubmitButtonDisabledWhenOrderIdIsNull(LoginViewModel lvm)
        {
            lvm.OrderId = null;
            Assert.False(lvm.Login.CanExecute(null));
        }

        [Theory]
        [AutoFakeData]
        public void SubmitButtonDisabledWhenUserNameIsNull(LoginViewModel lvm)
        {
            lvm.UserId = null;
            Assert.False(lvm.Login.CanExecute(null));
        }

        [Theory]
        [AutoFakeData]
        public void SubmitButtonDisabledWhenPasswordIsNull(LoginViewModel lvm)
        {
            lvm.Password = null;
            Assert.False(lvm.Login.CanExecute(null));
        }
    }
}
