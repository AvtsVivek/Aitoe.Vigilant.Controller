using System;
using Aitoe.Vigilant.Controller.BL.Ccc;
using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.RepositoryInterfaces;
using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Aitoe.Vigilant.Controller.DL.Entities;
using Aitoe.Vigilant.Controller.DL.Repositories;
using Aitoe.Vigilant.Controller.DL.Services;
using Ninject.Modules;

namespace Aitoe.Vigilant.Controller.WpfHo
{
    public class CLPHoNInjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISMTPHost>().To<SMTPHost>();
            Bind<IEmailService>().To<ExceptionHandlerEmailService>();
            Bind<IEmailService>().To<LoggerEmailService>().WhenInjectedInto<ExceptionHandlerEmailService>();
            Bind<IEmailService>().To<EmailService>().WhenInjectedInto<LoggerEmailService>();

            Bind<IEmail>().To<EmailMessage>();
            
            Bind<IPushbulletService>().To<ExceptionHandlerPushbulletService>();
            Bind<IPushbulletService>().To<LoggerPushbulletService>().WhenInjectedInto<ExceptionHandlerPushbulletService>();
            Bind<IPushbulletService>().To<PushbulletService>().WhenInjectedInto<LoggerPushbulletService>();

            Bind<IDropboxService>().To<ExceptionHandlerDropboxService>().InSingletonScope();
            Bind<IDropboxService>().To<LoggerDropboxService>().WhenInjectedInto<ExceptionHandlerDropboxService>();
            Bind<IDropboxService>().To<DropboxService>().WhenInjectedInto<LoggerDropboxService>();

        }
    }
}
