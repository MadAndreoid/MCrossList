
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace MCrossList.Client
{
    public partial class dbService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public dbService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/db/");
        }


        public async System.Threading.Tasks.Task ExportBrandsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/brands/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/brands/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportBrandsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/brands/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/brands/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetBrands(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Brand>> GetBrands(Query query)
        {
            return await GetBrands(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Brand>> GetBrands(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"Brands");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetBrands(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Brand>>(response);
        }

        partial void OnCreateBrand(HttpRequestMessage requestMessage);

        public async Task<MCrossList.Server.Models.db.Brand> CreateBrand(MCrossList.Server.Models.db.Brand brand = default(MCrossList.Server.Models.db.Brand))
        {
            var uri = new Uri(baseUri, $"Brands");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(brand), Encoding.UTF8, "application/json");

            OnCreateBrand(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<MCrossList.Server.Models.db.Brand>(response);
        }

        partial void OnDeleteBrand(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteBrand(long id = default(long))
        {
            var uri = new Uri(baseUri, $"Brands({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteBrand(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetBrandById(HttpRequestMessage requestMessage);

        public async Task<MCrossList.Server.Models.db.Brand> GetBrandById(string expand = default(string), long id = default(long))
        {
            var uri = new Uri(baseUri, $"Brands({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetBrandById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<MCrossList.Server.Models.db.Brand>(response);
        }

        partial void OnUpdateBrand(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateBrand(long id = default(long), MCrossList.Server.Models.db.Brand brand = default(MCrossList.Server.Models.db.Brand))
        {
            var uri = new Uri(baseUri, $"Brands({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(brand), Encoding.UTF8, "application/json");

            OnUpdateBrand(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportCategoriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/categories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/categories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportCategoriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/categories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/categories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetCategories(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Category>> GetCategories(Query query)
        {
            return await GetCategories(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Category>> GetCategories(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"Categories");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCategories(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Category>>(response);
        }

        partial void OnCreateCategory(HttpRequestMessage requestMessage);

        public async Task<MCrossList.Server.Models.db.Category> CreateCategory(MCrossList.Server.Models.db.Category category = default(MCrossList.Server.Models.db.Category))
        {
            var uri = new Uri(baseUri, $"Categories");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(category), Encoding.UTF8, "application/json");

            OnCreateCategory(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<MCrossList.Server.Models.db.Category>(response);
        }

        partial void OnDeleteCategory(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteCategory(long id = default(long))
        {
            var uri = new Uri(baseUri, $"Categories({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteCategory(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetCategoryById(HttpRequestMessage requestMessage);

        public async Task<MCrossList.Server.Models.db.Category> GetCategoryById(string expand = default(string), long id = default(long))
        {
            var uri = new Uri(baseUri, $"Categories({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCategoryById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<MCrossList.Server.Models.db.Category>(response);
        }

        partial void OnUpdateCategory(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateCategory(long id = default(long), MCrossList.Server.Models.db.Category category = default(MCrossList.Server.Models.db.Category))
        {
            var uri = new Uri(baseUri, $"Categories({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(category), Encoding.UTF8, "application/json");

            OnUpdateCategory(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportColorsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/colors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/colors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportColorsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/colors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/colors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetColors(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Color>> GetColors(Query query)
        {
            return await GetColors(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Color>> GetColors(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"Colors");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetColors(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Color>>(response);
        }

        partial void OnCreateColor(HttpRequestMessage requestMessage);

        public async Task<MCrossList.Server.Models.db.Color> CreateColor(MCrossList.Server.Models.db.Color color = default(MCrossList.Server.Models.db.Color))
        {
            var uri = new Uri(baseUri, $"Colors");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(color), Encoding.UTF8, "application/json");

            OnCreateColor(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<MCrossList.Server.Models.db.Color>(response);
        }

        partial void OnDeleteColor(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteColor(long id = default(long))
        {
            var uri = new Uri(baseUri, $"Colors({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteColor(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetColorById(HttpRequestMessage requestMessage);

        public async Task<MCrossList.Server.Models.db.Color> GetColorById(string expand = default(string), long id = default(long))
        {
            var uri = new Uri(baseUri, $"Colors({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetColorById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<MCrossList.Server.Models.db.Color>(response);
        }

        partial void OnUpdateColor(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateColor(long id = default(long), MCrossList.Server.Models.db.Color color = default(MCrossList.Server.Models.db.Color))
        {
            var uri = new Uri(baseUri, $"Colors({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(color), Encoding.UTF8, "application/json");

            OnUpdateColor(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportConditionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/conditions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/conditions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportConditionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/conditions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/conditions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetConditions(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Condition>> GetConditions(Query query)
        {
            return await GetConditions(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Condition>> GetConditions(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"Conditions");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetConditions(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Condition>>(response);
        }

        partial void OnCreateCondition(HttpRequestMessage requestMessage);

        public async Task<MCrossList.Server.Models.db.Condition> CreateCondition(MCrossList.Server.Models.db.Condition condition = default(MCrossList.Server.Models.db.Condition))
        {
            var uri = new Uri(baseUri, $"Conditions");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(condition), Encoding.UTF8, "application/json");

            OnCreateCondition(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<MCrossList.Server.Models.db.Condition>(response);
        }

        partial void OnDeleteCondition(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteCondition(long id = default(long))
        {
            var uri = new Uri(baseUri, $"Conditions({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteCondition(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetConditionById(HttpRequestMessage requestMessage);

        public async Task<MCrossList.Server.Models.db.Condition> GetConditionById(string expand = default(string), long id = default(long))
        {
            var uri = new Uri(baseUri, $"Conditions({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetConditionById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<MCrossList.Server.Models.db.Condition>(response);
        }

        partial void OnUpdateCondition(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateCondition(long id = default(long), MCrossList.Server.Models.db.Condition condition = default(MCrossList.Server.Models.db.Condition))
        {
            var uri = new Uri(baseUri, $"Conditions({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(condition), Encoding.UTF8, "application/json");

            OnUpdateCondition(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportMaterialsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/materials/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/materials/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportMaterialsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/materials/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/materials/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetMaterials(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Material>> GetMaterials(Query query)
        {
            return await GetMaterials(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Material>> GetMaterials(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"Materials");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMaterials(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Material>>(response);
        }

        partial void OnCreateMaterial(HttpRequestMessage requestMessage);

        public async Task<MCrossList.Server.Models.db.Material> CreateMaterial(MCrossList.Server.Models.db.Material material = default(MCrossList.Server.Models.db.Material))
        {
            var uri = new Uri(baseUri, $"Materials");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(material), Encoding.UTF8, "application/json");

            OnCreateMaterial(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<MCrossList.Server.Models.db.Material>(response);
        }

        partial void OnDeleteMaterial(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteMaterial(long id = default(long))
        {
            var uri = new Uri(baseUri, $"Materials({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteMaterial(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetMaterialById(HttpRequestMessage requestMessage);

        public async Task<MCrossList.Server.Models.db.Material> GetMaterialById(string expand = default(string), long id = default(long))
        {
            var uri = new Uri(baseUri, $"Materials({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMaterialById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<MCrossList.Server.Models.db.Material>(response);
        }

        partial void OnUpdateMaterial(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateMaterial(long id = default(long), MCrossList.Server.Models.db.Material material = default(MCrossList.Server.Models.db.Material))
        {
            var uri = new Uri(baseUri, $"Materials({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(material), Encoding.UTF8, "application/json");

            OnUpdateMaterial(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportProductsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/products/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/products/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportProductsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/products/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/products/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetProducts(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Product>> GetProducts(Query query)
        {
            return await GetProducts(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Product>> GetProducts(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"Products");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetProducts(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Product>>(response);
        }

        partial void OnCreateProduct(HttpRequestMessage requestMessage);

        public async Task<MCrossList.Server.Models.db.Product> CreateProduct(MCrossList.Server.Models.db.Product product = default(MCrossList.Server.Models.db.Product))
        {
            var uri = new Uri(baseUri, $"Products");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(product), Encoding.UTF8, "application/json");

            OnCreateProduct(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<MCrossList.Server.Models.db.Product>(response);
        }

        partial void OnDeleteProduct(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteProduct(long id = default(long))
        {
            var uri = new Uri(baseUri, $"Products({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteProduct(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetProductById(HttpRequestMessage requestMessage);

        public async Task<MCrossList.Server.Models.db.Product> GetProductById(string expand = default(string), long id = default(long))
        {
            var uri = new Uri(baseUri, $"Products({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetProductById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<MCrossList.Server.Models.db.Product>(response);
        }

        partial void OnUpdateProduct(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateProduct(long id = default(long), MCrossList.Server.Models.db.Product product = default(MCrossList.Server.Models.db.Product))
        {
            var uri = new Uri(baseUri, $"Products({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(product), Encoding.UTF8, "application/json");

            OnUpdateProduct(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportSizesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/sizes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/sizes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportSizesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/sizes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/sizes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetSizes(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Size>> GetSizes(Query query)
        {
            return await GetSizes(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Size>> GetSizes(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"Sizes");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSizes(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Size>>(response);
        }

        partial void OnCreateSize(HttpRequestMessage requestMessage);

        public async Task<MCrossList.Server.Models.db.Size> CreateSize(MCrossList.Server.Models.db.Size size = default(MCrossList.Server.Models.db.Size))
        {
            var uri = new Uri(baseUri, $"Sizes");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(size), Encoding.UTF8, "application/json");

            OnCreateSize(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<MCrossList.Server.Models.db.Size>(response);
        }

        partial void OnDeleteSize(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteSize(long id = default(long))
        {
            var uri = new Uri(baseUri, $"Sizes({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteSize(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetSizeById(HttpRequestMessage requestMessage);

        public async Task<MCrossList.Server.Models.db.Size> GetSizeById(string expand = default(string), long id = default(long))
        {
            var uri = new Uri(baseUri, $"Sizes({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSizeById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<MCrossList.Server.Models.db.Size>(response);
        }

        partial void OnUpdateSize(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateSize(long id = default(long), MCrossList.Server.Models.db.Size size = default(MCrossList.Server.Models.db.Size))
        {
            var uri = new Uri(baseUri, $"Sizes({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(size), Encoding.UTF8, "application/json");

            OnUpdateSize(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStoresToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/stores/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/stores/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStoresToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/stores/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/stores/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStores(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Store>> GetStores(Query query)
        {
            return await GetStores(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Store>> GetStores(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"Stores");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStores(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<MCrossList.Server.Models.db.Store>>(response);
        }

        partial void OnCreateStore(HttpRequestMessage requestMessage);

        public async Task<MCrossList.Server.Models.db.Store> CreateStore(MCrossList.Server.Models.db.Store store = default(MCrossList.Server.Models.db.Store))
        {
            var uri = new Uri(baseUri, $"Stores");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(store), Encoding.UTF8, "application/json");

            OnCreateStore(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<MCrossList.Server.Models.db.Store>(response);
        }

        partial void OnDeleteStore(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteStore(long id = default(long))
        {
            var uri = new Uri(baseUri, $"Stores({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteStore(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStoreById(HttpRequestMessage requestMessage);

        public async Task<MCrossList.Server.Models.db.Store> GetStoreById(string expand = default(string), long id = default(long))
        {
            var uri = new Uri(baseUri, $"Stores({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStoreById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<MCrossList.Server.Models.db.Store>(response);
        }

        partial void OnUpdateStore(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateStore(long id = default(long), MCrossList.Server.Models.db.Store store = default(MCrossList.Server.Models.db.Store))
        {
            var uri = new Uri(baseUri, $"Stores({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(store), Encoding.UTF8, "application/json");

            OnUpdateStore(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}