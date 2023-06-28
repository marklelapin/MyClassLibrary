using Amazon.Runtime.Internal.Transform;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver.Core.Operations;
using MyApiMonitorClassLibrary.Interfaces;
using System.Net;
using System.Runtime.CompilerServices;
using static System.Net.WebRequestMethods;

namespace MyApiMonitorClassLibrary.Models
{
    public class TestCollectionSetup_WhaddonShowApi
    {
        public TestCollectionSetup_WhaddonShowApi()
        {
        }



        private static string baseUrl = "https://thewhaddonshowdev.azurewebsites.net/api/v2/";

        public ApiTestCollection GenerateTestCollection()
        {
            ApiTestCollection output = new ApiTestCollection(Guid.Parse("05b0adac-6ee4-4390-a83b-092ca92b040d"), "The Whaddon Show API Test", DateTime.Now);

            GenerateTests(output);

            return output;
        }




        private ApiTest ResetPartSampleData = new ApiTest(Guid.Parse("25df31f5-3b9c-40c3-9d83-883dcb21a9e8"), "Part - ResetSampleData", HttpMethod.Delete, "Part/resetsampledata", HttpStatusCode.OK);

        private ApiTest ResetPersonSampleData = new ApiTest(Guid.Parse("137456d3-7b49-41d3-bc92-3585026afe70"), "Person - ResetSampleData", HttpMethod.Delete, "Person/resetsampledate", HttpStatusCode.OK);

        private ApiTest ResetScriptItemSampleData = new ApiTest(Guid.Parse("5aa52bf0-23fc-4edc-a642-acb074de0e66"), "ScriptItem - ResetSampleData", HttpMethod.Delete, "ScriptItem/resetsampledata", HttpStatusCode.OK);





        private void GenerateTests(ApiTestCollection testCollection)
        {
            testCollection.Tests.Add(ResetPartSampleData);

            testCollection.Tests.Add(new ApiTest(Guid.Parse("572aaa9f-de31-4862-b06c-e07a6ef8da3e")
                                                        , "Part - Latest"
                                                        , HttpMethod.Get
                                                        , "Part/latest?ids=68417C12-80C3-48BC-8EBE-3F3F2A91B8E5,17822466-DD66-4F2D-B4A9-F7EAAD6EB08B,F380FD46-6E6E-450D-AD3E-23EEC0B6A75E"
                                                        , HttpStatusCode.OK));

            testCollection.Tests.Add(new ApiTest(Guid.Parse("6bb17865-ce95-4afc-95f2-d65a42d27a11")
                                                        , "Part - History"
                                                        , HttpMethod.Get
                                                        , "Part/history?ids=68417C12-80C3-48BC-8EBE-3F3F2A91B8E5,17822466-DD66-4F2D-B4A9-F7EAAD6EB08B,F380FD46-6E6E-450D-AD3E-23EEC0B6A75E"
                                                        , HttpStatusCode.OK));

            testCollection.Tests.Add(new ApiTest(Guid.Parse("c9a45c3f-9f3b-44d2-b8d0-df53730b1675")
                                                        , "Part - Unsynced"
                                                        , HttpMethod.Get
                                                        , "Part/unsynced/27fc9657-3c92-6758-16a6-b9f82ca696b3"
                                                        , HttpStatusCode.OK));

            testCollection.Tests.Add(new ApiTest(Guid.Parse("bd5401fa-9a9b-448b-b0a4-ec176f45a6c0")
                                                        , "Part - Conflicts"
                                                        , HttpMethod.Get
                                                        , "Part/conflicts?ids=68417C12-80C3-48BC-8EBE-3F3F2A91B8E5,17822466-DD66-4F2D-B4A9-F7EAAD6EB08B,F380FD46-6E6E-450D-AD3E-23EEC0B6A75E"
                                                        , HttpStatusCode.OK));

            testCollection.Tests.Add(new ApiTest(Guid.Parse("3843b1fc-7788-482e-8431-825bd0582d37")
                                                        , "Part - Update"
                                                        , HttpMethod.Post
                                                        , "Part/updates/27fc9657-3c92-6758-16a6-b9f82ca696b3"
                                                        , @$"[  {{
                                                                ""Id"":""{Guid.NewGuid()}""
                                                                ,""Created"":""{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss")}"",
                                                                ""CreatedBy"":""mcarter"",
                                                                ""UpdatedOnServer"":null,
                                                                ""IsConflicted"":false,
                                                                ""IsActive"":false,
                                                                ""IsSample"":true,
                                                                ""Name"": ""Rodney"",
                                                                ""PersonId"" : null,
                                                                 ""Tags"":[""Male"", ""Test""]
                                                          }} """
                                                        , HttpStatusCode.OK
                                                        ));

            testCollection.Tests.Add(ResetPartSampleData);

        }

    }
}
