using GalaSoft.MvvmLight;
using System;

namespace Aitoe.Vigilant.Controller.WpfController.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AitoeVigilantViewModelBase : ViewModelBase
    {
        public Uri AisentinelLogoSource { get; set; }
        public Uri AitoeLogoSource { get; set; }

        public Uri HomeBackground { get; set; }


        public Uri HomeBackgroundTransparent { get; set; }
        /// <summary>
        /// Initializes a new instance of the AitoeVigilantViewModelBase class.
        /// </summary>
        public AitoeVigilantViewModelBase()
        {
            HomeBackground = new Uri("/Icons/HomeBackground.jpg", UriKind.Relative);
            HomeBackgroundTransparent = new Uri("/Icons/HomeBackgroundTransparent.png", UriKind.Relative);
            //AitoeImageSource = new Uri("/Icons/aitoe_Logo_new_new.png", UriKind.Relative);
            AisentinelLogoSource = new Uri("/Icons/aisentinelLogo.png", UriKind.Relative);
            AitoeLogoSource = new Uri("/Icons/aitoe_Logo.png", UriKind.Relative);
        }
    }
}