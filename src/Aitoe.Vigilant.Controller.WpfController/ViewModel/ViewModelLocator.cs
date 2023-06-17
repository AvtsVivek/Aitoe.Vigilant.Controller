/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Aitoe.Vigilant.Controller.WpfController"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/


using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.WpfController.Infra;
using Aitoe.Vigilant.Controller.WpfHo.WpfHo;
using AutoMapper;
using GalaSoft.MvvmLight.Messaging;
using Ninject;
using System.Windows;
using System;
using System.Linq;

using log4net;
using log4net.Repository.Hierarchy;
using log4net.Core;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Views;

namespace Aitoe.Vigilant.Controller.WpfController.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private readonly StandardKernel NInjectKernel;
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {           

            var wpfModule = new WpfControllerNInjectModule();
            var wpfHoModule = new WpfHoNInjectModule();
            NInjectKernel = new StandardKernel(wpfModule, wpfHoModule);

            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.CreateMap<IAitoeRedCell, VigilantSingleProcessViewModel>()
                    .ForMember(dest => dest.AitoeRedProcessId, opts => opts.MapFrom(src => src.AitoeRedProcess == null ? 0 : src.AitoeRedProcess.Id))
                    .ForMember(dest => dest.AitoeRedProcessMainWindowHandle, opts => opts.MapFrom(src => src.AitoeRedProcess == null ? IntPtr.Zero : src.AitoeRedProcess.MainWindowHandle));
                cfg.CreateMap<VigilantSingleProcessViewModel, IAitoeRedCell>().ConstructUsing(x => NInjectKernel.Get<IAitoeRedCell>());
                cfg.CreateMap<MailSettingsViewModel, ISMTPHost>().ConstructUsing(x => NInjectKernel.Get<ISMTPHost>());
                cfg.CreateMap<ISMTPHost, MailSettingsViewModel>()
                    .ForMember(dest => dest.SendersEmailAddress, opts => opts.MapFrom(src => src.SendersEmailAddress))
                    .ForMember(dest => dest.EmailSendersPassword, opts => opts.MapFrom(src => src.EmailSendersPassword))
                    .ForMember(dest => dest.EmailSendersPassword2, opts => opts.MapFrom(src => src.EmailSendersPassword))
                    .ForMember(dest => dest.SMTPServer, opts => opts.MapFrom(src => src.SMTPServer))
                    .ForMember(dest => dest.SMTPPort, opts => opts.MapFrom(src => src.SMTPPort));
                cfg.CreateMap<MailSettingsViewModel, IEmail>().ConstructUsing(x => NInjectKernel.Get<IEmail>());
                cfg.CreateMap<PushbulletSettingsViewModel, IEmail>().ConstructUsing(x => NInjectKernel.Get<IEmail>());
                cfg.CreateMap<LoginViewModel, IAuthDetails>().ConstructUsing(x => NInjectKernel.Get<IAuthDetails>());
                cfg.CreateMap<IAuthDetails, LoginViewModel>();
            });
            
            NInjectKernel.Bind<IMapper>().ToConstant(mapperConfig.CreateMapper());
            NInjectKernel.Bind<IMessageBoxService>().To<MessageBoxService>();
        }

        public MultiControllerHomeViewModel MultiControllerHomeVM
        {
            get
            {
                return NInjectKernel.Get<MultiControllerHomeViewModel>();
            }
        }
                
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}