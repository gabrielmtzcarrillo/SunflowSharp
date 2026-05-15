using Microsoft.UI.Xaml;
using System.Runtime.Versioning;

namespace SunflowSharp.Gui
{
    [SupportedOSPlatform("windows10.0.17763.0")]
    public partial class App : Application
    {
        private Window? window;

        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            window = new MainWindow();
            window.Activate();
        }
    }
}
