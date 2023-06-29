using System.Net;

namespace MyApiMonitorClassLibrary.Models
{
    public class TestCollectionSetup_WhaddonShowApi
    {
        //TODO - Phase2Work: If going further with the Api Monitor the setup of the tests below should be moved to database and UI.

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



        //Repeated Tests
        private ApiTest ResetPartSampleData = new ApiTestBuilder("25df31f5-3b9c-40c3-9d83-883dcb21a9e8", "Part - ResetSampleData")
                                                .AddRequest(HttpMethod.Delete
                                                            , "Part/resetsampledata")
                                                .ExpectedStatusCode(HttpStatusCode.OK)
                                                .Build();

        private ApiTest ResetPersonSampleData = new ApiTestBuilder("137456d3-7b49-41d3-bc92-3585026afe70", "Person - ResetSampleData")
                                                .AddRequest(HttpMethod.Delete
                                                            , "Person/resetsampledata")
                                                .ExpectedStatusCode(HttpStatusCode.OK)
                                                .Build();

        private ApiTest ResetScriptItemSampleData = new ApiTestBuilder("5aa52bf0-23fc-4edc-a642-acb074de0e66", "ScriptItem - ResetSampleData")
                                                .AddRequest(HttpMethod.Delete
                                                           , "ScriptItem/resetsampledata")
                                                .ExpectedStatusCode(HttpStatusCode.OK)
                                                .Build();


        private void GenerateTests(ApiTestCollection testCollection)
        {
            GeneratePartTests(testCollection);

            GeneratePersonTests(testCollection);

            GenerateScriptItemTests(testCollection);
        }

