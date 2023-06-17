using System.Windows;
using System.Windows.Input;

namespace Aitoe.Vigilant.Controller.WpfController.Views
{
    /// <summary>
    /// Interaction logic for MultiControllerHomeView.xaml
    /// </summary>
    public partial class MultiControllerHomeView : Window
    {
        
        //public WindowState lastWindowState;
        public MultiControllerHomeView()
        {
            
            InitializeComponent();
        }

        //private void Window_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.F11)
        //    {
        //        ToggleFullScreen();
        //    }
        //}

        //private void ToggleFullScreen(object sender, RoutedEventArgs e)
        //{
        //    ToggleFullScreen();
        //}

        //private void ToggleFullScreen()
        //{
        //    if (IsFullscreen)
        //    {
        //        MainMenu.Visibility = Visibility.Visible;
        //        this.WindowStyle = WindowStyle.SingleBorderWindow;
        //        //this.WindowState = lastWindowState;
        //        IsFullscreen = false;
        //    }
        //    else
        //    {
        //        MainMenu.Visibility = Visibility.Collapsed;
        //        //lastWindowState = this.WindowState;
        //        this.WindowStyle = WindowStyle.None;
        //        this.WindowState = WindowState.Maximized;
        //        IsFullscreen = true;
        //    }
        //}

        //private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    if (IsFullscreen)
        //    {
        //        MainMenu.Visibility = Visibility.Visible;
        //        this.WindowStyle = WindowStyle.SingleBorderWindow;
        //        //this.WindowState = lastWindowState;
        //        IsFullscreen = false;
        //    }
        //}
    }
}
