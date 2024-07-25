using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimCoRetailManager_WPF.Library.Models;

namespace TimCoRetailManager_WPF.Library.Services
{
    public interface ISaleService
    {
        Task PostAsync(SaleDTO sale);
    }

    public class SaleService : ISaleService
    {
        private readonly IApi _api;

        public SaleService(IApi api)
        {
            _api = api;
        }

        public async Task PostAsync(SaleDTO sale)
        {
            using (var res = await _api.Http.PostAsJsonAsync("/api/sales/post", sale))
            {
                if (!res.IsSuccessStatusCode)
                    throw new Exception(res.ReasonPhrase);
            }
        }
    }
}
