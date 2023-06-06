
using Microsoft.Extensions.DependencyInjection;
using MyClassLibrary.LocalServerMethods.Extensions;
using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Models;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using System.Text.Json;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests;

public class PostTestUpdate_LocalDataAccessTests : IPostTestUpdateToLocalTests
{
    private readonly ILocalDataAccess<TestUpdate> _localDataAccess;

    public PostTestUpdate_LocalDataAccessTests(ILocalDataAccess<TestUpdate> localDataAccess)
    {
        _localDataAccess = localDataAccess;
    }


    //Tests
    [Fact]
    public async Task SaveUpdatesTest()
    {
        //Test
        await _localDataAccess.SaveUpdatesToLocal(TestContent.GetNewUpdates());

        //Assert
        Assert.True(true);
    }


    [Fact]
    public async Task SaveAndGetUpdatesTest()
    {
        //Setup
        List<TestUpdate> testUpdates = TestContent.GetNewUpdates();
        List<TestUpdate> expected = testUpdates;
        List<LocalToServerPostBack> expectedPostBack = testUpdates.Select(x => new LocalToServerPostBack(x.Id, x.Created, x.IsConflicted)).ToList();
        expectedPostBack = expectedPostBack.SortByCreated();

        //Test
        List<LocalToServerPostBack> actualPostBack = await _localDataAccess.SaveUpdatesToLocal(testUpdates);
        actualPostBack = actualPostBack.SortByCreated();

        //Get Result From Local
        List<TestUpdate> actual = await _localDataAccess.GetUpdatesFromLocal(testUpdates.Select(x => x.Id).ToList(), false);
        actual = actual.SortByCreated();

       //Assert
       Assert.Equal(JsonSerializer.Serialize(expectedPostBack), JsonSerializer.Serialize(actualPostBack));
       Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));


    }


    [Fact]
    public async Task SaveAndGetDuplicateUpdatesTest()
    {
        //Setup
        List<TestUpdate> testUpdates = TestContent.GetNewUpdates();
        List<TestUpdate> expected = testUpdates;
        List<LocalToServerPostBack> expectedPostBack = testUpdates.Select(x => new LocalToServerPostBack(x.Id, x.Created, x.IsConflicted)).ToList();
        await _localDataAccess.SaveUpdatesToLocal(testUpdates);
        expected = expected.SortByCreated();

        //Test
        List<LocalToServerPostBack> actualPostBack = await _localDataAccess.SaveUpdatesToLocal(testUpdates);
        actualPostBack = actualPostBack.SortByCreated();
        //Get Result from Local
        List<TestUpdate> actual = await _localDataAccess.GetUpdatesFromLocal(testUpdates.Select(x => x.Id).ToList());
        actual = actual.SortByCreated();

        //Assert
        Assert.Equal(JsonSerializer.Serialize(expectedPostBack), JsonSerializer.Serialize(actualPostBack));
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }

    [Fact]
    public async Task SaveAndGetLocalLastSyncDateTest()
    {
        //Setup
        DateTime lastSyncDate = DateTime.Now;
        DateTime expected = lastSyncDate;

        //Test Save
        await _localDataAccess.SaveLocalLastSyncDate(lastSyncDate);

        //Test Get
        DateTime actual = await _localDataAccess.GetLocalLastSyncDate();

        //Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task ClearConflictsTest()
    {
        //Test Setup
        List<TestUpdate> testUpdates = TestContent.GetNewUpdatesWithConflicts();
        Guid testId = testUpdates.First().Id;
        await _localDataAccess.SaveUpdatesToLocal(testUpdates);

        List<TestUpdate> expected = testUpdates;

        expected.ForEach(update =>
        {
            if (update.Id == testId) { update.IsConflicted = false; }
        });

        expected =  expected.SortByCreated();

        //Test
        await _localDataAccess.ClearConflictsFromLocal(new List<Guid> { testId });

        //Get Result
        List<TestUpdate> actual = await _localDataAccess.GetUpdatesFromLocal(testUpdates.Select(x => x.Id).ToList());

        //Assert
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }

    [Fact]
    public async Task ServerPostBackToLocalTest()
    {
        //Test Setup
        List<TestUpdate> testUpdates = TestContent.GetNewUpdatesWithoutUpdatedOnServerDates();
        await _localDataAccess.SaveUpdatesToLocal(testUpdates);

        //_serverDataAccess saves these and postbackUpdates (simulated below)
        DateTime updatedOnServer = new DateTime(DateTime.UtcNow.Ticks);
        List<TestUpdate> expected = testUpdates;
        expected.ForEach(update =>
        {
            update.UpdatedOnServer = updatedOnServer;
            update.IsConflicted = true;
        });
        expected = expected.SortByCreated();

        List<ServerToLocalPostBack> postBacks = expected.Select(x => new ServerToLocalPostBack(x.Id, x.Created, x.IsConflicted, updatedOnServer)).ToList();


        //Test
        await _localDataAccess.ServerPostBackToLocal(postBacks);

        //GetResult
        List<TestUpdate> actual = await _localDataAccess.GetUpdatesFromLocal(testUpdates.Select(x => x.Id).ToList());

        //Assert
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));

    }

}
