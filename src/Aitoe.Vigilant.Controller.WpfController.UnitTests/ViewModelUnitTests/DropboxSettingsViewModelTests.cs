using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Aitoe.Vigilant.Controller.WpfController.Infra;
using Aitoe.Vigilant.Controller.WpfController.UnitTests.Infra;
using Aitoe.Vigilant.Controller.WpfController.ViewModel;
using FakeItEasy;
using Ninject;
using Ninject.MockingKernel.FakeItEasy;
using System;
using System.Windows;
using Ploeh.AutoFixture;
using Xunit;
using Aitoe.Vigilant.Controller.WpfController.Infra.ViewModelStateEnums;
using Dropbox.Api.Files;

namespace Aitoe.Vigilant.Controller.WpfController.UnitTests.ViewModelUnitTests
{


    public class DropboxSettingsViewModelTests
    {
        [WpfFact]
        public void ConfigureDropboxBasicMethodCalls()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var fakeMBService = fakingKernel.Get<IMessageBoxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            A.CallTo(() => fakeDBService.GetAppMainWindow()).Returns(new System.Windows.Window());
            IntPtr? handle = new IntPtr();
            A.CallTo(() => fakeDBService.GetHandle()).Returns(handle);

            // Act
            dbVM.ConfigureDropbox.Execute(null);

            // Assert
            A.CallTo(() => fakeDBService.ShowDialog()).MustHaveHappened();
            A.CallTo(() => fakeDBService.IsLoginSuccessfull).MustHaveHappened();
            A.CallTo(() => fakeDBService.IsAccessTokenExists()).MustHaveHappened();
        }

        [WpfFact]
        public void VMCtorShouldCallIsAccessTokenExists()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            A.CallTo(() => fakeDBService.GetAppMainWindow()).Returns(new Window());
            IntPtr? handle = new IntPtr();
            A.CallTo(() => fakeDBService.GetHandle()).Returns(handle);
            A.CallTo(() => fakeDBService.IsAccessTokenExists()).Returns(true);

            // Act
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();

