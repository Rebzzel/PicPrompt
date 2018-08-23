using Microsoft.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace PicPrompt
{
    public partial class App : Application, ISingleInstanceApp
    {
        public static Utils.Configuration Config = new Utils.Configuration();

        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance("PicPrompt"))
            {
                var args = SingleInstance<App>.CommandLineArgs;

                var app = new App();
                app.InitializeComponent();
                app.InitiliazeConfig(Path.GetDirectoryName(args[0]));

                var backgroundWindow = new Window();
                var mainWindow = new MainWindow();

                mainWindow.Closed += (_, __) =>
                {
                    if ((bool)Config["allow-background-work"] == false)
                    {
                        backgroundWindow.Close();
                    }
                };

                if (args.Count > 1)
                {
                    mainWindow.OpenImage(args[1]);
                }

                app.Run(mainWindow);

                SingleInstance<App>.Cleanup();
            }
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            InitiliazeConfig(Path.GetDirectoryName(args[0]));

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
            {
                mainWindow.OpenImage(args[1]);
            }

            return true;
        }

        public void InitiliazeConfig(string path)
        {
            var filePath = $"{path}\\PicPrompt.json";

            if (!File.Exists(filePath))
            {
                var writer = new LitJson.JsonWriter();

                writer.WriteObjectStart();
                writer.WritePropertyName("start-with-windows-enabled");
                writer.Write(false);
                writer.WritePropertyName("allow-background-work");
                writer.Write(true);
                writer.WriteObjectEnd();

                File.WriteAllText(filePath, writer.ToString());
            }

            Config.Load(filePath);
        }
    }
}