        private void GeneratePartTests(ApiTestCollection testCollection)
        {
            testCollection.Tests.Add(ResetPartSampleData);

            testCollection.Tests.Add(new ApiTestBuilder("572aaa9f-de31-4862-b06c-e07a6ef8da3e", "Part - Latest")
            .AddRequest(HttpMethod.Get
                        , "Part/latest?ids=68417C12-80C3-48BC-8EBE-3F3F2A91B8E5,17822466-DD66-4F2D-B4A9-F7EAAD6EB08B,F380FD46-6E6E-450D-AD3E-23EEC0B6A75E")
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());



            testCollection.Tests.Add(new ApiTestBuilder("6bb17865-ce95-4afc-95f2-d65a42d27a11", "Part - History")
            .AddRequest(HttpMethod.Get
                        , "Part/history?ids=68417C12-80C3-48BC-8EBE-3F3F2A91B8E5,17822466-DD66-4F2D-B4A9-F7EAAD6EB08B,F380FD46-6E6E-450D-AD3E-23EEC0B6A75E")
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());



            testCollection.Tests.Add(new ApiTestBuilder("c9a45c3f-9f3b-44d2-b8d0-df53730b1675", "Part - Unsynced")
            .AddRequest(HttpMethod.Get
                        , "Part/unsynced/27fc9657-3c92-6758-16a6-b9f82ca696b3")
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());

            testCollection.Tests.Add(new ApiTestBuilder("bd5401fa-9a9b-448b-b0a4-ec176f45a6c0", "Part - Conflicts")
           .AddRequest(HttpMethod.Get
                       , "Part/conflicts?ids=68417C12-80C3-48BC-8EBE-3F3F2A91B8E5,17822466-DD66-4F2D-B4A9-F7EAAD6EB08B,F380FD46-6E6E-450D-AD3E-23EEC0B6A75E")
           .ExpectedStatusCode(HttpStatusCode.OK)
           .Build());

            testCollection.Tests.Add(new ApiTestBuilder("dbf5c341-39d8-4eef-b08f-f7e694126b91", "Part - ClearConflicts")
            .AddRequest(HttpMethod.Put
                        , "Part/conflicts/clear/?ids=68417C12-80C3-48BC-8EBE-3F3F2A91B8E5,17822466-DD66-4F2D-B4A9-F7EAAD6EB08B,F380FD46-6E6E-450D-AD3E-23EEC0B6A75E"
                        )
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());

            testCollection.Tests.Add(new ApiTestBuilder("8128d835-0ed5-4042-9012-295e6c91f5e1", "Part - ClearConflicts (Unauthorized)")
            .AddRequest(HttpMethod.Put
                        , "Part/conflicts/clear/?ids=68417C12-80C3-48BC-8EBE-3F3F2A91B8E5,17822466-DD66-4F2D-B4A9-F7EAAD6EB08B,F380FD46-6E6E-450D-AD3E-23EEC0B6A75E"
                       )
            .RemoveAuthentication()
            .ExpectedStatusCode(HttpStatusCode.Unauthorized)
            .Build()); ;

            testCollection.Tests.Add(new ApiTestBuilder("bd5401fa-9a9b-448b-b0a4-ec176f45a6c0", "Part - ClearConflicts Check")
            .AddRequest(HttpMethod.Get
                        , "Part/conflicts?ids=68417C12-80C3-48BC-8EBE-3F3F2A91B8E5,17822466-DD66-4F2D-B4A9-F7EAAD6EB08B,F380FD46-6E6E-450D-AD3E-23EEC0B6A75E")
            .ExpectedStatusCode(HttpStatusCode.NotFound)
            .Build());


            testCollection.Tests.Add(new ApiTestBuilder("3843b1fc-7788-482e-8431-825bd0582d37", "Part - Update")
            .AddRequest(HttpMethod.Post
                        , "Part/updates/27fc9657-3c92-6758-16a6-b9f82ca696b3"
                        , @$"[
                                    {{
                                        ""Id"":""{Guid.NewGuid()}""
                                        ,""Created"":""{DateTimeNowToString()}"",
                                        ""CreatedBy"":""mcarter"",
                                        ""UpdatedOnServer"":null,
                                        ""IsConflicted"":false,
                                        ""IsActive"":false,
                                        ""IsSample"":true,
                                        ""Name"": ""Rodney"",
                                        ""PersonId"" : null,
                                            ""Tags"":[""Male"", ""Test""]
                                    }} """)
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());

            testCollection.Tests.Add(new ApiTestBuilder("3843b1fc-7788-482e-8431-825bd0582d37", "Part - Update (Unauthorized)")
            .AddRequest(HttpMethod.Post
                        , "Part/updates/27fc9657-3c92-6758-16a6-b9f82ca696b3"
                        , @$"[
                                    {{
                                        ""Id"":""{Guid.NewGuid()}""
                                        ,""Created"":""{DateTimeNowToString()}"",
                                        ""CreatedBy"":""mcarter"",
                                        ""UpdatedOnServer"":null,
                                        ""IsConflicted"":false,
                                        ""IsActive"":false,
                                        ""IsSample"":true,
                                        ""Name"": ""Rodney"",
                                        ""PersonId"" : null,
                                            ""Tags"":[""Male"", ""Test""]
                                    }} """)
            .RemoveAuthentication()
            .ExpectedStatusCode(HttpStatusCode.Unauthorized)
            .Build());

            testCollection.Tests.Add(new ApiTestBuilder("f07c6b59-c0f1-4e22-ae76-97334331773d", "Part - PostBack")
            .AddRequest(HttpMethod.Put
                        , "Part/updates/postbackfromlocal/27fc9657-3c92-6758-16a6-b9f82ca696b3"
                        , @$"[
                                  {{
                                    ""id"":""{Guid.NewGuid()}"",
                                    ""created"":""{DateTimeNowToString()}"",
                                    ""isConflicted"": true
                                  }}
                                ]")
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());


            testCollection.Tests.Add(new ApiTestBuilder("f07c6b59-c0f1-4e22-ae76-97334331773d", "Part - PostBack (Unauthorized)")
            .AddRequest(HttpMethod.Put
                        , "Part/updates/postbackfromlocal/27fc9657-3c92-6758-16a6-b9f82ca696b3"
                        , @$"[
                                  {{
                                    ""id"":""{{{{newId}}}}"",
                                    ""created"":""{{{{created}}}}"",
                                    ""isConflicted"": true
                                  }}
                                ]")
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());

            testCollection.Tests.Add(ResetPartSampleData);


        }


