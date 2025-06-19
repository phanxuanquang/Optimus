using Microsoft.UI;
using Microsoft.UI.Xaml;
using System.Threading.Tasks;

namespace Optimus
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.AppWindow.Title = "Optimus";
            this.AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            this.AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            this.AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;

            _ = FinishStartupAsync();
        }

        private async Task FinishStartupAsync()
        {
            // Simulate some startup work, like loading resources or initializing services
            await Task.Delay(2000);
        }
    }
}
