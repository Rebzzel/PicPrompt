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
            if (SingleInstance<App>.InitializeAsFirstInstance("PicPrompt"))
            {
                var splitPath = SingleInstance<App>.CommandLineArgs[0].Split('\\');

                string dirPath = splitPath[0];
                for (int i = 1; i < splitPath.Length - 1; i++)
                {
                    dirPath += $@"\{splitPath[i]}";
                }

                var app = new App();
                app.InitializeComponent();
                app.InitializeConfig(dirPath);
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
            var splitPath = args[0].Split('\\');

            string dirPath = splitPath[0];
            for (int i = 1; i < splitPath.Length - 1; i++)
            {
                dirPath += $@"\{splitPath[i]}";
            }

            InitializeConfig(dirPath);

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

        public void InitializeConfig(string path)
        {
            var filePath = $"{path}\\PicPrompt.json";

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "{\"allow-background-work\":true}");
            }

            Config = new Utils.Configuration(filePath);
        }
    }
}
