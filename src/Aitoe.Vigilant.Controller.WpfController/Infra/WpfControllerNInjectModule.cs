using System;
using Aitoe.Vigilant.Controller.WpfController.ViewModel;
using log4net;
using Ninject.Activation;
using Ninject.Modules;
using log4net.Repository.Hierarchy;

namespace Aitoe.Vigilant.Controller.WpfController.Infra
{
    public class WpfControllerNInjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILog>().ToMethod(GetLogger);
            Bind<MultiControllerHomeViewModel>().   ToSelf().InSingletonScope();
            Bind<VigilantGridViewModel>().          ToSelf().InSingletonScope();
            Bind<MailSettingsViewModel>().         ToSelf().InSingletonScope();
            Bind<PushbulletSettingsViewModel>().    ToSelf().InSingletonScope();
            Bind<DropboxSettingsViewModel>().ToSelf().InSingletonScope();
        }

        private ILog GetLogger(IContext ctx)
        {
            return LogManager.GetLogger(ctx.Request.Target.Member.ReflectedType);
        }
    }
}
