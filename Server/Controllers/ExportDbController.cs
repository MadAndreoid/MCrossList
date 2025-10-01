using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using MCrossList.Server.Data;

namespace MCrossList.Server.Controllers
{
    public partial class ExportdbController : ExportController
    {
        private readonly dbContext context;
        private readonly dbService service;

        public ExportdbController(dbContext context, dbService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/db/brands/csv")]
        [HttpGet("/export/db/brands/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBrandsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBrands(), Request.Query, false), fileName);
        }

        [HttpGet("/export/db/brands/excel")]
        [HttpGet("/export/db/brands/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBrandsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBrands(), Request.Query, false), fileName);
        }

        [HttpGet("/export/db/categories/csv")]
        [HttpGet("/export/db/categories/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCategoriesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCategories(), Request.Query, false), fileName);
        }

        [HttpGet("/export/db/categories/excel")]
        [HttpGet("/export/db/categories/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCategoriesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCategories(), Request.Query, false), fileName);
        }

        [HttpGet("/export/db/colors/csv")]
        [HttpGet("/export/db/colors/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportColorsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetColors(), Request.Query, false), fileName);
        }

        [HttpGet("/export/db/colors/excel")]
        [HttpGet("/export/db/colors/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportColorsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetColors(), Request.Query, false), fileName);
        }

        [HttpGet("/export/db/conditions/csv")]
        [HttpGet("/export/db/conditions/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportConditionsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetConditions(), Request.Query, false), fileName);
        }

        [HttpGet("/export/db/conditions/excel")]
        [HttpGet("/export/db/conditions/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportConditionsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetConditions(), Request.Query, false), fileName);
        }

        [HttpGet("/export/db/materials/csv")]
        [HttpGet("/export/db/materials/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMaterialsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetMaterials(), Request.Query, false), fileName);
        }

        [HttpGet("/export/db/materials/excel")]
        [HttpGet("/export/db/materials/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMaterialsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetMaterials(), Request.Query, false), fileName);
        }

        [HttpGet("/export/db/products/csv")]
        [HttpGet("/export/db/products/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProductsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetProducts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/db/products/excel")]
        [HttpGet("/export/db/products/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProductsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetProducts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/db/sizes/csv")]
        [HttpGet("/export/db/sizes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSizesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSizes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/db/sizes/excel")]
        [HttpGet("/export/db/sizes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSizesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSizes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/db/stores/csv")]
        [HttpGet("/export/db/stores/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStoresToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStores(), Request.Query, false), fileName);
        }

        [HttpGet("/export/db/stores/excel")]
        [HttpGet("/export/db/stores/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStoresToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStores(), Request.Query, false), fileName);
        }
    }
}
