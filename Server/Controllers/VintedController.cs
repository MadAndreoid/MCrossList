using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MCrossList.Server.Models;
using MCrossList.Server.Services;
using DocumentFormat.OpenXml.Office2016.Drawing.Command;

namespace MCrossList.Server.Controllers
{
    [Route("Vinted/[action]")]
    public partial class VintedController : Controller
    {
        private readonly VintedBackService vintedBackService;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration configuration;

        public VintedController(IWebHostEnvironment env,IConfiguration configuration, VintedBackService vintedBackService)
        {
            this.env = env;
            this.configuration = configuration;
            this.vintedBackService = vintedBackService;
        }

        private IActionResult RedirectWithError(string error, string redirectUrl = null)
        {
             if (!string.IsNullOrEmpty(redirectUrl))
             {
                 return Redirect($"~/Login?error={error}&redirectUrl={Uri.EscapeDataString(redirectUrl.Replace("~", ""))}");
             }
             else
             {
                 return Redirect($"~/Login?error={error}");
             }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProductsNumber()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var x = await vintedBackService.GetProductsDetails();
                    return Ok(x);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }

        }

        public async Task<IActionResult> Logout()
        {
            //await signInManager.SignOutAsync();

            return Redirect("~/");
        }
    }
}
