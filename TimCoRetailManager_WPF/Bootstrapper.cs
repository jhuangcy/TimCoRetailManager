using AutoMapper;
using Caliburn.Micro;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TimCoRetailManager_WPF.Helpers;
using TimCoRetailManager_WPF.Library;
using TimCoRetailManager_WPF.Library.Models;
using TimCoRetailManager_WPF.Library.Services;
using TimCoRetailManager_WPF.Models;
using TimCoRetailManager_WPF.Services;
using TimCoRetailManager_WPF.ViewModels;

namespace TimCoRetailManager_WPF
{
    public class Bootstrapper : BootstrapperBase
    {
        // Setup dependency injection
        SimpleContainer container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();

            // https://stackoverflow.com/questions/30631522/caliburn-micro-support-for-passwordbox
            ConventionManager.AddElementConvention<PasswordBox>(PasswordBoxHelper.BoundPasswordProperty, "Password", "PasswordChanged");
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key) => container.GetInstance(service, key);
        protected override IEnumerable<object> GetAllInstances(Type service) => container.GetAllInstances(service);
        protected override void BuildUp(object instance) => container.BuildUp(instance);
        protected override void Configure()
        {
            //container.Instance(container);    // allows access to container in view models, but just use IoC

            // appsettings
            container.RegisterInstance(typeof(IConfiguration), "IConfiguration", AddConfig());

            // Services
            container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>() // for broadcasting and listening to events
                .Instance(ConfigAutomapper())

                .PerRequest<ITestDI, TestDI>()
                .Singleton<IApi, Api>()
                .Singleton<IConfigService, ConfigService>()
                .PerRequest<IUserService, UserService>()
                .PerRequest<IProductService, ProductService>()
                .PerRequest<ISaleService, SaleService>()

                .Singleton<IUser, User>();  // app-wide user
                
            // Link views to view models
            GetType().Assembly.GetTypes().Where(t => t.IsClass && t.Name.EndsWith("ViewModel")).ToList().ForEach(v => container.RegisterPerRequest(v, v.ToString(), v));
        }

        // Automapper
        IMapper ConfigAutomapper() => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Product, ProductVM>();
            cfg.CreateMap<CartItem, CartItemVM>();
            cfg.CreateMap<UserDTO, UserVM>();
        }).CreateMapper();

        // To use appsettings.json
        IConfiguration AddConfig()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
#if DEBUG
            builder.AddJsonFile("appsettings.Development.json", true, true);
#else
            builder.AddJsonFile("appsettings.json", true, true);
#endif
            return builder.Build();
        }
    }
}
