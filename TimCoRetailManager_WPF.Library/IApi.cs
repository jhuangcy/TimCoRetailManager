using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TimCoRetailManager_WPF.Library
{
    public interface IApi
    {
        HttpClient Http { get; }
        void ClearHeaders();
    }

    public class Api : IApi
    {
        private readonly IConfiguration _config;
        HttpClient _http;
        public HttpClient Http => _http;

        public Api(IConfiguration config)
        {
            _config = config;
            InitHttp();
        }

        void InitHttp()
        {
            //var api = ConfigurationManager.AppSettings["api"];
            var api = _config.GetValue<string>("api");

            _http = new HttpClient();
            _http.DefaultRequestHeaders.Accept.Clear();
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _http.BaseAddress = new Uri(api);
        }

        // To remove token
        public void ClearHeaders()
        {
            _http.DefaultRequestHeaders.Clear();
        }
    }
}
