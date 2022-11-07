using ABE.LTRIC.Core.Interfaces;
using ABE.LTRIC.Infrastructure;
using ABE.LTRIC.Infrastructure.Interfaces;
using ABE.LTRIC.Infrastructure.Services;
using ABE.LTRIC.WpfGui.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ABE.LTRIC.WpfGui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration? Configuration { get; private set; }

        public new static App Current => (App)Application.Current;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        protected void OnStartup(object sender, StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindowView>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LTRIC_Context>(item => item.UseSqlServer(Configuration.GetConnectionString("LTRIC_Database")));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IDocRepository, DocRepository>();
            services.AddTransient<IDocDtlRepository, DocDtlRepository>();
            services.AddTransient<ISettingRepository, SettingRepository>();
            //Views
            services.AddSingleton<MainWindowView>();
            //ViewModels
            services.AddSingleton<MainWindowViewModel>();
        }
    }
}
