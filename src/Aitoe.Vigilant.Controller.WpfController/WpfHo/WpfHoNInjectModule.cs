using Aitoe.Vigilant.Controller.BL.Ccc;
using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.RepositoryInterfaces;
using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Aitoe.Vigilant.Controller.DL.Entities;
using Aitoe.Vigilant.Controller.DL.Repositories;
using Aitoe.Vigilant.Controller.DL.Services;
using Ninject.Modules;

namespace Aitoe.Vigilant.Controller.WpfHo.WpfHo
{
    public class WpfHoNInjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICamProcRepository>().To<ExceptionHandlerCamProcRepository>().InSingletonScope();
            Bind<ICamProcRepository>().To<LoggerCamProcRepository>().WhenInjectedInto<ExceptionHandlerCamProcRepository>();
            Bind<ICamProcRepository>().To<CamProcRepository>().WhenInjectedInto<LoggerCamProcRepository>();

            Bind<IAitoeRedCell>().To<LoggerAitoeRedCell>();
            Bind<IAitoeRedCell>().To<AitoeRedCell>().WhenInjectedInto<LoggerAitoeRedCell>();
           
            Bind<ISMTPHost>().To<SMTPHost>();

            Bind<IEmailService>().To<ExceptionHandlerEmailService>().InSingletonScope();
            Bind<IEmailService>().To<LoggerEmailService>().WhenInjectedInto<ExceptionHandlerEmailService>();
            Bind<IEmailService>().To<EmailService>().WhenInjectedInto<LoggerEmailService>();

            Bind<IEmail>().To<EmailMessage>();

            Bind<IPushbulletService>().To<ExceptionHandlerPushbulletService>().InSingletonScope();
            Bind<IPushbulletService>().To<LoggerPushbulletService>().WhenInjectedInto<ExceptionHandlerPushbulletService>();
            Bind<IPushbulletService>().To<PushbulletService>().WhenInjectedInto<LoggerPushbulletService>();
            
            Bind<IDropboxService>().To<ExceptionHandlerDropboxService>().InSingletonScope();
            Bind<IDropboxService>().To<LoggerDropboxService>().WhenInjectedInto<ExceptionHandlerDropboxService>();
            Bind<IDropboxService>().To<DropboxService>().WhenInjectedInto<LoggerDropboxService>();

            Bind<IAuthenticationService>().To<ExceptionHandlerAuthenticationService>().InSingletonScope();
            Bind<IAuthenticationService>().To<LoggerAuthenticationService>().WhenInjectedInto<ExceptionHandlerAuthenticationService>();
            Bind<IAuthenticationService>().To<AuthenticationService>().WhenInjectedInto<LoggerAuthenticationService>();           

            Bind<IAuthDetails>().To<AuthDetails>();

        }
    }
}
