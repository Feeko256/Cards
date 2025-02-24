using Cards.DI.Loader;
using Cards.DI.Service;
using Cards.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Cards
{
    public partial class App : Application
    {
        public IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<IDataLoader, DataLoader>();

            services.AddTransient<MainPageViewModel>();
            services.AddTransient<EditPackViewModel>();
            services.AddTransient<SettingsPageViewModel>();
            services.AddTransient<MainWindowViewModel>();

            services.AddTransient<MainWindow>();
        }
    }

}
