using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Configuration
{
    /// <summary>
    /// ClientCredentialsConfiguration for AzureAd
    /// </summary>
    public class AzureAdClientCredentialsConfiguration
    {
        /// <summary>
        /// Endpoint for token aquisitiong = "https://login.microsoftonline.com/yourtenantid/oauth2/v2.0/token";
        /// </summary>
        public string RequestUri = "";
        /// <summary>
        /// Default scope for token aquisition = ApplicationIDURI+/.default e.g : "api://clientId/.default";
        /// </summary>
        public string Scope = "";
        /// <summary>
        /// ClientId from app registration
        /// </summary>
        public string ClientId = "";
        /// <summary>
        /// ClientSecret from app registration
        /// </summary>
        public string ClientSecret = "";


        /// <summary>
        /// Constructor for AzureAdClientCredentialsConfiguration defaulting to "AzureAd:" configuration section
        /// </summary>
        /// <remarks>
        /// requires the following configuration settings:
        /// AzureAd:RequestUri
        /// AzureAd:Scope
        /// AzureAd:ClientId
        /// AzureAd:ClientCredentials:0:ClientSecret
        /// 
        /// </remarks>
        /// <param name="configuration"></param>
        /// <param name="configurationSection"></param>
        public AzureAdClientCredentialsConfiguration(IConfiguration configuration, string configurationSection="AzureAd:")
        {
            RequestUri = configuration.GetValue<string>(configurationSection + "RequestUri");
            Scope = configuration.GetValue<string>(configurationSection + "Scope");
            ClientId = configuration.GetValue<string>(configurationSection + "ClientId");
            ClientSecret = configuration.GetValue<string>(configurationSection + "ClientCredentials:0:ClientSecret");
        }

    }
}
