using log4net;
using Ninject.Activation;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.CLP
{
    public class CLPNInjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILog>().ToMethod(GetLogger);
        }
        private ILog GetLogger(IContext ctx)
        {
            return LogManager.GetLogger(ctx.Request.Target.Member.ReflectedType);
        }
    }
}
