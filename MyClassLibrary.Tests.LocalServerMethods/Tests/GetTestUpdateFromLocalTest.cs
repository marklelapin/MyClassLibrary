
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.EventHandlers;
using MyClassLibrary.LocalServerMethods.Extensions;
using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using System.Text.Json;


namespace MyClassLibrary.Tests.LocalServerMethods.Tests;

public class GetTestUpdateFromLocalTests : IGetTestUpdateFromLocalTests
{
  private readonly ILocalDataAccess<TestUpdate> _localDataAccess;


    public GetTestUpdateFromLocalTests(ILocalDataAccess<TestUpdate> localDataAccess)
    {
        _localDataAccess = localDataAccess;
    }


    [Fact]
    public async Task GetLocalCopyIDTest()
    {
        var copyIdFirstAttemptTask = _localDataAccess.GetLocalCopyID(); //if database has been reset this will be null in the database and the function should create new Id.

        await Task.WhenAll(copyIdFirstAttemptTask);

        Guid copyIdFirstAttempt = copyIdFirstAttemptTask.Result;

        Guid copyIdSecondAttempt = await _localDataAccess.GetLocalCopyID();

        Assert.Equal(copyIdFirstAttempt, copyIdSecondAttempt);
    }


    public static object[][] GetLatestUpdatesTestData()
    {
        return new object[][] {
            new object[] {
                new List<Guid> {TestContent.SingleTestId }
                ,TestContent.SingleLatestUpdateOnLocal
            }
            ,new object[]
            {
                TestContent.TwoTestIds
                ,TestContent.TwoLatestTestUpdatesOnLocal
            }
            ,new object[]
            {
                new List<Guid>()
                ,TestContent.AllLatestTestUpdatesOnLocal
            }
        };
    }
    [Theory, MemberData(nameof(GetLatestUpdatesTestData))]
    public async Task GetLatestUpdatesTest(List<Guid>? ids, List<TestUpdate> expected)
    {
        //setup
        if (ids?.Count == 0) { ids = null; };
        expected = expected.SortByCreated();

        //test
        List<TestUpdate> actual = await _localDataAccess.GetUpdatesFromLocal(ids, true);


        //assert
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }


    public static object[][] GetAllUpdatesTestData()
    {
        return new object[][]{
            new object[] {
               new List<Guid> {TestContent.SingleTestId }
                ,TestContent.SingleTestUpdatesOnLocal
            }
            ,new object[]
            {
                TestContent.TwoTestIds
                ,TestContent.TwoTestUpdatesOnLocal
            }
            ,new object[]
            {
                new List<Guid>()
                , TestContent.LocalStartingData
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
        List<TestUpdate> actual = await _localDataAccess.GetUpdatesFromLocal(ids, false);
        
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
        List<TestUpdate> actual = await _localDataAccess.GetConflictedUpdatesFromLocal(ids);

        //Assert
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }


    [Fact]
    public async Task GetUnsyncedUpdatesFromLocalTest()
    {
        //Setup
        List<TestUpdate> expected = TestContent.LocalUnsyncedUpdates;
        expected = expected.SortByCreated();

        //Test
        List<TestUpdate> actual = await _localDataAccess.GetUnsyncedFromLocal();


        //Assert
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }


}
