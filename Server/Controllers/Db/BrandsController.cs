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
    [Route("odata/db/Brands")]
    public partial class BrandsController : ODataController
    {
        private MCrossList.Server.Data.dbContext context;

        public BrandsController(MCrossList.Server.Data.dbContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<MCrossList.Server.Models.db.Brand> GetBrands()
        {
            var items = this.context.Brands.AsQueryable<MCrossList.Server.Models.db.Brand>();
            this.OnBrandsRead(ref items);

            return items;
        }

        partial void OnBrandsRead(ref IQueryable<MCrossList.Server.Models.db.Brand> items);

        partial void OnBrandGet(ref SingleResult<MCrossList.Server.Models.db.Brand> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/db/Brands(ID={ID})")]
        public SingleResult<MCrossList.Server.Models.db.Brand> GetBrand(long key)
        {
            var items = this.context.Brands.Where(i => i.ID == key);
            var result = SingleResult.Create(items);

            OnBrandGet(ref result);

            return result;
        }
        partial void OnBrandDeleted(MCrossList.Server.Models.db.Brand item);
        partial void OnAfterBrandDeleted(MCrossList.Server.Models.db.Brand item);

        [HttpDelete("/odata/db/Brands(ID={ID})")]
        public IActionResult DeleteBrand(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Brands
                    .Where(i => i.ID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnBrandDeleted(item);
                this.context.Brands.Remove(item);
                this.context.SaveChanges();
                this.OnAfterBrandDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnBrandUpdated(MCrossList.Server.Models.db.Brand item);
        partial void OnAfterBrandUpdated(MCrossList.Server.Models.db.Brand item);

        [HttpPut("/odata/db/Brands(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutBrand(long key, [FromBody]MCrossList.Server.Models.db.Brand item)
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
                this.OnBrandUpdated(item);
                this.context.Brands.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Brands.Where(i => i.ID == key);
                
                this.OnAfterBrandUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/db/Brands(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchBrand(long key, [FromBody]Delta<MCrossList.Server.Models.db.Brand> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Brands.Where(i => i.ID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnBrandUpdated(item);
                this.context.Brands.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Brands.Where(i => i.ID == key);
                
                this.OnAfterBrandUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnBrandCreated(MCrossList.Server.Models.db.Brand item);
        partial void OnAfterBrandCreated(MCrossList.Server.Models.db.Brand item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] MCrossList.Server.Models.db.Brand item)
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

                this.OnBrandCreated(item);
                this.context.Brands.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Brands.Where(i => i.ID == item.ID);

                

                this.OnAfterBrandCreated(item);

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
