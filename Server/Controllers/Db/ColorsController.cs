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
    [Route("odata/db/Colors")]
    public partial class ColorsController : ODataController
    {
        private MCrossList.Server.Data.dbContext context;

        public ColorsController(MCrossList.Server.Data.dbContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<MCrossList.Server.Models.db.Color> GetColors()
        {
            var items = this.context.Colors.AsQueryable<MCrossList.Server.Models.db.Color>();
            this.OnColorsRead(ref items);

            return items;
        }

        partial void OnColorsRead(ref IQueryable<MCrossList.Server.Models.db.Color> items);

        partial void OnColorGet(ref SingleResult<MCrossList.Server.Models.db.Color> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/db/Colors(ID={ID})")]
        public SingleResult<MCrossList.Server.Models.db.Color> GetColor(long key)
        {
            var items = this.context.Colors.Where(i => i.ID == key);
            var result = SingleResult.Create(items);

            OnColorGet(ref result);

            return result;
        }
        partial void OnColorDeleted(MCrossList.Server.Models.db.Color item);
        partial void OnAfterColorDeleted(MCrossList.Server.Models.db.Color item);

        [HttpDelete("/odata/db/Colors(ID={ID})")]
        public IActionResult DeleteColor(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Colors
                    .Where(i => i.ID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnColorDeleted(item);
                this.context.Colors.Remove(item);
                this.context.SaveChanges();
                this.OnAfterColorDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnColorUpdated(MCrossList.Server.Models.db.Color item);
        partial void OnAfterColorUpdated(MCrossList.Server.Models.db.Color item);

        [HttpPut("/odata/db/Colors(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutColor(long key, [FromBody]MCrossList.Server.Models.db.Color item)
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
                this.OnColorUpdated(item);
                this.context.Colors.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Colors.Where(i => i.ID == key);
                
                this.OnAfterColorUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/db/Colors(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchColor(long key, [FromBody]Delta<MCrossList.Server.Models.db.Color> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Colors.Where(i => i.ID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnColorUpdated(item);
                this.context.Colors.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Colors.Where(i => i.ID == key);
                
                this.OnAfterColorUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnColorCreated(MCrossList.Server.Models.db.Color item);
        partial void OnAfterColorCreated(MCrossList.Server.Models.db.Color item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] MCrossList.Server.Models.db.Color item)
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

                this.OnColorCreated(item);
                this.context.Colors.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Colors.Where(i => i.ID == item.ID);

                

                this.OnAfterColorCreated(item);

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
