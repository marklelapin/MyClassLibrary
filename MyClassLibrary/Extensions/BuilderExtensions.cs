using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MyClassLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Extensions
{
    public static class BuilderExtensions
    {

        public static void ConfigureAuthentication_AzureAdB2C(this WebApplicationBuilder builder)
        {
            
            //configures cookie options
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.HandleSameSiteCookieCompatibility();
            });

            //Configure authentication using AzureAdB2C and set this as the fallback policy
            builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "AzureAdB2C");
            
            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = options.DefaultPolicy;
            });
            

            //Setups up UI for accessing and loging into AzureAdB2c 
            builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();

            builder.Services.AddOptions();

            builder.Services.Configure<OpenIdConnectOptions>(builder.Configuration.GetSection("AzureAdB2C"));


            


           //TODO Add in example of appsettings setup to builder extension





            //ADD to Program.cs

            // app.UseCookiePolicy(); (above UseRouting)
            // app.UseAuthorization(); (above UseAuthentication)


        }
    
        /// <summary>
        /// This will authenticate all authorizations as 'passed' if in the Development Environment.
        /// </summary>
        public static void ByPassAuthenticationIfInDevelopment(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddSingleton<IAuthorizationHandler, ByPassAuthorization>(); //ByPassess all authorisation by approving everything.
            }
        }

        private class ByPassAuthorization : IAuthorizationHandler
        {
            public Task HandleAsync(AuthorizationHandlerContext context)
            {
                foreach (IAuthorizationRequirement requirement in context.PendingRequirements.ToList())
                {
                    context.Succeed(requirement); //passes all requirements
                }
                return Task.CompletedTask;
            }
        }
    }
}
