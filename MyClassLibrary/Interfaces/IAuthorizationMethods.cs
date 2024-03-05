using MyClassLibrary.Configuration;

namespace MyClassLibrary.Interfaces
{
    public interface IAuthorizationMethods
    {
        Task<string> GetAuthorizationToken(AzureAdClientCredentialsConfiguration config);
        Task<bool> RefreshAuthorizationHeader(HttpClient client, AzureAdClientCredentialsConfiguration azureAdConfig);
        void RefreshAuthorizationHeaderPeriodically(HttpClient client, int minutes, AzureAdClientCredentialsConfiguration azureAdConfig);
    }
}