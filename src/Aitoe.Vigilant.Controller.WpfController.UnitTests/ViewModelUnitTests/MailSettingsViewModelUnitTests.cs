using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Aitoe.Vigilant.Controller.WpfController.Infra.ViewModelStateEnums;
using Aitoe.Vigilant.Controller.WpfController.ViewModel;
using AutoMapper;
using Ninject;
using Ninject.MockingKernel.Moq;
using Xunit;

namespace Aitoe.Vigilant.Controller.WpfController.UnitTests.ViewModelUnitTests
{
    public class MailSettingsViewModelUnitTests
    {
        [Fact]
        public void MailSettingsViewModelDefaultState()
        {
            var mockingKernel = new MoqMockingKernel();
            mockingKernel.Bind<MailSettingsViewModel>().ToSelf();
            var msVM = mockingKernel.Get<MailSettingsViewModel>();
            Assert.Equal(MailSettingsStateEnum.SendersEmailNotConfigured, msVM.MailSettingState);
        }

        [Theory]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "", "")]
        [InlineData(MailSettingsStateEnum.SendersPasswordsDoNotMatch, "", "1")]
        [InlineData(MailSettingsStateEnum.SendersPasswordsDoNotMatch, "1", "")]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "1", "1")]
        [InlineData(MailSettingsStateEnum.SendersPasswordsDoNotMatch, "2", "1")]
        [InlineData(MailSettingsStateEnum.SendersPasswordsDoNotMatch, "1", "2")]
        public void EmailSendersPasswordNotMatchingParameterized(MailSettingsStateEnum expectedState, string p, string p2)
        {
            var mockingKernel = new MoqMockingKernel();
            mockingKernel.Bind<MailSettingsViewModel>().ToSelf();
            var msVM = mockingKernel.Get<MailSettingsViewModel>();
            msVM.EmailSendersPassword = p;
            msVM.EmailSendersPassword2 = p2;
            Assert.Equal(expectedState, msVM.MailSettingState);
        }

        [Theory]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "", "", "", "")]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "", "1", "", "")]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "1", "", "", "")]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "1", "1", "", "")]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "2", "1", "", "")]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "1", "2", "", "")]
        [InlineData(MailSettingsStateEnum.SendersPasswordsDoNotMatch, "1", "2", "11", "")]
        [InlineData(MailSettingsStateEnum.SendersPasswordsDoNotMatch, "1", "2", "11", "22")]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "1", "2", "1", "1")]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "1", "2", "33", "33")]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "", "2", "22", "22")]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "", "", "1", "1")]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "1", "", "4", "4")]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "", "", "5", "5")]
        public void EmailSendersPasswordEditingNotMatchingParameterized(MailSettingsStateEnum expectedState, string p, string p2, string pe, string pe2)
        {
            var mockingKernel = new MoqMockingKernel();
            mockingKernel.Bind<MailSettingsViewModel>().ToSelf();
            var msVM = mockingKernel.Get<MailSettingsViewModel>();

            msVM.EmailSendersPassword = p;
            msVM.EmailSendersPassword2 = p2;

            msVM.EmailSendersPassword = pe;
            msVM.EmailSendersPassword2 = pe2;
            Assert.Equal(expectedState, msVM.MailSettingState);
        }
        [Theory]
        [InlineData(MailSettingsStateEnum.SendersEmailDetailsReadyToBeSaved, "V1@xyz.com", "1", "1", "smtp.live.com", 587, true)]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "", "1", "1", "smtp.live.com", 587, false)]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, null, "1", "1", "smtp.live.com", 587, false)]

        [InlineData(MailSettingsStateEnum.SendersPasswordsDoNotMatch, "V1@xyz.com", "", "1", "smtp.live.com", 587, false)]
        [InlineData(MailSettingsStateEnum.SendersPasswordsDoNotMatch, "V1@xyz.com", null, "1", "smtp.live.com", 587, false)]
        [InlineData(MailSettingsStateEnum.SendersPasswordsDoNotMatch, "V1@xyz.com", "1", "", "smtp.live.com", 587, false)]
        [InlineData(MailSettingsStateEnum.SendersPasswordsDoNotMatch, "V1@xyz.com", "1", null, "smtp.live.com", 587, false)]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "V1@xyz.com", "1", "1", "", 587, false)]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "V1@xyz.com", "1", "1", null, 587, false)]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "V1@xyz.com", "1", "1", "smtp.live.com", null, false)]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, "", "", "", "", 587, false)]
        [InlineData(MailSettingsStateEnum.SendersEmailNotConfigured, null, null, null, null, 587, false)]

        public void CallToConfigureSendersEmailExecuteParameterized(MailSettingsStateEnum expectedState, string email, string p, string p2, string smtpServer, int? smtpPort, bool isConfigureSendersEmailButtonEnabledExtected)
        {
            var mockingKernel = new MoqMockingKernel();

            var emailServiceMock = mockingKernel.GetMock<IEmailService>();
            emailServiceMock.Setup(e => e.IsSMTPHostSet()).Returns(false);

            mockingKernel.Bind<MailSettingsViewModel>().ToSelf();
            var msVM = mockingKernel.Get<MailSettingsViewModel>();

            msVM.SendersEmailAddress = email;
            msVM.EmailSendersPassword = p;
            msVM.EmailSendersPassword2 = p2;

            msVM.SMTPServer = smtpServer;
            msVM.SMTPPort = smtpPort;

            var bIsConfigureSendersEmailButtonEnabledActual = msVM.ConfigureSendersEmail.CanExecute(null);

            Assert.Equal<bool>(isConfigureSendersEmailButtonEnabledExtected, bIsConfigureSendersEmailButtonEnabledActual);
            Assert.Equal(expectedState, msVM.MailSettingState);
        }

        [Theory]
        [InlineData(MailSettingsStateEnum.SendersEmailDetailsSaved, "V1@xyz.com", "1", "1", "smtp.live.com", 587)]
        public void Ta(MailSettingsStateEnum expectedState, string email, string p, string p2, string smtpServer, int? smtpPort)
        {
            //http://stackoverflow.com/questions/14648863/how-do-i-mock-a-interface-with-moq-or-ninject-mocking-kernel
            //http://stackoverflow.com/a/36079559/1977871
            //http://stackoverflow.com/questions/31033849/unit-testing-automapper?rq=1#comment50245083_31033849
            //http://stackoverflow.com/questions/7287540/different-return-values-the-first-and-second-time-with-moq

            var mockingKernel = new MoqMockingKernel();

            // As of now we dont need this ISMTPHost.
            //mockingKernel.Bind<ISMTPHost>().To<DL.Entities.SMTPHost>();
            //var mapperConfig = new MapperConfiguration(cfg => {
            //    cfg.CreateMap<MailSettingsViewModel1, ISMTPHost>().ConstructUsing(x => mockingKernel.Get<ISMTPHost>());
            //});
            //mockingKernel.Bind<IMapper>().ToConstant(mapperConfig.CreateMapper());

            var emailServiceMock = mockingKernel.GetMock<IEmailService>();
            emailServiceMock.Setup(e => e.IsSMTPHostSet()).Returns(false);            

            mockingKernel.Bind<MailSettingsViewModel>().ToSelf();
            var msVM = mockingKernel.Get<MailSettingsViewModel>();
            msVM.SendersEmailAddress = email;
            msVM.EmailSendersPassword = p;
            msVM.EmailSendersPassword2 = p2;

            msVM.SMTPServer = smtpServer;
            msVM.SMTPPort = smtpPort;

            emailServiceMock.Setup(e => e.IsSMTPHostSet()).Returns(true);
            msVM.ConfigureSendersEmail.Execute(null);
            Assert.Equal(expectedState, msVM.MailSettingState);
        }

        //[Theory]
        //[InlineData(MailSettingsStateEnum.SendersEmailDetailsSaved, "V1@xyz.com", "1", "1", "smtp.live.com", 587)]
        public void TrialTest(MailSettingsStateEnum expectedState, string email, string p, string p2, string smtpServer, int? smtpPort)
        {
            
            //http://stackoverflow.com/questions/14648863/how-do-i-mock-a-interface-with-moq-or-ninject-mocking-kernel
            //http://stackoverflow.com/a/36079559/1977871
            //http://stackoverflow.com/questions/31033849/unit-testing-automapper?rq=1#comment50245083_31033849
            //http://stackoverflow.com/questions/7287540/different-return-values-the-first-and-second-time-with-moq

            var mockingKernel = new MoqMockingKernel();

            mockingKernel.Bind<MailSettingsViewModel>().ToSelf();
            var msVM = mockingKernel.Get<MailSettingsViewModel>();

            mockingKernel.Bind<ISMTPHost>().To<DL.Entities.SMTPHost>();

            var mockSMTPHost = mockingKernel.GetMock<ISMTPHost>();


            var mapperConfig = new MapperConfiguration(cfg => {
                //cfg.CreateMap<MailSettingsViewModel1, ISMTPHost>().ConstructUsing(x => mockingKernel.Get<ISMTPHost>());
                cfg.CreateMap<MailSettingsViewModel, ISMTPHost>();
            });

            var mapper = mapperConfig.CreateMapper();
            var host = mapper.Map<MailSettingsViewModel, ISMTPHost>(msVM);
            mockingKernel.Bind<IMapper>().ToConstant(mapper);

            var emailServiceMock = mockingKernel.GetMock<IEmailService>();
            emailServiceMock.Setup(e => e.IsSMTPHostSet()).Returns(false);


            msVM.SendersEmailAddress = email;
            msVM.EmailSendersPassword = p;
            msVM.EmailSendersPassword2 = p2;

            msVM.SMTPServer = smtpServer;
            msVM.SMTPPort = smtpPort;

            emailServiceMock.Setup(e => e.IsSMTPHostSet()).Returns(true);
            msVM.ConfigureSendersEmail.Execute(null);
            Assert.Equal(expectedState, msVM.MailSettingState);
        }
    }
}