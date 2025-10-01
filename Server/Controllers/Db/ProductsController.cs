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
    [Route("odata/db/Products")]
    public partial class ProductsController : ODataController
    {
        private MCrossList.Server.Data.dbContext context;

        public ProductsController(MCrossList.Server.Data.dbContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<MCrossList.Server.Models.db.Product> GetProducts()
        {
            var items = this.context.Products.AsQueryable<MCrossList.Server.Models.db.Product>();
            this.OnProductsRead(ref items);

            return items;
        }

        partial void OnProductsRead(ref IQueryable<MCrossList.Server.Models.db.Product> items);

        partial void OnProductGet(ref SingleResult<MCrossList.Server.Models.db.Product> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/db/Products(ID={ID})")]
        public SingleResult<MCrossList.Server.Models.db.Product> GetProduct(long key)
        {
            var items = this.context.Products.Where(i => i.ID == key);
            var result = SingleResult.Create(items);

            OnProductGet(ref result);

            return result;
        }
        partial void OnProductDeleted(MCrossList.Server.Models.db.Product item);
        partial void OnAfterProductDeleted(MCrossList.Server.Models.db.Product item);

        [HttpDelete("/odata/db/Products(ID={ID})")]
        public IActionResult DeleteProduct(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Products
                    .Where(i => i.ID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnProductDeleted(item);
                this.context.Products.Remove(item);
                this.context.SaveChanges();
                this.OnAfterProductDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnProductUpdated(MCrossList.Server.Models.db.Product item);
        partial void OnAfterProductUpdated(MCrossList.Server.Models.db.Product item);

        [HttpPut("/odata/db/Products(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutProduct(long key, [FromBody]MCrossList.Server.Models.db.Product item)
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
                this.OnProductUpdated(item);
                this.context.Products.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Products.Where(i => i.ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Product_Brand,Product_Category,Product_Color,Product_Condition,Product_Material,Product_Size,Product_Store");
                this.OnAfterProductUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/db/Products(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchProduct(long key, [FromBody]Delta<MCrossList.Server.Models.db.Product> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Products.Where(i => i.ID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnProductUpdated(item);
                this.context.Products.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Products.Where(i => i.ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Product_Brand,Product_Category,Product_Color,Product_Condition,Product_Material,Product_Size,Product_Store");
                this.OnAfterProductUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnProductCreated(MCrossList.Server.Models.db.Product item);
        partial void OnAfterProductCreated(MCrossList.Server.Models.db.Product item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] MCrossList.Server.Models.db.Product item)
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

                this.OnProductCreated(item);
                this.context.Products.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Products.Where(i => i.ID == item.ID);

                Request.QueryString = Request.QueryString.Add("$expand", "Product_Brand,Product_Category,Product_Color,Product_Condition,Product_Material,Product_Size,Product_Store");

                this.OnAfterProductCreated(item);

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
