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
    [Route("odata/db/Stores")]
    public partial class StoresController : ODataController
    {
        private MCrossList.Server.Data.dbContext context;

        public StoresController(MCrossList.Server.Data.dbContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<MCrossList.Server.Models.db.Store> GetStores()
        {
            var items = this.context.Stores.AsQueryable<MCrossList.Server.Models.db.Store>();
            this.OnStoresRead(ref items);

            return items;
        }

        partial void OnStoresRead(ref IQueryable<MCrossList.Server.Models.db.Store> items);

        partial void OnStoreGet(ref SingleResult<MCrossList.Server.Models.db.Store> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/db/Stores(ID={ID})")]
        public SingleResult<MCrossList.Server.Models.db.Store> GetStore(long key)
        {
            var items = this.context.Stores.Where(i => i.ID == key);
            var result = SingleResult.Create(items);

            OnStoreGet(ref result);

            return result;
        }
        partial void OnStoreDeleted(MCrossList.Server.Models.db.Store item);
        partial void OnAfterStoreDeleted(MCrossList.Server.Models.db.Store item);

        [HttpDelete("/odata/db/Stores(ID={ID})")]
        public IActionResult DeleteStore(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Stores
                    .Where(i => i.ID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnStoreDeleted(item);
                this.context.Stores.Remove(item);
                this.context.SaveChanges();
                this.OnAfterStoreDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStoreUpdated(MCrossList.Server.Models.db.Store item);
        partial void OnAfterStoreUpdated(MCrossList.Server.Models.db.Store item);

        [HttpPut("/odata/db/Stores(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutStore(long key, [FromBody]MCrossList.Server.Models.db.Store item)
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
                this.OnStoreUpdated(item);
                this.context.Stores.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Stores.Where(i => i.ID == key);
                
                this.OnAfterStoreUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/db/Stores(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchStore(long key, [FromBody]Delta<MCrossList.Server.Models.db.Store> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Stores.Where(i => i.ID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnStoreUpdated(item);
                this.context.Stores.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Stores.Where(i => i.ID == key);
                
                this.OnAfterStoreUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStoreCreated(MCrossList.Server.Models.db.Store item);
        partial void OnAfterStoreCreated(MCrossList.Server.Models.db.Store item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] MCrossList.Server.Models.db.Store item)
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

                this.OnStoreCreated(item);
                this.context.Stores.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Stores.Where(i => i.ID == item.ID);

                

                this.OnAfterStoreCreated(item);

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
