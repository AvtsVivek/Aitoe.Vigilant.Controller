using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.SingleInstanceApp
{
    public class SingleInstanceApplicationWrapper : WindowsFormsApplicationBase
    {
        public SingleInstanceApplicationWrapper()
        {
            // Enable single-instance mode.
            IsSingleInstance = true;
        }

        AppWpfController app;

        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            

            app = new AppWpfController();
            app.Run();
            return false;

            //return base.OnStartup(eventArgs);

        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs e)
        {
            //if (e.CommandLine.Count > 0)
            //{
            //    app.ShowDocument(e.CommandLine[0]);
            //}
            base.OnStartupNextInstance(e);
            app.Activate();

        }
    }
}