        private void GeneratePersonTests(ApiTestCollection testCollection)
        {
            testCollection.Tests.Add(ResetPersonSampleData);

            testCollection.Tests.Add(new ApiTestBuilder("4204a2eb-ba5a-4c45-b500-7623af375890", "Person - Latest")
            .AddRequest(HttpMethod.Get
                        , "Person/latest?ids=545A9495-DB58-44EC-BA47-FD0B7E478D4A,2B3FA075-D0B5-49AB-B897-DAB1428CA500")
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());

            testCollection.Tests.Add(new ApiTestBuilder("5426f24c-3e55-44e9-aa66-08e0863b53ea", "Person - History")
            .AddRequest(HttpMethod.Get
                        , "Person/history/?ids=545A9495-DB58-44EC-BA47-FD0B7E478D4A,2B3FA075-D0B5-49AB-B897-DAB1428CA500")
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());

            testCollection.Tests.Add(new ApiTestBuilder("98f2896d-0c6d-45b3-be41-f978a69a2d15", "Person - Unsynced")
            .AddRequest(HttpMethod.Get
                        , "Person/unsynced/27fc9657-3c92-6758-16a6-b9f82ca696b3")
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());

            testCollection.Tests.Add(new ApiTestBuilder("925efc75-1392-4a8f-8210-b57d8c2e7c95", "Person - Conflicts")
           .AddRequest(HttpMethod.Get
                       , "Person/conflicts/?ids=545A9495-DB58-44EC-BA47-FD0B7E478D4A,2B3FA075-D0B5-49AB-B897-DAB1428CA500")
           .ExpectedStatusCode(HttpStatusCode.OK)
           .Build());

            testCollection.Tests.Add(new ApiTestBuilder("9c3cf6db-627d-4e45-9b43-4739bded952b", "Person - ClearConflicts")
            .AddRequest(HttpMethod.Put
                        , "Person/conflicts/clear/?ids=545A9495-DB58-44EC-BA47-FD0B7E478D4A,2B3FA075-D0B5-49AB-B897-DAB1428CA500")

            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());

            testCollection.Tests.Add(new ApiTestBuilder("31b5b792-96d6-4aab-bbd6-52d454c969f3", "Person - ClearConflicts (Unauthorized)")
            .AddRequest(HttpMethod.Put
                        , "Person/conflicts/clear/?ids=545A9495-DB58-44EC-BA47-FD0B7E478D4A,2B3FA075-D0B5-49AB-B897-DAB1428CA500")
            .RemoveAuthentication()
            .ExpectedStatusCode(HttpStatusCode.Unauthorized)
            .Build()); ;

            testCollection.Tests.Add(new ApiTestBuilder("afa325c5-e248-4125-9a96-6c77823ddbec", "Person - ClearConflicts Check")
            .AddRequest(HttpMethod.Get
                        , "Person/conflicts/?ids=545A9495-DB58-44EC-BA47-FD0B7E478D4A,2B3FA075-D0B5-49AB-B897-DAB1428CA500")
            .ExpectedStatusCode(HttpStatusCode.NotFound)
            .Build());


            testCollection.Tests.Add(new ApiTestBuilder("0fb56869-3fc7-4b9c-843e-e7abdbead420", "Person - Update")
            .AddRequest(HttpMethod.Post
                        , "Person/updates/27fc9657-3c92-6758-16a6-b9f82ca696b3"
                        , @$"[
                                    {{
                                        ""Id"":""{Guid.NewGuid()}""
                                        ,""Created"":""{DateTimeNowToString()}"",
                                        ""CreatedBy"":""mcarter"",
                                        ""UpdatedOnServer"":null,
                                        ""IsConflicted"":false,
                                        ""IsActive"":false,
                                        ""IsSample"":true,
                                        ""firstName"": ""string"",
                                        ""lastName"": ""string"",
                                        ""email"": ""user@example.com"",
                                        ""pictureRef"": ""string"",
                                        ""isActor"": true,
                                        ""isSinger"": true,
                                        ""isWriter"": true,
                                        ""isBand"": true,
                                        ""isTechnical"": true,
                                        ""tags"": [
                                          ""string""
                                        ]
                                    }} """)
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());

            testCollection.Tests.Add(new ApiTestBuilder("53fc516a-3dad-4ef3-8b7a-884f167cc232", "Person - Update (Unauthorized)")
            .AddRequest(HttpMethod.Post
                        , "Person/updates/27fc9657-3c92-6758-16a6-b9f82ca696b3"
                        , @$"[
                                    {{
                                        ""Id"":""{Guid.NewGuid()}""
                                        ,""Created"":""{DateTimeNowToString()}"",
                                        ""CreatedBy"":""mcarter"",
                                        ""UpdatedOnServer"":null,
                                        ""IsConflicted"":false,
                                        ""IsActive"":false,
                                        ""IsSample"":true,
                                        ""firstName"": ""string"",
                                        ""lastName"": ""string"",
                                        ""email"": ""user@example.com"",
                                        ""pictureRef"": ""string"",
                                        ""isActor"": true,
                                        ""isSinger"": true,
                                        ""isWriter"": true,
                                        ""isBand"": true,
                                        ""isTechnical"": true,
                                        ""tags"": [
                                          ""string""
                                        ]
                                    }} """)
            .RemoveAuthentication()
            .ExpectedStatusCode(HttpStatusCode.Unauthorized)
            .Build());

            testCollection.Tests.Add(new ApiTestBuilder("9ccb0abf-08b3-4f89-8fdc-e23b9c2a8eb1", "Person - PostBack")
            .AddRequest(HttpMethod.Put
                        , "Person/updates/postbackfromlocal/27fc9657-3c92-6758-16a6-b9f82ca696b3"
                        , @$"[
                                  {{
                                    ""id"":""{Guid.NewGuid()}"",
                                    ""created"":""{DateTimeNowToString()}"",
                                    ""isConflicted"": true
                                  }}
                                ]")
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());


            testCollection.Tests.Add(new ApiTestBuilder("b9bcaa12-de00-4ea3-a8a9-6c10b76739c8", "Person - PostBack (Unauthorized)")
            .AddRequest(HttpMethod.Put
                        , "Person/updates/postbackfromlocal/27fc9657-3c92-6758-16a6-b9f82ca696b3"
                        , @$"[
                                  {{
                                    ""id"":""{Guid.NewGuid()}"",
                                    ""created"":""{DateTimeNowToString()}"",
                                    ""isConflicted"": true
                                  }}
                                ]")
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());

            testCollection.Tests.Add(ResetPersonSampleData);


        }


