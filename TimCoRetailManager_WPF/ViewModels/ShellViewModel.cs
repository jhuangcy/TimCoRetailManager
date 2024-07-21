using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimCoRetailManager_WPF.EventModels;
using TimCoRetailManager_WPF.Services;

namespace TimCoRetailManager_WPF.ViewModels
{
    // This will be the main container view model that will load other view models into it
    public class ShellViewModel : Conductor<object>, IHandle<LoginEvent>
    {
        //private readonly ITestDI _testDI;
        private readonly IEventAggregator _events;

        private readonly SimpleContainer _container;
        //private LoginViewModel _loginViewModel;
        private readonly SalesViewModel _salesViewModel;

        public ShellViewModel(ITestDI testDI, IEventAggregator events, SimpleContainer container, /*LoginViewModel loginViewModel,*/ SalesViewModel salesViewModel)
        {
            //_testDI = testDI;
            _events = events;
            _container = container;
            //_loginViewModel = loginViewModel;
            _salesViewModel = salesViewModel;

            _events.Subscribe(this);

            // The container can be used to create a brand new view everytime instead
            //ActivateItem(_loginViewModel);
            ActivateItem(_container.GetInstance<LoginViewModel>());
        }

        // Handle specific event broadcasted by other view models
        public void Handle(LoginEvent message)
        {
            ActivateItem(_salesViewModel);  // will auto close the previous view

            // Need to replace the old login view so the original data does not show up again
            //_loginViewModel = _container.GetInstance<LoginViewModel>();
        }
    }
}
