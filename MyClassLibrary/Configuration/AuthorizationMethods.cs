using Microsoft.Extensions.Configuration;
using MyClassLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace MyClassLibrary.Configuration
{
    /// <summary>
    /// Methods for obtaining and refreshing authorization tokens
    /// </summary>
    public class AuthorizationMethods : IAuthorizationMethods
    {
        /// <summary>
        /// Refreshes the authorization token for a specific HttpClient using the client credentials flow
        /// </summary>
        /// <param name="client"></param>
        /// <param name="azureAdConfig"></param>
        /// <returns></returns>
        public async Task<bool> RefreshAuthorizationHeader(HttpClient client, AzureAdClientCredentialsConfiguration azureAdConfig)
        {
            try
            {
                string token = await GetAuthorizationToken(azureAdConfig);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error refreshing authorization header", ex);
            }


        }

        /// <summary>
        /// Refreshes the authorization token for a specifc HttpClient periodically
        /// </summary>
        /// <param name="client"></param>
        /// <param name="minutes"></param>
        /// <param name="azureAdConfig"></param>
        public void RefreshAuthorizationHeaderPeriodically(HttpClient client, int minutes, AzureAdClientCredentialsConfiguration azureAdConfig)
        {

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromMinutes(minutes));
                    await RefreshAuthorizationHeader(client, azureAdConfig);
                }
            });
        }

        /// <summary>
        /// Obtains an authorization token from AzureAd using the client credentials flow
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<string> GetAuthorizationToken(AzureAdClientCredentialsConfiguration config)
        {
            try
            {
                var client = new HttpClient();

                var request = new HttpRequestMessage(
                                        HttpMethod.Post
                                        , config.RequestUri);

                var collection = new List<KeyValuePair<string, string>>();

                collection.Add(new("grant_type", "client_credentials"));
                collection.Add(new("scope", config.Scope));
                collection.Add(new("client_id", config.ClientId));
                collection.Add(new("client_secret", config.ClientSecret));

                var content = new FormUrlEncodedContent(collection);
                request.Content = content;

                using var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string authenticationResponse = await response.Content.ReadAsStringAsync();

                string token = (string)JsonObject.Parse(authenticationResponse)?["access_token"]!;

                return token;

            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining token from AzureAd", ex);
            }

        }



    }
}
