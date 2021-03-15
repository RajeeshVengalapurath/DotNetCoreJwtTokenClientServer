using DotNetCoreJwtAuthorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet]
        public IActionResult Authorize(string response_type, string client_id, string redirect_uri, string scope, string state)
        {
            //?a=xyz&b=abc
            var query = new QueryBuilder();
            query.Add("redirect_uri", redirect_uri);
            query.Add("state", state);

            return View(model: query.ToString());
        }
        [HttpPost]
        public IActionResult Authorize(string username, string redirect_uri, string state)
        {
            const string code = "BLAHBLAH";

            var query = new QueryBuilder();
            query.Add("code", code);
            query.Add("state", state);

            return Redirect($"{redirect_uri}{query.ToString()}");
        }
        public async Task<IActionResult> Token(
            string grand_type,
            string code, //confirmation of the authentication process
            string redirect_url,
            string client_id
            )
        {
            //some mechanism to validate the code here

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

            var access_token = new JwtSecurityTokenHandler().WriteToken(token);

            var responseObject = new
            {
                access_token,
                token_type = "Bearer",
                raw_claim = "oauthTutorial"
            };

            var responseJson = JsonConvert.SerializeObject(responseObject);
            var responseBytes = Encoding.UTF8.GetBytes(responseJson);

            await Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);
            return View();
        }
    }
}
