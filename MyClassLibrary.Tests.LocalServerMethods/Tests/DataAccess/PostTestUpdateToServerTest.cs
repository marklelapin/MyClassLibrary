using MyClassLibrary.LocalServerMethods.Extensions;
using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Models;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using System.Text.Json;


namespace MyClassLibrary.Tests.LocalServerMethods.Tests.DataAccess;

public class PostTestUpdate_ServerDataAccessTests : IPostTestUpdateToServerTests
{
    private readonly IServerDataAccess<TestUpdate> _serverDataAccess;

    private static Guid CopyId = TestContent.CopyId; //This matches the GUid used when resetting the ServerSyncInfo table in ResetSampleData
    private static Guid CopyId2 = TestContent.CopyId2; //This matches the 2nd Local Copy GUid used when resetting the ServerSyncInfo table in ResetSampleData

    public PostTestUpdate_ServerDataAccessTests(IServerDataAccess<TestUpdate> serverDataAccess)
    {
        _serverDataAccess = serverDataAccess;
    }

    [Fact]
    public async Task SaveUpdatesTest()
    {
        //Test
        await _serverDataAccess.SaveUpdatesToServer(TestContent.GetNewUpdates(), CopyId);

        //Assert
        Assert.True(true);
    }

    [Fact]
    public async Task SaveAndGetUpdatesTest()
    {
        //Setup
        List<TestUpdate> testUpdates = TestContent.GetNewUpdatesWithoutUpdatedOnServerDates();

        //Test
        List<ServerToLocalPostBack> actualPostBack = await _serverDataAccess.SaveUpdatesToServer(testUpdates, CopyId);
        actualPostBack = actualPostBack.SortByCreated();

        //Get Result From Server
        List<TestUpdate> actual = await _serverDataAccess.GetUpdatesFromServer(testUpdates.Select(x => x.Id).ToList(), false);
        actual = actual.SortByCreated();


        //Assert UpdateOnServer
        Assert.True(actual.Where(x => x.UpdatedOnServer == null).Count() == 0, "UpdatedOnServer Not Being Saved Fully");


        //Get expected (can only be done once UpdatedOnServer date is known.
        DateTime updatedOnServer = (DateTime)actual.First().UpdatedOnServer!;
        List<TestUpdate> expected = testUpdates;
        expected.ForEach(update =>
        {
            if (update.UpdatedOnServer == null) { update.UpdatedOnServer = updatedOnServer; };
        });
        expected = expected.SortByCreated();
        List<ServerToLocalPostBack> expectedPostBack = testUpdates.Select(x => new ServerToLocalPostBack(x.Id, x.Created, x.IsConflicted, (DateTime)x.UpdatedOnServer!)).ToList();


        //Assert
        Assert.Equal(JsonSerializer.Serialize(expectedPostBack), JsonSerializer.Serialize(actualPostBack));
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }


    [Fact]
    public async Task SaveAndGetDuplicateUpdatesTest()
    {
        //Test Setup
        List<TestUpdate> testUpdates = TestContent.GetNewUpdates();

        List<ServerToLocalPostBack> expectedPostBack = await _serverDataAccess.SaveUpdatesToServer(testUpdates, CopyId);

        List<TestUpdate> expected = await _serverDataAccess.GetUpdatesFromServer(testUpdates.Select(x => x.Id).ToList());


        //Test - rerunning the save with same values - shouldn't error out but shouldn't change anything either.
        List<ServerToLocalPostBack> actualPostBack = await _serverDataAccess.SaveUpdatesToServer(testUpdates, CopyId);

        //Get Result from Server
        List<TestUpdate> actual = await _serverDataAccess.GetUpdatesFromServer(testUpdates.Select(x => x.Id).ToList());

        //Assert
        Assert.Equal(JsonSerializer.Serialize(expectedPostBack), JsonSerializer.Serialize(actualPostBack));
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }


    [Fact]
    public async Task ClearConflictsTest()
    {
        //Test Setup
        List<TestUpdate> testUpdates = TestContent.GetNewUpdatesWithConflicts();
        Guid testId = testUpdates.First().Id;
        await _serverDataAccess.SaveUpdatesToServer(testUpdates, CopyId);

        //Test
        await _serverDataAccess.ClearConflictsFromServer(new List<Guid> { testId });

        //Get Result from server
        List<TestUpdate> actual = await _serverDataAccess.GetUpdatesFromServer(testUpdates.Select(x => x.Id).ToList());


        //Assert
        Assert.True(actual.Where(x => x.IsConflicted == true).Count() == 1, "Not all conflicts have been removed."); //count = 1 is used as the test data has 1 line with an id that isn't cleared.
    }

    [Fact]
    public async Task LocalPostBackToServerTest()
    {
        //Test Setup
        List<TestUpdate> testUpdates = TestContent.GetNewUpdatesWithoutUpdatedOnServerDates();

        List<ServerToLocalPostBack> serverToLocalPostBack = await _serverDataAccess.SaveUpdatesToServer(testUpdates, CopyId2);//simluates a save to the server from a different local

        //simulates server then saving these to a new local and that local returning a post back.
        List<LocalToServerPostBack> localToServerPostBack = serverToLocalPostBack.Select(x => new LocalToServerPostBack(x.Id, x.Created, x.IsConflicted)).ToList()!;

        //Test
        await _serverDataAccess.LocalPostBackToServer(localToServerPostBack, CopyId);


        //GetResult - looks for unsynced testUpdates and filters for Ids.
        List<TestUpdate> actual = await _serverDataAccess.GetUnsyncedFromServer(CopyId);
        actual = actual.Where(x => testUpdates.Select(y => y.Id).ToList().Contains(x.Id)).ToList();    //This is necessary as data on server may contain other unsynced updates.


        //Assert
        Assert.True(actual.Count == 0);

    }

}