        private void GenerateScriptItemTests(ApiTestCollection testCollection)
        {
            testCollection.Tests.Add(ResetScriptItemSampleData);

            testCollection.Tests.Add(new ApiTestBuilder("c74d408b-ba26-445b-b2af-7fb1bb00e959", "ScriptItem - Latest")
            .AddRequest(HttpMethod.Get
                        , "ScriptItem/latest?ids=FC97305D-8A92-42D5-94DB-6FC9F5FF1432,744BD79A-1A2B-425F-874F-315A3B3BA9F2,79E604CF-7CC2-41F6-B37F-F30C76AB5F34")
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());

            testCollection.Tests.Add(new ApiTestBuilder("8230d326-9c6e-4226-b24f-15362a23e225", "ScriptItem - History")
            .AddRequest(HttpMethod.Get
                        , "Scriptitem/history/?ids=FC97305D-8A92-42D5-94DB-6FC9F5FF1432,744BD79A-1A2B-425F-874F-315A3B3BA9F2,79E604CF-7CC2-41F6-B37F-F30C76AB5F34")
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());

            testCollection.Tests.Add(new ApiTestBuilder("be57cfae-b71f-4cdf-b7de-d1df8865a011", "ScriptItem - Unsynced")
            .AddRequest(HttpMethod.Get
                        , "ScriptItem/unsynced/27fc9657-3c92-6758-16a6-b9f82ca696b3")
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());

            testCollection.Tests.Add(new ApiTestBuilder("61061a33-6215-4639-887e-15ab2b142c74", "ScriptItem - Conflicts")
           .AddRequest(HttpMethod.Get
                       , "ScriptItem/conflicts/?ids=ED789FA3-4B2B-41A0-A322-773ED7CE89FE")
           .ExpectedStatusCode(HttpStatusCode.OK)
           .Build());

            testCollection.Tests.Add(new ApiTestBuilder("841da550-fe40-40ae-aeea-acdf356ba904", "ScriptItem - ClearConflicts")
            .AddRequest(HttpMethod.Put
                        , "ScriptItem/conflicts/clear/?ids=ED789FA3-4B2B-41A0-A322-773ED7CE89FE")
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());

            testCollection.Tests.Add(new ApiTestBuilder("68dcdd17-a997-4366-aed7-9a8cfafe307b", "ScriptItem - ClearConflicts (Unauthorized)")
            .AddRequest(HttpMethod.Put
                        , "ScriptItem/conflicts/clear/?ids=ED789FA3-4B2B-41A0-A322-773ED7CE89FE")
            .RemoveAuthentication()
            .ExpectedStatusCode(HttpStatusCode.Unauthorized)
            .Build());

            testCollection.Tests.Add(new ApiTestBuilder("b11b07ed-3b20-4c5f-a776-d42d5efa0689", "ScriptItem - ClearConflicts Check")
            .AddRequest(HttpMethod.Get
                        , "ScriptItem/conflicts/clear/?ids=ED789FA3-4B2B-41A0-A322-773ED7CE89FE")
            .ExpectedStatusCode(HttpStatusCode.NotFound)
            .Build());


            testCollection.Tests.Add(new ApiTestBuilder("db293067-4229-4d85-8b93-af34b6304aeb", "ScriptItem - Update")
            .AddRequest(HttpMethod.Post
                        , "ScriptItem/updates/27fc9657-3c92-6758-16a6-b9f82ca696b3"
                        , @$"[
                                    {{
                                        ""Id"":""{Guid.NewGuid()}""
                                        ,""Created"":""{DateTimeNowToString()}"",
                                        ""CreatedBy"":""mcarter"",
                                        ""UpdatedOnServer"":null,
                                        ""IsConflicted"":false,
                                        ""IsActive"":false,
                                        ""IsSample"":true,
                                        ""parentId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                                        ""orderNo"": 0,
                                        ""type"": ""Synopsis"",
                                        ""text"": ""string"",
                                        ""partIds"": [
                                                    ""3fa85f64-5717-4562-b3fc-2c963f66afa6""
                                        ],
                                        ""tags"": [
                                                    ""string""
                                        ]
                                        }}] """)
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());

            testCollection.Tests.Add(new ApiTestBuilder("0c151bc8-25aa-4a0d-9d80-2b728d16adc1", "ScriptItem - Update (Unauthorized)")
            .AddRequest(HttpMethod.Post
                        , "ScriptItem/updates/27fc9657-3c92-6758-16a6-b9f82ca696b3"
                        , @$"[
                                    {{
                                        ""Id"":""{Guid.NewGuid()}""
                                        ,""Created"":""{DateTimeNowToString()}"",
                                        ""CreatedBy"":""mcarter"",
                                        ""UpdatedOnServer"":null,
                                        ""IsConflicted"":false,
                                        ""IsActive"":false,
                                        ""IsSample"":true,
                                        ""parentId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                                        ""orderNo"": 0,
                                        ""type"": ""Synopsis"",
                                        ""text"": ""string"",
                                        ""partIds"": [
                                                    ""3fa85f64-5717-4562-b3fc-2c963f66afa6""
                                        ],
                                        ""tags"": [
                                                    ""string""
                                        ]
                                        }}] """)
            .RemoveAuthentication()
            .ExpectedStatusCode(HttpStatusCode.Unauthorized)
            .Build());

