using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Instead of the UI getting a business value from its own config, the UI will call an api to get it from the server config instead
// After changing to use appsettings, this is no longer needed
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
