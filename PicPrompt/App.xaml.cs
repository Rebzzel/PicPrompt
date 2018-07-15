using Microsoft.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace PicPrompt
{
    public partial class App : Application, ISingleInstanceApp
    {
        public static Utils.Configuration Config;

        private Window _backgroundWindow;

        [STAThread]
        public static void Main()
        {
            if (!File.Exists("PicPrompt.json"))
            {
                File.WriteAllText("PicPrompt.json",
@"{
    ""allow-background-work"": true
}");
            }

            Config = new Utils.Configuration("PicPrompt.json");

            if (SingleInstance<App>.InitializeAsFirstInstance("PicPrompt"))
            {
                var app = new App();
                app.InitializeComponent();
                app._backgroundWindow = new Window();
                
                var mainWindow = new MainWindow();

                mainWindow.Closed += (_, __) =>
                {
                    if (!(bool)Config["allow-background-work"])
                        app._backgroundWindow.Close();
                };

                var args = SingleInstance<App>.CommandLineArgs;
                if (args.Count > 1)
                    mainWindow.OpenImage(args[1]);

                app.Run(mainWindow);

                SingleInstance<App>.Cleanup();
            }
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            var mainWindow = MainWindow as MainWindow;

            if (mainWindow == null)
            {
                mainWindow = new MainWindow();
                mainWindow.Show();
                MainWindow = mainWindow;
            } else
            {
                mainWindow.Activate();
            }

            if (args.Count > 1)
                mainWindow.OpenImage(args[1]);

            return true;
        }
    }
}
