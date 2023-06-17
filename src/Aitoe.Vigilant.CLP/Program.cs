using Ninject;
using System;

namespace Aitoe.Vigilant.CLP
{
    class Program
    {
        static void Main(string[] args)
        {

            var app = new CommandLineAppBootstrapper();
            app.Start(args);

            //var p = new ChangeCaseProcessor();
            //p.Process(args, Console.In, Console.Out, Console.Error);


            //WriteWarning("This is a warning");
            //WriteError("This is an error");
            //// Console Title property.//Pluralsight Player_29
            //Console.Title = Assembly.GetExecutingAssembly().GetName().FullName;
            //var options = new AitoeVigilantAlertOptions();
            //CommandLine.Parser.Default.ParseArgumentsStrict(args, options, OnFail);
        }
        
        private static void WriteWarning(string sWarningMessage)
        {
            //Pluralsight Player_31 // Forground color and back ground color
            StoreUsersForeGroundAndBackgroundColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(sWarningMessage);
            ReStoreUsersForeGroundAndBackgroundColor();
        }

        private static void WriteError(string sErrorMessage)
        {
            //Pluralsight Player_31 // Forground color and back ground color
            StoreUsersForeGroundAndBackgroundColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(sErrorMessage);
            ReStoreUsersForeGroundAndBackgroundColor();
        }

        private static ConsoleColor _ForegroundColor = Console.ForegroundColor;
        private static ConsoleColor _BackgroundColor = Console.BackgroundColor;

        private static void StoreUsersForeGroundAndBackgroundColor()
        {
            _ForegroundColor = Console.ForegroundColor;
            _BackgroundColor = Console.BackgroundColor;
        }

        private static void ReStoreUsersForeGroundAndBackgroundColor()
        {
            Console.ForegroundColor = _ForegroundColor;
            Console.BackgroundColor = _BackgroundColor;
        }

        private static void WriteError()
        {

        }
        
        private static void OnFail()
        {
            Console.ReadLine();
            Environment.Exit(-1);
        }
    }
}
