using System;
using System.Net;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MCrossList.Server.Controllers.db
{
    [Route("odata/db/Categories")]
    public partial class CategoriesController : ODataController
    {
        private MCrossList.Server.Data.dbContext context;

        public CategoriesController(MCrossList.Server.Data.dbContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<MCrossList.Server.Models.db.Category> GetCategories()
        {
            var items = this.context.Categories.AsQueryable<MCrossList.Server.Models.db.Category>();
            this.OnCategoriesRead(ref items);

            return items;
        }

        partial void OnCategoriesRead(ref IQueryable<MCrossList.Server.Models.db.Category> items);

        partial void OnCategoryGet(ref SingleResult<MCrossList.Server.Models.db.Category> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/db/Categories(ID={ID})")]
        public SingleResult<MCrossList.Server.Models.db.Category> GetCategory(long key)
        {
            var items = this.context.Categories.Where(i => i.ID == key);
            var result = SingleResult.Create(items);

            OnCategoryGet(ref result);

            return result;
        }
        partial void OnCategoryDeleted(MCrossList.Server.Models.db.Category item);
        partial void OnAfterCategoryDeleted(MCrossList.Server.Models.db.Category item);

        [HttpDelete("/odata/db/Categories(ID={ID})")]
        public IActionResult DeleteCategory(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Categories
                    .Where(i => i.ID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnCategoryDeleted(item);
                this.context.Categories.Remove(item);
                this.context.SaveChanges();
                this.OnAfterCategoryDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCategoryUpdated(MCrossList.Server.Models.db.Category item);
        partial void OnAfterCategoryUpdated(MCrossList.Server.Models.db.Category item);

        [HttpPut("/odata/db/Categories(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutCategory(long key, [FromBody]MCrossList.Server.Models.db.Category item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.ID != key))
                {
                    return BadRequest();
                }
                this.OnCategoryUpdated(item);
                this.context.Categories.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Categories.Where(i => i.ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Category_Father");
                this.OnAfterCategoryUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/db/Categories(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchCategory(long key, [FromBody]Delta<MCrossList.Server.Models.db.Category> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Categories.Where(i => i.ID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnCategoryUpdated(item);
                this.context.Categories.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Categories.Where(i => i.ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Category_Father");
                this.OnAfterCategoryUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCategoryCreated(MCrossList.Server.Models.db.Category item);
        partial void OnAfterCategoryCreated(MCrossList.Server.Models.db.Category item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] MCrossList.Server.Models.db.Category item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnCategoryCreated(item);
                this.context.Categories.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Categories.Where(i => i.ID == item.ID);

                Request.QueryString = Request.QueryString.Add("$expand", "Category_Father");

                this.OnAfterCategoryCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
