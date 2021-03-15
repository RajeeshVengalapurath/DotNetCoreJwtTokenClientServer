using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet]
        public IActionResult Authorize()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Authorize(string username)
        {
            return View();
        }
        public IActionResult Token()
        {
            return View();
        }
    }
}
