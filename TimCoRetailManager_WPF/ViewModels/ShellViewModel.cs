using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimCoRetailManager_WPF.Services;

namespace TimCoRetailManager_WPF.ViewModels
{
    // This will be the main container view model that will load other view models into it
    public class ShellViewModel : Conductor<object>
    {
        //private readonly ITestDI _testDI;

        private readonly LoginViewModel _loginViewModel;

        public ShellViewModel(ITestDI testDI, LoginViewModel loginViewModel)
        {
            //_testDI = testDI;

            _loginViewModel = loginViewModel;

            ActivateItem(_loginViewModel);
        }
    }
}
