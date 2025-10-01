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
    [Route("odata/db/Conditions")]
    public partial class ConditionsController : ODataController
    {
        private MCrossList.Server.Data.dbContext context;

        public ConditionsController(MCrossList.Server.Data.dbContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<MCrossList.Server.Models.db.Condition> GetConditions()
        {
            var items = this.context.Conditions.AsQueryable<MCrossList.Server.Models.db.Condition>();
            this.OnConditionsRead(ref items);

            return items;
        }

        partial void OnConditionsRead(ref IQueryable<MCrossList.Server.Models.db.Condition> items);

        partial void OnConditionGet(ref SingleResult<MCrossList.Server.Models.db.Condition> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/db/Conditions(ID={ID})")]
        public SingleResult<MCrossList.Server.Models.db.Condition> GetCondition(long key)
        {
            var items = this.context.Conditions.Where(i => i.ID == key);
            var result = SingleResult.Create(items);

            OnConditionGet(ref result);

            return result;
        }
        partial void OnConditionDeleted(MCrossList.Server.Models.db.Condition item);
        partial void OnAfterConditionDeleted(MCrossList.Server.Models.db.Condition item);

        [HttpDelete("/odata/db/Conditions(ID={ID})")]
        public IActionResult DeleteCondition(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Conditions
                    .Where(i => i.ID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnConditionDeleted(item);
                this.context.Conditions.Remove(item);
                this.context.SaveChanges();
                this.OnAfterConditionDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnConditionUpdated(MCrossList.Server.Models.db.Condition item);
        partial void OnAfterConditionUpdated(MCrossList.Server.Models.db.Condition item);

        [HttpPut("/odata/db/Conditions(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutCondition(long key, [FromBody]MCrossList.Server.Models.db.Condition item)
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
                this.OnConditionUpdated(item);
                this.context.Conditions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Conditions.Where(i => i.ID == key);
                
                this.OnAfterConditionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/db/Conditions(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchCondition(long key, [FromBody]Delta<MCrossList.Server.Models.db.Condition> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Conditions.Where(i => i.ID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnConditionUpdated(item);
                this.context.Conditions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Conditions.Where(i => i.ID == key);
                
                this.OnAfterConditionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnConditionCreated(MCrossList.Server.Models.db.Condition item);
        partial void OnAfterConditionCreated(MCrossList.Server.Models.db.Condition item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] MCrossList.Server.Models.db.Condition item)
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

                this.OnConditionCreated(item);
                this.context.Conditions.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Conditions.Where(i => i.ID == item.ID);

                

                this.OnAfterConditionCreated(item);

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
