using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MyClassLibrary.Interfaces;

namespace MyClassLibrary.Configuration
{
    public class AuthorizationHeaderHandler : DelegatingHandler
    {

        private readonly AzureAdClientCredentialsConfiguration _azureAdConfig;
        private readonly IAuthorizationMethods _authorizationMethods;

        public AuthorizationHeaderHandler (IAuthorizationMethods authorizationMethods,IConfiguration configuration)
        {
            _authorizationMethods = authorizationMethods;
            _azureAdConfig = new AzureAdClientCredentialsConfiguration(configuration);
        
        }

        /// <summary>
        /// overrides the SendAsync method of Delegation Handler to add the authorization header to the request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string token = await _authorizationMethods.GetAuthorizationToken(_azureAdConfig);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
