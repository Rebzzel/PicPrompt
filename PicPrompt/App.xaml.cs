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
                app.Run(new MainWindow());

                SingleInstance<App>.Cleanup();
            }
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            return true;
        }
    }
}
