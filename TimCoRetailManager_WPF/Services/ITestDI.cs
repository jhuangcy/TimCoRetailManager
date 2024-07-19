using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoRetailManager_WPF.Services
{
    public interface ITestDI
    {
        List<string> Register { get; set; }
        double Add(double x, double y);
    }

    class TestDI : ITestDI
    {
        public List<string> Register { get; set; } = new List<string>();    // should show "Count = 0"

        public double Add(double x, double y) => x + y;
    }
}
