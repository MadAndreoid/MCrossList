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
    [Route("odata/db/Materials")]
    public partial class MaterialsController : ODataController
    {
        private MCrossList.Server.Data.dbContext context;

        public MaterialsController(MCrossList.Server.Data.dbContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<MCrossList.Server.Models.db.Material> GetMaterials()
        {
            var items = this.context.Materials.AsQueryable<MCrossList.Server.Models.db.Material>();
            this.OnMaterialsRead(ref items);

            return items;
        }

        partial void OnMaterialsRead(ref IQueryable<MCrossList.Server.Models.db.Material> items);

        partial void OnMaterialGet(ref SingleResult<MCrossList.Server.Models.db.Material> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/db/Materials(ID={ID})")]
        public SingleResult<MCrossList.Server.Models.db.Material> GetMaterial(long key)
        {
            var items = this.context.Materials.Where(i => i.ID == key);
            var result = SingleResult.Create(items);

            OnMaterialGet(ref result);

            return result;
        }
        partial void OnMaterialDeleted(MCrossList.Server.Models.db.Material item);
        partial void OnAfterMaterialDeleted(MCrossList.Server.Models.db.Material item);

        [HttpDelete("/odata/db/Materials(ID={ID})")]
        public IActionResult DeleteMaterial(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Materials
                    .Where(i => i.ID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnMaterialDeleted(item);
                this.context.Materials.Remove(item);
                this.context.SaveChanges();
                this.OnAfterMaterialDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMaterialUpdated(MCrossList.Server.Models.db.Material item);
        partial void OnAfterMaterialUpdated(MCrossList.Server.Models.db.Material item);

        [HttpPut("/odata/db/Materials(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutMaterial(long key, [FromBody]MCrossList.Server.Models.db.Material item)
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
                this.OnMaterialUpdated(item);
                this.context.Materials.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Materials.Where(i => i.ID == key);
                
                this.OnAfterMaterialUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/db/Materials(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchMaterial(long key, [FromBody]Delta<MCrossList.Server.Models.db.Material> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Materials.Where(i => i.ID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnMaterialUpdated(item);
                this.context.Materials.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Materials.Where(i => i.ID == key);
                
                this.OnAfterMaterialUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMaterialCreated(MCrossList.Server.Models.db.Material item);
        partial void OnAfterMaterialCreated(MCrossList.Server.Models.db.Material item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] MCrossList.Server.Models.db.Material item)
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

                this.OnMaterialCreated(item);
                this.context.Materials.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Materials.Where(i => i.ID == item.ID);

                

                this.OnAfterMaterialCreated(item);

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
