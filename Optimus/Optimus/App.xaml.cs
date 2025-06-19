using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Optimus.LocalDatabase;
using System.IO;
using System.Threading.Tasks;

namespace Optimus
{
    public partial class App : Application
    {
        public static IHost Host { get; private set; }
        public static Window Window { get; private set; }
        public static AppDbContext LocalDb { get; private set; }


        public App()
        {
            InitializeComponent();
            Host = CreateHostBuilder();
            LocalDb = Host.Services.GetService<AppDbContext>();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Window = new MainWindow();
            Window.Activate();
            _ = InitializeAsync();
        }

        private static IHost CreateHostBuilder()
        {
            return Microsoft.Extensions.Hosting.Host
                .CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<AppDbContext>(options => options.UseSqlite($"Data Source={AppDbContext.DbPath}"));
                })
                .Build();
        }

        private static async Task InitializeAsync()
        {
            string dbPath = AppDbContext.DbPath;

            if (!File.Exists(dbPath))
            {
                var directoryPath = Path.GetDirectoryName(dbPath);

                if (!string.IsNullOrEmpty(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                await LocalDb.Database.EnsureCreatedAsync().ConfigureAwait(false);
            }
        }
    }
}
