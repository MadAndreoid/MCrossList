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
    [Route("odata/db/Sizes")]
    public partial class SizesController : ODataController
    {
        private MCrossList.Server.Data.dbContext context;

        public SizesController(MCrossList.Server.Data.dbContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<MCrossList.Server.Models.db.Size> GetSizes()
        {
            var items = this.context.Sizes.AsQueryable<MCrossList.Server.Models.db.Size>();
            this.OnSizesRead(ref items);

            return items;
        }

        partial void OnSizesRead(ref IQueryable<MCrossList.Server.Models.db.Size> items);

        partial void OnSizeGet(ref SingleResult<MCrossList.Server.Models.db.Size> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/db/Sizes(ID={ID})")]
        public SingleResult<MCrossList.Server.Models.db.Size> GetSize(long key)
        {
            var items = this.context.Sizes.Where(i => i.ID == key);
            var result = SingleResult.Create(items);

            OnSizeGet(ref result);

            return result;
        }
        partial void OnSizeDeleted(MCrossList.Server.Models.db.Size item);
        partial void OnAfterSizeDeleted(MCrossList.Server.Models.db.Size item);

        [HttpDelete("/odata/db/Sizes(ID={ID})")]
        public IActionResult DeleteSize(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Sizes
                    .Where(i => i.ID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnSizeDeleted(item);
                this.context.Sizes.Remove(item);
                this.context.SaveChanges();
                this.OnAfterSizeDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSizeUpdated(MCrossList.Server.Models.db.Size item);
        partial void OnAfterSizeUpdated(MCrossList.Server.Models.db.Size item);

        [HttpPut("/odata/db/Sizes(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSize(long key, [FromBody]MCrossList.Server.Models.db.Size item)
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
                this.OnSizeUpdated(item);
                this.context.Sizes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Sizes.Where(i => i.ID == key);
                
                this.OnAfterSizeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/db/Sizes(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchSize(long key, [FromBody]Delta<MCrossList.Server.Models.db.Size> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Sizes.Where(i => i.ID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnSizeUpdated(item);
                this.context.Sizes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Sizes.Where(i => i.ID == key);
                
                this.OnAfterSizeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSizeCreated(MCrossList.Server.Models.db.Size item);
        partial void OnAfterSizeCreated(MCrossList.Server.Models.db.Size item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] MCrossList.Server.Models.db.Size item)
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

                this.OnSizeCreated(item);
                this.context.Sizes.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Sizes.Where(i => i.ID == item.ID);

                

                this.OnAfterSizeCreated(item);

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
