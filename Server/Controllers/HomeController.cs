using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCoreJwtAuthorization.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }
        public IActionResult Authenticate()
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, "something"),
                new Claim("MyClaim", "MyClaimValue"),
            };

            var secretPhraseBytes = Encoding.UTF8.GetBytes(MyConstants.SecretPhrase);
            var key = new SymmetricSecurityKey(secretPhraseBytes);
            var algorithm = SecurityAlgorithms.HmacSha256; //or any

            var signinCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                    MyConstants.Issuer,
                    MyConstants.Audiance,
                    claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(1),
                    signinCredentials
                );

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { my_access_token = tokenJson });
        }
    }
}
