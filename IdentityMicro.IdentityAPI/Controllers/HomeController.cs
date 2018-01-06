using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using IdentityServer4.Services;

using IdentityMicro.IdentityAPI;
using IdentityMicro.IdentityAPI.ViewModels;

namespace IdentityMicro.IdentityAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
      private readonly IIdentityServerInteractionService _interaction;

        public HomeController(IIdentityServerInteractionService interaction)
        {
            //_interaction = interaction;
        }
        [HttpGet("/index")]
        public bool Index()
        {
            return true;
        }

        [HttpGet("/about")]
        public IActionResult About()
        {


            return new JsonResult(new { value = "success" });
        }

        //[HttpGet]
        //public IActionResult Contact()
        //{
        //    return new JsonResult(new { value = "Contact Page" });
        //}

        /// <summary>
        /// Shows the error page
        /// </summary>
        //public async Task<IActionResult> Error(string errorId)
        //{
        //    var vm = new ErrorViewModel();

        //    // retrieve error details from identityserver
        //    var message = await _interaction.GetErrorContextAsync(errorId);
        //    if (message != null)
        //    {
        //        vm.Error = message;
        //    }

        //    return View("Error", vm);
        //}
    }
}
