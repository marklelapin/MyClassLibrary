
using MyClassLibrary.LocalServerMethods.Extensions;
using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using System.Text.Json;


namespace MyClassLibrary.Tests.LocalServerMethods.Tests;

public class GetTestUpdateFromServerTests : IGetTestUpdateFromServerTests
{

    private readonly IServerDataAccess<TestUpdate> _serverDataAccess;

    private static Guid CopyId { get { return TestContent.CopyId; } } //This matches the GUid used when resetting the ServerSyncInfo table in ResetSampleData

    public GetTestUpdateFromServerTests(IServerDataAccess<TestUpdate> serverDataAccess)
    {
        _serverDataAccess = serverDataAccess;
    }


    public static object[][] GetLatestUpdatesTestData()
    {
        return new object[][] {
            new object[] {
                new List<Guid> {TestContent.SingleTestId }
                ,TestContent.SingleLatestUpdateOnServer
            }
            ,new object[]
            {
                TestContent.TwoTestIds
                ,TestContent.TwoLatestTestUpdatesOnServer
            }
            ,new object[]
            {
                new List<Guid>()
                ,TestContent.AllLatestTestUpdatesOnServer
            }
        };
    }
    [Theory, MemberData(nameof(GetLatestUpdatesTestData))]
    public async Task GetLatestUpdatesTest(List<Guid>? ids, List<TestUpdate> expected)
    {
        //Setup
        if (ids?.Count == 0) { ids = null; };
        expected = expected.SortByCreated();

        //Test
        List<TestUpdate> actual = await _serverDataAccess.GetUpdatesFromServer(ids, true);

        //Assert
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }



    public static object[][] GetAllUpdatesTestData()
    {
        return new object[][]{
            new object[] {
                new List<Guid> {TestContent.SingleTestId }
                ,TestContent.SingleTestUpdatesOnServer
            }
            ,new object[]
            {
                TestContent.TwoTestIds
                ,TestContent.TwoTestUpdatesOnServer
            }
            ,new object[]
            {
                new List<Guid>()
                , TestContent.ServerStartingData
            }
        };
    }
    [Theory, MemberData(nameof(GetAllUpdatesTestData))]
    public async Task GetAllUpdatesTest(List<Guid>? ids, List<TestUpdate> expected)
    {
        //Setup
        if (ids?.Count == 0) { ids = null; };
        expected = expected.SortByCreated();

        //Test
        List<TestUpdate> actual = await _serverDataAccess.GetUpdatesFromServer(ids, false);

        //Assert
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }


    [Fact]
    public async Task GetConflictedUpdatesTest()
    {
        //Setup
        List<Guid> ids = new List<Guid> { TestContent.ConflictedTestId };
        List<TestUpdate> expected = TestContent.ConflictedTestUpdates;
        expected = expected.SortByCreated();

        //Test
        List<TestUpdate> actual = await _serverDataAccess.GetConflictedUpdatesFromServer(ids);

        //Assert
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }

    public static object[][] GetUnsyncedUpdateTestData()
    {
        return new object[][]
        {
            new object[] {TestContent.CopyId,TestContent.ServerUnsyncedUpdates}
            ,new object[] {TestContent.CopyId2,new List<TestUpdate>()} //Data is setup so that CopyId2 is fully synced already
        };
    }
    [Theory, MemberData(nameof(GetUnsyncedUpdateTestData))]
    public async Task GetUnsyncedUpdatesFromServerTest(Guid copyId,List<TestUpdate> expected)
    {
        //Setup
       expected = expected.SortByCreated();
               
        
        //Test
        List<TestUpdate> actual = await _serverDataAccess.GetUnsyncedFromServer(copyId);
       
        //Assert
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }



}
