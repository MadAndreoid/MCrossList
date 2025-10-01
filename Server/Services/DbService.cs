using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

using MCrossList.Server.Data;

namespace MCrossList.Server
{
    public partial class dbService
    {
        dbContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly dbContext context;
        private readonly NavigationManager navigationManager;

        public dbService(dbContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportBrandsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/brands/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/brands/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBrandsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/brands/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/brands/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBrandsRead(ref IQueryable<MCrossList.Server.Models.db.Brand> items);

        public async Task<IQueryable<MCrossList.Server.Models.db.Brand>> GetBrands(Query query = null)
        {
            var items = Context.Brands.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnBrandsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBrandGet(MCrossList.Server.Models.db.Brand item);
        partial void OnGetBrandById(ref IQueryable<MCrossList.Server.Models.db.Brand> items);


        public async Task<MCrossList.Server.Models.db.Brand> GetBrandById(long id)
        {
            var items = Context.Brands
                              .AsNoTracking()
                              .Where(i => i.ID == id);

 
            OnGetBrandById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnBrandGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBrandCreated(MCrossList.Server.Models.db.Brand item);
        partial void OnAfterBrandCreated(MCrossList.Server.Models.db.Brand item);

        public async Task<MCrossList.Server.Models.db.Brand> CreateBrand(MCrossList.Server.Models.db.Brand brand)
        {
            OnBrandCreated(brand);

            var existingItem = Context.Brands
                              .Where(i => i.ID == brand.ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Brands.Add(brand);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(brand).State = EntityState.Detached;
                throw;
            }

            OnAfterBrandCreated(brand);

            return brand;
        }

        public async Task<MCrossList.Server.Models.db.Brand> CancelBrandChanges(MCrossList.Server.Models.db.Brand item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBrandUpdated(MCrossList.Server.Models.db.Brand item);
        partial void OnAfterBrandUpdated(MCrossList.Server.Models.db.Brand item);

        public async Task<MCrossList.Server.Models.db.Brand> UpdateBrand(long id, MCrossList.Server.Models.db.Brand brand)
        {
            OnBrandUpdated(brand);

            var itemToUpdate = Context.Brands
                              .Where(i => i.ID == brand.ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(brand);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterBrandUpdated(brand);

            return brand;
        }

        partial void OnBrandDeleted(MCrossList.Server.Models.db.Brand item);
        partial void OnAfterBrandDeleted(MCrossList.Server.Models.db.Brand item);

        public async Task<MCrossList.Server.Models.db.Brand> DeleteBrand(long id)
        {
            var itemToDelete = Context.Brands
                              .Where(i => i.ID == id)
                              .Include(i => i.Products)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnBrandDeleted(itemToDelete);


            Context.Brands.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterBrandDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCategoriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/categories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/categories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCategoriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/categories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/categories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCategoriesRead(ref IQueryable<MCrossList.Server.Models.db.Category> items);

        public async Task<IQueryable<MCrossList.Server.Models.db.Category>> GetCategories(Query query = null)
        {
            var items = Context.Categories.AsQueryable();

            items = items.Include(i => i.Category_Father);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCategoriesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCategoryGet(MCrossList.Server.Models.db.Category item);
        partial void OnGetCategoryById(ref IQueryable<MCrossList.Server.Models.db.Category> items);


        public async Task<MCrossList.Server.Models.db.Category> GetCategoryById(long id)
        {
            var items = Context.Categories
                              .AsNoTracking()
                              .Where(i => i.ID == id);

            items = items.Include(i => i.Category_Father);
 
            OnGetCategoryById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCategoryGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCategoryCreated(MCrossList.Server.Models.db.Category item);
        partial void OnAfterCategoryCreated(MCrossList.Server.Models.db.Category item);

        public async Task<MCrossList.Server.Models.db.Category> CreateCategory(MCrossList.Server.Models.db.Category category)
        {
            OnCategoryCreated(category);

            var existingItem = Context.Categories
                              .Where(i => i.ID == category.ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Categories.Add(category);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(category).State = EntityState.Detached;
                throw;
            }

            OnAfterCategoryCreated(category);

            return category;
        }

        public async Task<MCrossList.Server.Models.db.Category> CancelCategoryChanges(MCrossList.Server.Models.db.Category item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCategoryUpdated(MCrossList.Server.Models.db.Category item);
        partial void OnAfterCategoryUpdated(MCrossList.Server.Models.db.Category item);

        public async Task<MCrossList.Server.Models.db.Category> UpdateCategory(long id, MCrossList.Server.Models.db.Category category)
        {
            OnCategoryUpdated(category);

            var itemToUpdate = Context.Categories
                              .Where(i => i.ID == category.ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(category);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCategoryUpdated(category);

            return category;
        }

        partial void OnCategoryDeleted(MCrossList.Server.Models.db.Category item);
        partial void OnAfterCategoryDeleted(MCrossList.Server.Models.db.Category item);

        public async Task<MCrossList.Server.Models.db.Category> DeleteCategory(long id)
        {
            var itemToDelete = Context.Categories
                              .Where(i => i.ID == id)
                              .Include(i => i.Products)
                              .Include(i => i.InverseCategory_Father)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCategoryDeleted(itemToDelete);


            Context.Categories.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCategoryDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportColorsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/colors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/colors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportColorsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/colors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/colors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnColorsRead(ref IQueryable<MCrossList.Server.Models.db.Color> items);

        public async Task<IQueryable<MCrossList.Server.Models.db.Color>> GetColors(Query query = null)
        {
            var items = Context.Colors.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnColorsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnColorGet(MCrossList.Server.Models.db.Color item);
        partial void OnGetColorById(ref IQueryable<MCrossList.Server.Models.db.Color> items);


        public async Task<MCrossList.Server.Models.db.Color> GetColorById(long id)
        {
            var items = Context.Colors
                              .AsNoTracking()
                              .Where(i => i.ID == id);

 
            OnGetColorById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnColorGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnColorCreated(MCrossList.Server.Models.db.Color item);
        partial void OnAfterColorCreated(MCrossList.Server.Models.db.Color item);

        public async Task<MCrossList.Server.Models.db.Color> CreateColor(MCrossList.Server.Models.db.Color color)
        {
            OnColorCreated(color);

            var existingItem = Context.Colors
                              .Where(i => i.ID == color.ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Colors.Add(color);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(color).State = EntityState.Detached;
                throw;
            }

            OnAfterColorCreated(color);

            return color;
        }

        public async Task<MCrossList.Server.Models.db.Color> CancelColorChanges(MCrossList.Server.Models.db.Color item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnColorUpdated(MCrossList.Server.Models.db.Color item);
        partial void OnAfterColorUpdated(MCrossList.Server.Models.db.Color item);

        public async Task<MCrossList.Server.Models.db.Color> UpdateColor(long id, MCrossList.Server.Models.db.Color color)
        {
            OnColorUpdated(color);

            var itemToUpdate = Context.Colors
                              .Where(i => i.ID == color.ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(color);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterColorUpdated(color);

            return color;
        }

        partial void OnColorDeleted(MCrossList.Server.Models.db.Color item);
        partial void OnAfterColorDeleted(MCrossList.Server.Models.db.Color item);

        public async Task<MCrossList.Server.Models.db.Color> DeleteColor(long id)
        {
            var itemToDelete = Context.Colors
                              .Where(i => i.ID == id)
                              .Include(i => i.Products)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnColorDeleted(itemToDelete);


            Context.Colors.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterColorDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportConditionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/conditions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/conditions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportConditionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/conditions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/conditions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnConditionsRead(ref IQueryable<MCrossList.Server.Models.db.Condition> items);

        public async Task<IQueryable<MCrossList.Server.Models.db.Condition>> GetConditions(Query query = null)
        {
            var items = Context.Conditions.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnConditionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnConditionGet(MCrossList.Server.Models.db.Condition item);
        partial void OnGetConditionById(ref IQueryable<MCrossList.Server.Models.db.Condition> items);


        public async Task<MCrossList.Server.Models.db.Condition> GetConditionById(long id)
        {
            var items = Context.Conditions
                              .AsNoTracking()
                              .Where(i => i.ID == id);

 
            OnGetConditionById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnConditionGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnConditionCreated(MCrossList.Server.Models.db.Condition item);
        partial void OnAfterConditionCreated(MCrossList.Server.Models.db.Condition item);

        public async Task<MCrossList.Server.Models.db.Condition> CreateCondition(MCrossList.Server.Models.db.Condition condition)
        {
            OnConditionCreated(condition);

            var existingItem = Context.Conditions
                              .Where(i => i.ID == condition.ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Conditions.Add(condition);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(condition).State = EntityState.Detached;
                throw;
            }

            OnAfterConditionCreated(condition);

            return condition;
        }

        public async Task<MCrossList.Server.Models.db.Condition> CancelConditionChanges(MCrossList.Server.Models.db.Condition item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnConditionUpdated(MCrossList.Server.Models.db.Condition item);
        partial void OnAfterConditionUpdated(MCrossList.Server.Models.db.Condition item);

        public async Task<MCrossList.Server.Models.db.Condition> UpdateCondition(long id, MCrossList.Server.Models.db.Condition condition)
        {
            OnConditionUpdated(condition);

            var itemToUpdate = Context.Conditions
                              .Where(i => i.ID == condition.ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(condition);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterConditionUpdated(condition);

            return condition;
        }

        partial void OnConditionDeleted(MCrossList.Server.Models.db.Condition item);
        partial void OnAfterConditionDeleted(MCrossList.Server.Models.db.Condition item);

        public async Task<MCrossList.Server.Models.db.Condition> DeleteCondition(long id)
        {
            var itemToDelete = Context.Conditions
                              .Where(i => i.ID == id)
                              .Include(i => i.Products)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnConditionDeleted(itemToDelete);


            Context.Conditions.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterConditionDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportMaterialsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/materials/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/materials/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportMaterialsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/materials/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/materials/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnMaterialsRead(ref IQueryable<MCrossList.Server.Models.db.Material> items);

        public async Task<IQueryable<MCrossList.Server.Models.db.Material>> GetMaterials(Query query = null)
        {
            var items = Context.Materials.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnMaterialsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMaterialGet(MCrossList.Server.Models.db.Material item);
        partial void OnGetMaterialById(ref IQueryable<MCrossList.Server.Models.db.Material> items);


        public async Task<MCrossList.Server.Models.db.Material> GetMaterialById(long id)
        {
            var items = Context.Materials
                              .AsNoTracking()
                              .Where(i => i.ID == id);

 
            OnGetMaterialById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnMaterialGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnMaterialCreated(MCrossList.Server.Models.db.Material item);
        partial void OnAfterMaterialCreated(MCrossList.Server.Models.db.Material item);

        public async Task<MCrossList.Server.Models.db.Material> CreateMaterial(MCrossList.Server.Models.db.Material material)
        {
            OnMaterialCreated(material);

            var existingItem = Context.Materials
                              .Where(i => i.ID == material.ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Materials.Add(material);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(material).State = EntityState.Detached;
                throw;
            }

            OnAfterMaterialCreated(material);

            return material;
        }

        public async Task<MCrossList.Server.Models.db.Material> CancelMaterialChanges(MCrossList.Server.Models.db.Material item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnMaterialUpdated(MCrossList.Server.Models.db.Material item);
        partial void OnAfterMaterialUpdated(MCrossList.Server.Models.db.Material item);

        public async Task<MCrossList.Server.Models.db.Material> UpdateMaterial(long id, MCrossList.Server.Models.db.Material material)
        {
            OnMaterialUpdated(material);

            var itemToUpdate = Context.Materials
                              .Where(i => i.ID == material.ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(material);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterMaterialUpdated(material);

            return material;
        }

        partial void OnMaterialDeleted(MCrossList.Server.Models.db.Material item);
        partial void OnAfterMaterialDeleted(MCrossList.Server.Models.db.Material item);

        public async Task<MCrossList.Server.Models.db.Material> DeleteMaterial(long id)
        {
            var itemToDelete = Context.Materials
                              .Where(i => i.ID == id)
                              .Include(i => i.Products)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnMaterialDeleted(itemToDelete);


            Context.Materials.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterMaterialDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportProductsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/products/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/products/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportProductsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/products/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/products/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnProductsRead(ref IQueryable<MCrossList.Server.Models.db.Product> items);

        public async Task<IQueryable<MCrossList.Server.Models.db.Product>> GetProducts(Query query = null)
        {
            var items = Context.Products.AsQueryable();

            items = items.Include(i => i.Product_Brand);
            items = items.Include(i => i.Product_Category);
            items = items.Include(i => i.Product_Color);
            items = items.Include(i => i.Product_Condition);
            items = items.Include(i => i.Product_Material);
            items = items.Include(i => i.Product_Size);
            items = items.Include(i => i.Product_Store);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnProductsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnProductGet(MCrossList.Server.Models.db.Product item);
        partial void OnGetProductById(ref IQueryable<MCrossList.Server.Models.db.Product> items);


        public async Task<MCrossList.Server.Models.db.Product> GetProductById(long id)
        {
            var items = Context.Products
                              .AsNoTracking()
                              .Where(i => i.ID == id);

            items = items.Include(i => i.Product_Brand);
            items = items.Include(i => i.Product_Category);
            items = items.Include(i => i.Product_Color);
            items = items.Include(i => i.Product_Condition);
            items = items.Include(i => i.Product_Material);
            items = items.Include(i => i.Product_Size);
            items = items.Include(i => i.Product_Store);
 
            OnGetProductById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnProductGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnProductCreated(MCrossList.Server.Models.db.Product item);
        partial void OnAfterProductCreated(MCrossList.Server.Models.db.Product item);

        public async Task<MCrossList.Server.Models.db.Product> CreateProduct(MCrossList.Server.Models.db.Product product)
        {
            OnProductCreated(product);

            var existingItem = Context.Products
                              .Where(i => i.ID == product.ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Products.Add(product);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(product).State = EntityState.Detached;
                throw;
            }

            OnAfterProductCreated(product);

            return product;
        }

        public async Task<MCrossList.Server.Models.db.Product> CancelProductChanges(MCrossList.Server.Models.db.Product item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnProductUpdated(MCrossList.Server.Models.db.Product item);
        partial void OnAfterProductUpdated(MCrossList.Server.Models.db.Product item);

        public async Task<MCrossList.Server.Models.db.Product> UpdateProduct(long id, MCrossList.Server.Models.db.Product product)
        {
            OnProductUpdated(product);

            var itemToUpdate = Context.Products
                              .Where(i => i.ID == product.ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(product);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterProductUpdated(product);

            return product;
        }

        partial void OnProductDeleted(MCrossList.Server.Models.db.Product item);
        partial void OnAfterProductDeleted(MCrossList.Server.Models.db.Product item);

        public async Task<MCrossList.Server.Models.db.Product> DeleteProduct(long id)
        {
            var itemToDelete = Context.Products
                              .Where(i => i.ID == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnProductDeleted(itemToDelete);


            Context.Products.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterProductDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSizesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/sizes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/sizes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSizesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/sizes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/sizes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSizesRead(ref IQueryable<MCrossList.Server.Models.db.Size> items);

        public async Task<IQueryable<MCrossList.Server.Models.db.Size>> GetSizes(Query query = null)
        {
            var items = Context.Sizes.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSizesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSizeGet(MCrossList.Server.Models.db.Size item);
        partial void OnGetSizeById(ref IQueryable<MCrossList.Server.Models.db.Size> items);


        public async Task<MCrossList.Server.Models.db.Size> GetSizeById(long id)
        {
            var items = Context.Sizes
                              .AsNoTracking()
                              .Where(i => i.ID == id);

 
            OnGetSizeById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSizeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSizeCreated(MCrossList.Server.Models.db.Size item);
        partial void OnAfterSizeCreated(MCrossList.Server.Models.db.Size item);

        public async Task<MCrossList.Server.Models.db.Size> CreateSize(MCrossList.Server.Models.db.Size size)
        {
            OnSizeCreated(size);

            var existingItem = Context.Sizes
                              .Where(i => i.ID == size.ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Sizes.Add(size);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(size).State = EntityState.Detached;
                throw;
            }

            OnAfterSizeCreated(size);

            return size;
        }

        public async Task<MCrossList.Server.Models.db.Size> CancelSizeChanges(MCrossList.Server.Models.db.Size item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSizeUpdated(MCrossList.Server.Models.db.Size item);
        partial void OnAfterSizeUpdated(MCrossList.Server.Models.db.Size item);

        public async Task<MCrossList.Server.Models.db.Size> UpdateSize(long id, MCrossList.Server.Models.db.Size size)
        {
            OnSizeUpdated(size);

            var itemToUpdate = Context.Sizes
                              .Where(i => i.ID == size.ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(size);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSizeUpdated(size);

            return size;
        }

        partial void OnSizeDeleted(MCrossList.Server.Models.db.Size item);
        partial void OnAfterSizeDeleted(MCrossList.Server.Models.db.Size item);

        public async Task<MCrossList.Server.Models.db.Size> DeleteSize(long id)
        {
            var itemToDelete = Context.Sizes
                              .Where(i => i.ID == id)
                              .Include(i => i.Products)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSizeDeleted(itemToDelete);


            Context.Sizes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSizeDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStoresToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/stores/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/stores/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStoresToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/db/stores/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/db/stores/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStoresRead(ref IQueryable<MCrossList.Server.Models.db.Store> items);

        public async Task<IQueryable<MCrossList.Server.Models.db.Store>> GetStores(Query query = null)
        {
            var items = Context.Stores.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStoresRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStoreGet(MCrossList.Server.Models.db.Store item);
        partial void OnGetStoreById(ref IQueryable<MCrossList.Server.Models.db.Store> items);


        public async Task<MCrossList.Server.Models.db.Store> GetStoreById(long id)
        {
            var items = Context.Stores
                              .AsNoTracking()
                              .Where(i => i.ID == id);

 
            OnGetStoreById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStoreGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStoreCreated(MCrossList.Server.Models.db.Store item);
        partial void OnAfterStoreCreated(MCrossList.Server.Models.db.Store item);

        public async Task<MCrossList.Server.Models.db.Store> CreateStore(MCrossList.Server.Models.db.Store store)
        {
            OnStoreCreated(store);

            var existingItem = Context.Stores
                              .Where(i => i.ID == store.ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Stores.Add(store);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(store).State = EntityState.Detached;
                throw;
            }

            OnAfterStoreCreated(store);

            return store;
        }

        public async Task<MCrossList.Server.Models.db.Store> CancelStoreChanges(MCrossList.Server.Models.db.Store item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStoreUpdated(MCrossList.Server.Models.db.Store item);
        partial void OnAfterStoreUpdated(MCrossList.Server.Models.db.Store item);

        public async Task<MCrossList.Server.Models.db.Store> UpdateStore(long id, MCrossList.Server.Models.db.Store store)
        {
            OnStoreUpdated(store);

            var itemToUpdate = Context.Stores
                              .Where(i => i.ID == store.ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(store);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStoreUpdated(store);

            return store;
        }

        partial void OnStoreDeleted(MCrossList.Server.Models.db.Store item);
        partial void OnAfterStoreDeleted(MCrossList.Server.Models.db.Store item);

        public async Task<MCrossList.Server.Models.db.Store> DeleteStore(long id)
        {
            var itemToDelete = Context.Stores
                              .Where(i => i.ID == id)
                              .Include(i => i.Products)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStoreDeleted(itemToDelete);


            Context.Stores.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStoreDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}