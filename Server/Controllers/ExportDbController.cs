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
    }
}