            testCollection.Tests.Add(new ApiTestBuilder("a0346f6e-a0c3-4720-8f62-6477ac53f9df", "ScriptItem - PostBack")
            .AddRequest(HttpMethod.Put
                        , "ScriptItem/updates/postbackfromlocal/27fc9657-3c92-6758-16a6-b9f82ca696b3"
                        , @$"[
                                  {{
                                    ""id"":""{Guid.NewGuid()}"",
                                    ""created"":""{DateTimeNowToString()}"",
                                    ""isConflicted"": true
                                  }}
                                ]")
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());


            testCollection.Tests.Add(new ApiTestBuilder("8154bc03-28c9-4e78-9588-d7e8d6680dc5", "ScriptItem - PostBack (Unauthorized)")
            .AddRequest(HttpMethod.Put
                        , "ScriptItem/updates/postbackfromlocal/27fc9657-3c92-6758-16a6-b9f82ca696b3"
                        , @$"[
                                  {{
                                    ""id"":""{Guid.NewGuid()}"",
                                    ""created"":""{DateTimeNowToString()}"",
                                    ""isConflicted"": true
                                  }}
                                ]")
            .ExpectedStatusCode(HttpStatusCode.OK)
            .Build());

            testCollection.Tests.Add(ResetScriptItemSampleData);


        }


        //Helper Methods

        private string DateTimeNowToString()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
        }

    }
}
