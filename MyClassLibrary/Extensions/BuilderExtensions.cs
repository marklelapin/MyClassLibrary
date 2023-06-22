using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace MyClassLibrary.Extensions
{
    public static class BuilderExtensions
    {
        /// <summary>
        /// Configures Authentication using AzureAdB2C.
        /// </summary>
        /// <remarks>
        /// 1. Configures Cookie Options <br/>
        /// 2. Adds Authentication with configuration coming from appsettings "AzureAdB2C" <br/>
        /// 3. Sets up controllers and Microsoft Identity UI for login pages.<br/>
        /// 
        /// The following is an example of the appsettings.json file configuration need for this to work. <br/>
        /// "AzureAdB2C": { <br/>
        /// "Instance": "https://thewhaddonentertainers.b2clogin.com", <br/>
        /// "Domain": "thewhaddonentertainers.onmicrosoft.com", <br/>
        /// "ClientId": "ccd332a0-fght-4d09-865b-f5ab0a1aab18",    _______      (This is a mock ClientId. This needs to come from Azure AdB2c portal.) <br/>
        /// "ClientSecret": "sdfff~Ydffd30El.k3_30wrHMfdfdffdfe0bYf",_______    (similarly client secret is a mock secret.) <br/>
        /// "SignedOutCallbackPath": "/signout/B2C_1_susi", <br/>
        /// "SignUpSignInPolicyId": "B2C_1_susi", <br/>
        /// "ResetPasswordPolicyId": "B2C_1_reset", <br/>
        /// "EditProfilePolicyId": "B2C_1_edit" <br/>
        /// } 
        /// <br/>
        /// 
        /// You will aslo need to add to Program.cs <br/>
        ///
        ///  app.UseCookiePolicy(); (above UseRouting) <br/>
        ///    app.UseAuthorization(); (above UseAuthentication)
        /// </remarks>
        public static void ConfigureMicrosoftIdentityWebAuthenticationAndUI(this WebApplicationBuilder builder,string identityConfigurationSection)
        {
           
            //configures cookie options
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.HandleSameSiteCookieCompatibility();
            });

            //Configure authentication using AzureAdB2C and set this as the fallback policy
            builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection(identityConfigurationSection))
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddInMemoryTokenCaches();

           builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = options.DefaultPolicy;
            });

            //Setups up UI for accessing and loging into AzureAdB2c 
            builder.Services.AddControllersWithViews()
            .AddMicrosoftIdentityUI();
        }


        public static void ConfigureWebAPIAuthentication_AzureAdB2C(this WebApplicationBuilder builder)
        {
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApi(options =>
                                                    {
                            builder.Configuration.Bind("AzureAdB2C", options);

                            options.TokenValidationParameters.NameClaimType = "name";
                        },
                                                   options =>
                                                   {
                                                       builder.Configuration.Bind("AzureAdB2C", options);
                                                   });
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
    
    
        /// <summary>
        /// Sets the fallback policy to RequireAuthenticatedUser.
        /// </summary>
        /// <param name="builder"></param>
        public static void RequireAuthorizationThroughoutAsFallbackPolicy(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
        }

    
    }
}
