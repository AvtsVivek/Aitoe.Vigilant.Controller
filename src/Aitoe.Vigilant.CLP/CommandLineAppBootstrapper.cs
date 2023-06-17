using Aitoe.Vigilant.CLP.DTO;
using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.WpfHo;
using AutoMapper;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.CLP
{
    public class CommandLineAppBootstrapper
    {
        private readonly StandardKernel NInjectKernel;
        public CommandLineAppBootstrapper()
        {

            var clpModule = new CLPNInjectModule();
            var clpHOModule = new CLPHoNInjectModule();
            NInjectKernel = new StandardKernel(clpHOModule, clpModule);

            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.CreateMap<EmailDTO, IEmail>().ConstructUsing(x => NInjectKernel.Get<IEmail>());
                cfg.CreateMap<PushbulletDTO, IEmail>().ConstructUsing(x => NInjectKernel.Get<IEmail>());
            });

            NInjectKernel.Bind<IMapper>().ToConstant(mapperConfig.CreateMapper());
        }

        public void Start(string[] args)
        {
            var a = NInjectKernel.Get<AitoeVigilantAlertCommandProcessor>();
            a.Process(args, Console.In, Console.Out, Console.Error);
        }
    }
}
