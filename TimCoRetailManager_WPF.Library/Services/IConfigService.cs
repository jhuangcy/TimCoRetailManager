using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoRetailManager_WPF.Library.Services
{
    public interface IConfigService
    {
        decimal GetTax();
    }

    public class ConfigService : IConfigService
    {
        public decimal GetTax() => decimal.TryParse(ConfigurationManager.AppSettings["tax"], out var output) ? output : throw new ConfigurationErrorsException("The tax rate is not set up properly");
    }
}
