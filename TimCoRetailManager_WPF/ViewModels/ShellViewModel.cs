using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimCoRetailManager_WPF.Services;

namespace TimCoRetailManager_WPF.ViewModels
{
    public class ShellViewModel
    {
        private readonly ITestDI _testDI;

        public ShellViewModel(ITestDI testDI)
        {
            _testDI = testDI;
        }
    }
}
