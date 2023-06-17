using Aitoe.Vigilant.Controller.WpfController.Infra.SingleInstanceApp;
using Aitoe.Vigilant.Controller.WpfController.Properties;
using Aitoe.Vigilant.Controller.WpfController.Views;
using log4net.Config;
using System;
using System.Windows;


namespace Aitoe.Vigilant.Controller.WpfController
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class AppWpfController : Application
    {
        //private readonly log4net.ILog log;
        public AppWpfController()
        {

            InitializeComponent();

            XmlConfigurator.Configure();

            //DateTime expiryDateTime = new DateTime(2016, 11, 30);
            //DateTime expiryDateTime = new DateTime(2016, 07, 30);
            //if (DateTime.Now > expiryDateTime || Settings.Default.IsExpired)
            //{
            //    MessageBox.Show("You Aitoe Red Controller Software expired.", "Software Expired", MessageBoxButton.OK, MessageBoxImage.Stop);
            //    Settings.Default.IsExpired = true;
            //    Settings.Default.Save();
            //    Shutdown();
            //}
        }

        [STAThread]
        static void Main()
        {
            SingleInstanceApplicationWrapper manager = new SingleInstanceApplicationWrapper();
            manager.Run(new[] { "test" });
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //AppWpfController.Current = this;
            // Create and show the application's main window
            ////StartupUri="Views/MultiControllerHomeView.xaml"
            var startingWindow = new MultiControllerHomeView();
            startingWindow.Show();
        }

        public void Activate()
        {
            // Reactivate application's main window
            this.MainWindow.WindowState = WindowState.Maximized;
            this.MainWindow.Activate();
        }

    }
}
