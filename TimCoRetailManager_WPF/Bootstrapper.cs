using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TimCoRetailManager_WPF.Helpers;
using TimCoRetailManager_WPF.Library.Models;
using TimCoRetailManager_WPF.Library.Services;
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
            container.Instance(container);

            // Services
            container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>() // for broadcasting and listening to events
                .Singleton<IApiService, ApiService>()

                .Singleton<IUser, User>();  // App wide user

            container.PerRequest<ITestDI, TestDI>();

            // Link views to view models
            GetType().Assembly.GetTypes().Where(t => t.IsClass && t.Name.EndsWith("ViewModel")).ToList().ForEach(v => container.RegisterPerRequest(v, v.ToString(), v));
        }
    }
}
