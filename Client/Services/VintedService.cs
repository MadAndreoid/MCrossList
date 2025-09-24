using MCrossList.Server.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace MCrossList.Client.Services
{
    public class VintedService : IVintedService
    {
        private readonly Uri baseUri;
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;

        #region Properties
        public int Items { get; set; }
        #endregion

        public VintedService(NavigationManager navigationManager, IHttpClientFactory factory)
        {
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/Identity/");
            _navigationManager = navigationManager;
            _httpClient = factory.CreateClient("MCrossList.Server");
            _httpClient.Timeout = TimeSpan.FromMinutes(10);

        }

        public async Task Update()
        {
            var uri = new Uri($"{_navigationManager.BaseUri}Vinted/GetProductsNumber");
            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (int.TryParse(content, out int count))
                {
                    Items = count;
                }
            }
        }
    }
}
