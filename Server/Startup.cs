using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCoreJwtAuthorization
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("OAuth")
                .AddJwtBearer("OAuth", config =>
                {
                    var secretPhraseBytes = Encoding.UTF8.GetBytes(MyConstants.SecretPhrase);
                    var key = new SymmetricSecurityKey(secretPhraseBytes);

                    //This will help to pass token as a url query parameter
                    //https://localhost:44304/Home/Secret?my_access_token=eyJhbGciOiJIUzI.....KWbjTVnVbIzYqE
                    config.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Query.ContainsKey("my_access_token"))
                                context.Token = context.Request.Query["my_access_token"];
                            return Task.CompletedTask;
                        }
                    };

                    //To verify, Get token, access secret action method using postman with header key as Authorization and value as Bearer<space>token base 64 value
                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = key,
                        ValidIssuer = MyConstants.Issuer,
                        ValidAudience = MyConstants.Audiance
                    };
                });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