            // Assert
            Assert.Equal(DropboxSettingsState.DropboxConfigured, dbVM.DropboxSettingState);
            //Assert.Equal(visibility, dbVM.DropboxConfigurationMessageVisibility);
        }
        [WpfFact]
        public void VMCtorShouldCallIsAccessTokenNotExists()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            A.CallTo(() => fakeDBService.GetAppMainWindow()).Returns(new Window());
            IntPtr? handle = new IntPtr();
            A.CallTo(() => fakeDBService.GetHandle()).Returns(handle);
            A.CallTo(() => fakeDBService.IsAccessTokenExists()).Returns(false);

            // Act
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();

            // Assert
            Assert.Equal(DropboxSettingsState.DropboxNotConfigured, dbVM.DropboxSettingState);
        }



        [WpfFact]
        public void DropboxSettingsViewModelConfigureDropboxSuccessifully()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var fakeMBService = fakingKernel.Get<IMessageBoxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            A.CallTo(() => fakeDBService.GetAppMainWindow()).Returns(new Window());
            IntPtr? handle = new IntPtr();
            A.CallTo(() => fakeDBService.GetHandle()).Returns(handle);
            A.CallTo(() => fakeDBService.IsLoginSuccessfull).Returns(true);

            // Act
            dbVM.ConfigureDropbox.Execute(null);

            //
            A.CallTo(() => fakeDBService.ShowDialog()).MustHaveHappened();
            A.CallTo(() => fakeDBService.SetAccessToken()).MustHaveHappened();
            A.CallTo(() => fakeMBService.Show(A<string>.Ignored, A<string>.Ignored,
                A<MessageBoxButton>.Ignored, A<MessageBoxImage>.Ignored,
                A<MessageBoxResult>.Ignored)).MustNotHaveHappened();
        }
        [WpfFact]
        public void DropboxSettingsViewModelConfigureDropboxUnsuccessifully()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var fakeMBService = fakingKernel.Get<IMessageBoxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            A.CallTo(() => fakeDBService.GetAppMainWindow()).Returns(new Window());
            IntPtr? handle = new IntPtr();
            A.CallTo(() => fakeDBService.GetHandle()).Returns(handle);
            A.CallTo(() => fakeDBService.IsLoginSuccessfull).Returns(false);

            // Act
            dbVM.ConfigureDropbox.Execute(null);

            //
            A.CallTo(() => fakeDBService.ShowDialog()).MustHaveHappened();
            A.CallTo(() => fakeDBService.SetAccessToken()).MustNotHaveHappened();
            A.CallTo(() => fakeMBService.Show(A<string>.Ignored, A<string>.Ignored, 
                A<MessageBoxButton>.Ignored, A<MessageBoxImage>.Ignored, 
                A<MessageBoxResult>.Ignored)).MustHaveHappened();
        }
        [WpfFact]
        public void CreateDropboxFolderWillCallIsAccessTokenExists()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            var fixture = new Fixture();
            var somePath = fixture.Create<string>();
            //A.CallTo(() => dbVM.CreateFolderPath).Returns(somePath);
            dbVM.CreateFolderPath = somePath;

            // Act
            dbVM.CreateDropboxFolder.Execute(null);

            //Assert
            A.CallTo(() => fakeDBService.IsAccessTokenExists()).MustHaveHappened();
        }

        [WpfFact]
        public void CreateDropboxFolderWillNotCallCreateDropBoxFolder()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            A.CallTo(() => fakeDBService.IsAccessTokenExists()).Returns(false);

            // Act
            dbVM.CreateDropboxFolder.Execute(null);

            //Assert
            A.CallTo(() => fakeDBService.CreateDropBoxFolder(A<string>.Ignored)).MustNotHaveHappened();
        }

        [WpfFact]
        public void CreateDropboxFolderWillCallCreateDropBoxFolder()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            var fixture = new Fixture();
            var somePath = fixture.Create<string>();
            dbVM.CreateFolderPath = somePath;
            A.CallTo(() => fakeDBService.IsAccessTokenExists()).Returns(true);

            // Act
            dbVM.CreateDropboxFolder.Execute(null);

            //Assert
            A.CallTo(() => fakeDBService.CreateDropBoxFolder(somePath)).MustHaveHappened();
        }

        [WpfFact]
        public void CreateDropboxFolderWillCallCreateDropBoxFolderAndFolderCreatedSuccessfully()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            var fixture = new Fixture();
            var somePath = fixture.Create<string>();
            dbVM.CreateFolderPath = somePath;
            A.CallTo(() => fakeDBService.IsAccessTokenExists()).Returns(true);
            A.CallTo(() => fakeDBService.CreateDropBoxFolder(somePath))
                .Returns(true);

            // Act
            dbVM.CreateDropboxFolder.Execute(null);

            //Assert
            Assert.Equal(DropboxSettingsState.FolderCreatedSuccessfully, dbVM.DropboxSettingState);
        }

        [WpfFact]
        public void CreateDropboxFolderWillCallCreateDropBoxFolderAndFolderCreateFailure()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            var fixture = new Fixture();
            var somePath = fixture.Create<string>();
            dbVM.CreateFolderPath = somePath;
            A.CallTo(() => fakeDBService.IsAccessTokenExists()).Returns(true);
            A.CallTo(() => fakeDBService.CreateDropBoxFolder(somePath))
                .Returns(false);

            // Act
            dbVM.CreateDropboxFolder.Execute(null);

            //Assert
            Assert.Equal(DropboxSettingsState.FolderCreateFailure, dbVM.DropboxSettingState);
        }


        [WpfFact]
        public void DeleteDropboxFolderWillCallIsAccessTokenExists()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            var fixture = new Fixture();
            var somePath = fixture.Create<string>();
            //A.CallTo(() => dbVM.CreateFolderPath).Returns(somePath);
            dbVM.CreateFolderPath = somePath;

            // Act
            dbVM.DeleteDropboxFolder.Execute(null);

            //Assert
            A.CallTo(() => fakeDBService.IsAccessTokenExists()).MustHaveHappened();
        }

        [WpfFact]
        public void DeleteDropboxFolderWillNotCallDeleteDropBoxFolder()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            A.CallTo(() => fakeDBService.IsAccessTokenExists()).Returns(false);

            // Act
            dbVM.DeleteDropboxFolder.Execute(null);

            //Assert
            A.CallTo(() => fakeDBService.DeleteDropBoxFolder(A<string>.Ignored)).MustNotHaveHappened();
        }

        [WpfFact]
        public void DeleteDropboxFolderWillNotCallDeleteDropBoxFolderIfFolderDoesNotExist()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            A.CallTo(() => fakeDBService.IsAccessTokenExists()).Returns(true).NumberOfTimes(4);
            A.CallTo(() => fakeDBService.IsFolderExists(A<string>.Ignored)).Returns(false);
            
            // Act
            dbVM.DeleteDropboxFolder.Execute(null);

            //Assert
            A.CallTo(() => fakeDBService.DeleteDropBoxFolder(A<string>.Ignored)).MustNotHaveHappened();
        }


        [WpfFact]
        public void DeleteDropboxFolderWillCallDeleteDropBoxFolder()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            var fixture = new Fixture();
            var somePath = fixture.Create<string>();
            dbVM.DeleteFolderPath = somePath;
            A.CallTo(() => fakeDBService.IsAccessTokenExists()).Returns(true);
            A.CallTo(() => fakeDBService.IsFolderExists(A<string>.Ignored)).Returns(true);

            // Act
            dbVM.DeleteDropboxFolder.Execute(null);

            //Assert
            A.CallTo(() => fakeDBService.DeleteDropBoxFolder(somePath)).MustHaveHappened();
        }

        [WpfFact]
        public void DeleteDropboxFolderWillCallDeleteDropBoxFolderAndRetursTrue()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            var fixture = new Fixture();
            var somePath = fixture.Create<string>();
            dbVM.DeleteFolderPath = somePath;
            A.CallTo(() => fakeDBService.IsAccessTokenExists()).Returns(true);
            A.CallTo(() => fakeDBService.IsFolderExists(A<string>.Ignored)).Returns(true);
            A.CallTo(() => fakeDBService.DeleteDropBoxFolder(somePath)).Returns(true);
            // Act
            dbVM.DeleteDropboxFolder.Execute(null);

            //Assert
            Assert.Equal(DropboxSettingsState.FolderDeletedSuccessfully, dbVM.DropboxSettingState);
        }

        [WpfFact]
        public void DeleteDropboxFolderWillCallDeleteDropBoxFolderAndRetursFalse()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            var fixture = new Fixture();
            var somePath = fixture.Create<string>();
            dbVM.DeleteFolderPath = somePath;
            A.CallTo(() => fakeDBService.IsAccessTokenExists()).Returns(true);
            A.CallTo(() => fakeDBService.IsFolderExists(A<string>.Ignored)).Returns(true);
            A.CallTo(() => fakeDBService.DeleteDropBoxFolder(somePath)).Returns(false);
            // Act
            dbVM.DeleteDropboxFolder.Execute(null);

            //Assert
            Assert.Equal(DropboxSettingsState.FolderDeleteFailure, dbVM.DropboxSettingState);
        }
        [WpfFact]
        public void DeleteDropboxButtonDisabledWhenNoFileNameIsEntered()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            dbVM.DeleteFolderPath = String.Empty;

            Assert.False(dbVM.DeleteDropboxFolder.CanExecute(null));
        }
        [WpfFact]
        public void DeleteDropboxButtonEnabledOnlyWhenSomeFileNameIsEntered()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            var fixture = new Fixture();
            dbVM.DeleteFolderPath = fixture.Create<string>();

            Assert.True(dbVM.DeleteDropboxFolder.CanExecute(null));
        }

        [WpfFact]
        public void CreateDropboxButtonDisabledWhenNoFileNameIsEntered()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            dbVM.CreateFolderPath = String.Empty;

            Assert.False(dbVM.CreateDropboxFolder.CanExecute(null));
        }
        [WpfFact]
        public void CreateDropboxButtonEnabledOnlyWhenSomeFileNameIsEntered()
        {
            var fakingKernel = new FakeItEasyMockingKernel();
            fakingKernel.Bind<DropboxSettingsViewModel>().ToSelf();
            var fakeDBService = fakingKernel.Get<IDropboxService>();
            var dbVM = fakingKernel.Get<DropboxSettingsViewModel>();
            var fixture = new Fixture();
            dbVM.CreateFolderPath = fixture.Create<string>();

            Assert.True(dbVM.CreateDropboxFolder.CanExecute(null));
        }

    }
}