using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimCoRetailManager_WPF.Library.Models;

namespace TimCoRetailManager_WPF.Library.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();
    }

    public class ProductService : IProductService
    {
        private readonly IApi _api;

        public ProductService(IApi api)
        {
            _api = api;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            //_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            using (var res = await _api.Http.GetAsync("/api/products/get"))
            {
                if (res.IsSuccessStatusCode)
                    return await res.Content.ReadAsAsync<List<Product>>();
                else
                    throw new Exception(res.ReasonPhrase);
            }
        }
    }
}
