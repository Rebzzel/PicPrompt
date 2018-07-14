using Microsoft.Shell;
using System;
using System.Collections.Generic;
using System.Windows;

namespace PicPrompt
{
    public partial class App : Application, ISingleInstanceApp
    {
        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance("PicPrompt"))
            {
                var app = new App();
                app.InitializeComponent();

                var mainWindow = new MainWindow();

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
            }

            if (args.Count > 1)
                mainWindow.OpenImage(args[1]);

            return true;
        }
    }
}
