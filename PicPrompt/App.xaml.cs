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
                App app = new App();
                app.InitializeComponent();
                app.MainWindow = new MainWindow();

                var args = SingleInstance<App>.CommandLineArgs;
                if (args.Count > 1)
                    ((MainWindow)app.MainWindow).OpenImage(args[1]);

                app.Run(app.MainWindow);

                SingleInstance<App>.Cleanup();
            }
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            var mainWindow = (MainWindow)this.MainWindow;

            if (args.Count > 1)
                mainWindow.OpenImage(args[1]);

            if (!mainWindow.IsActive)
                mainWindow.Show();

            return true;
        }
    }
}
