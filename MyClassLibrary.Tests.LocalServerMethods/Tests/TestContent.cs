using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    public static class TestContent
    {
        public static Guid CopyId = new Guid("27fc9657-3c92-6758-16a6-b9f82ca696b3");

        public static Guid CopyId2 = new Guid("05A3DE29-008D-4785-825A-BC6A0A286A40");

        /// <summary>
        /// The TestUpdates matching testupdates on Local Storage just after ResetSampleData has been run
        /// </summary>
        public static List<TestUpdate> LocalStartingData
        {
            get
            {

                List<TestUpdate> output = new List<TestUpdate>()
            {
                //Setup so that this guid is unsynced on both local and storage (i.e. updatedOnServer = null)
                new TestUpdate(new Guid("e5ec560c-ab81-13b3-ece1-43b10bb19e49"),DateTime.Parse("2023-5-15T09:04:00.1234567"),"mr test",null,false,true,"Bob","Hoskins",DateTime.Parse("1999-12-31T23:59:59.1234567"),new List<string>{"Cake","Chocolate","Biscuits"},true)
                ,new TestUpdate(new Guid("e5ec560c-ab81-13b3-ece1-43b10bb19e49"),DateTime.Parse("2023-5-14T09:05:00.1234567"),"mr test",null,false,true,"Bob","Hoskins",DateTime.Parse("1999-12-31T23:59:59.1234567"),new List<string>{"Cake","Chocolate"},true)
                ,new TestUpdate(new Guid("e5ec560c-ab81-13b3-ece1-43b10bb19e49"),DateTime.Parse("2023-5-13T09:06:00.1234567"),"mr test",null,false,true,"Bob","Hoskins",DateTime.Parse("1999-12-31T23:59:59.1234567"),new List<string>{"Cake"},true)
                //Setup as already conflicted updates on local and storage with latest update IsActive = false
                ,new TestUpdate(new Guid("3d704ce3-1dc0-eba0-ace3-3b2428f41005"),DateTime.Parse("2023-5-12T09:02:00.1234567"),"mr test",DateTime.Parse("2023-5-12T09:02:20.1234567"),true,false,"Tracey","Emin",DateTime.Parse("1985-11-23T09:05:00.1234567"),new List<string>{"Cherries"},true)
                ,new TestUpdate(new Guid("3d704ce3-1dc0-eba0-ace3-3b2428f41005"),DateTime.Parse("2023-5-11T09:34:00.1234567"),"mr test",DateTime.Parse("2023-5-11T09:34:20.1234567"),true,true,"Tracey","Emin",DateTime.Parse("1985-11-23T09:05:00.1234567"),new List<string>{"Cherries"},true)
                //Normal Setup 
               ,new TestUpdate(new Guid("b01df9cc-4af9-7d5b-57e4-3ec0ec922e8b"),DateTime.Parse("2023-5-10T09:00:00.1234567"),"mr test",DateTime.Parse("2023-5-10T09:00:20.1234567"),false,true,"Jim","Broadbent",null,null,false)
                //Setup with additional update on Local that is unsynced with Server.
                ,new TestUpdate(new Guid("db516527-fcc3-6da6-b090-fb5ff747c7c2"),DateTime.Parse("2023-5-9T10:35:00.1234567"),"mr test",null,false,true,"Fred","Astair",DateTime.Parse("1945-11-11T11:11:11.1234567"),new List<string>{"Chicken"},true)
                ,new TestUpdate(new Guid("db516527-fcc3-6da6-b090-fb5ff747c7c2"),DateTime.Parse("2023-5-9T09:35:00.1234567"),"mr test",DateTime.Parse("2023-5-9T09:35:20.1234567"),false,true,"Fred","Astair",DateTime.Parse("1945-11-11T11:11:11.1234567"),new List<string>{"Chicken","Beef"},true)
                    ,new TestUpdate(new Guid("db516527-fcc3-6da6-b090-fb5ff747c7c2"),DateTime.Parse("2023-5-8T09:43:00.1234567"),"mr test",DateTime.Parse("2023-5-8T09:43:20.1234567"),false,true,"Fred","Astair",DateTime.Parse("1945-11-11T11:11:11.1234567"),new List<string>{"Chicken","Beef","Lamb"},false)

            };

                return output;
            }

        }
        /// <summary>
        /// The TestUpdates matching testupdates on Local Storage just after ResetSampleData has been run
        /// </summary>
        public static List<TestUpdate> ServerStartingData
        {
            get
            {
                List<TestUpdate> output = new List<TestUpdate>()
                {
                    //Setup so that this guid is unsynced on both local and storage (i.e. the ServerSyncInfo table does not have CopyId associated with this.)
                   new TestUpdate(new Guid("e5ec560c-ab81-13b3-ece1-43b10bb19e49"),DateTime.Parse("2023-5-16T09:04:00.1234567"),"mrs test",DateTime.Parse("2023-5-15T09:04:00.1234567"),false,true,"Bob","Hoskins",DateTime.Parse("1999-12-31T23:59:59.1234567"),new List<string>{"Cake","Chocolate","Biscuits","IceCream"},false)
                    //Setup as already conflicted updates on local and storage with latest update IsACtive = false
                    ,new TestUpdate(new Guid("3d704ce3-1dc0-eba0-ace3-3b2428f41005"),DateTime.Parse("2023-5-12T09:02:00.1234567"),"mr test",DateTime.Parse("2023-5-12T09:02:20.1234567"),true,false,"Tracey","Emin",DateTime.Parse("1985-11-23T09:05:00.1234567"),new List<string>{"Cherries"},true)
                    ,new TestUpdate(new Guid("3d704ce3-1dc0-eba0-ace3-3b2428f41005"),DateTime.Parse("2023-5-11T09:34:00.1234567"),"mr test",DateTime.Parse("2023-5-11T09:34:20.1234567"),true,true,"Tracey","Emin",DateTime.Parse("1985-11-23T09:05:00.1234567"),new List<string>{"Cherries"},true)
                    ,new TestUpdate(new Guid("b01df9cc-4af9-7d5b-57e4-3ec0ec922e8b"),DateTime.Parse("2023-5-10T09:00:00.1234567"),"mr test",DateTime.Parse("2023-5-10T09:00:20.1234567"),false,true,"Jim","Broadbent",null,null,false)
                    ,new TestUpdate(new Guid("db516527-fcc3-6da6-b090-fb5ff747c7c2"),DateTime.Parse("2023-5-9T09:35:00.1234567"),"mr test",DateTime.Parse("2023-5-9T09:35:20.1234567"),false,true,"Fred","Astair",DateTime.Parse("1945-11-11T11:11:11.1234567"),new List<string>{"Chicken","Beef"},true)
                    ,new TestUpdate(new Guid("db516527-fcc3-6da6-b090-fb5ff747c7c2"),DateTime.Parse("2023-5-8T09:43:00.1234567"),"mr test",DateTime.Parse("2023-5-8T09:43:20.1234567"),false,true,"Fred","Astair",DateTime.Parse("1945-11-11T11:11:11.1234567"),new List<string>{"Chicken","Beef","Lamb"},false)

                };

                return output;
            }

        }

        //Single Id Test Answers
        public static Guid SingleTestId = new Guid("db516527-fcc3-6da6-b090-fb5ff747c7c2");

        public static List<TestUpdate> SingleTestUpdatesOnServer = ServerStartingData.Where(x => x.Id == SingleTestId).ToList();
        public static List<TestUpdate> SingleTestUpdatesOnLocal = LocalStartingData.Where(x => x.Id == SingleTestId).ToList();

        public static List<TestUpdate> SingleLatestUpdateOnServer = new List<TestUpdate> { ServerStartingData[4] };
        public static List<TestUpdate> SingleLatestUpdateOnLocal = new List<TestUpdate> { LocalStartingData[6] };



        //Two TEst Id Test Answeres
        public static List<Guid> TwoTestIds = new List<Guid> { new Guid("b01df9cc-4af9-7d5b-57e4-3ec0ec922e8b"), new Guid("db516527-fcc3-6da6-b090-fb5ff747c7c2") };

        public static List<TestUpdate> TwoTestUpdatesOnLocal = LocalStartingData.Where(x => TwoTestIds.Contains(x.Id)).ToList();
        public static List<TestUpdate> TwoTestUpdatesOnServer = ServerStartingData.Where(x => TwoTestIds.Contains(x.Id)).ToList();

        public static List<TestUpdate> TwoLatestTestUpdatesOnLocal = new List<TestUpdate> { LocalStartingData[5], LocalStartingData[6] };
        public static List<TestUpdate> TwoLatestTestUpdatesOnServer = new List<TestUpdate> { ServerStartingData[3], ServerStartingData[4] };


        //All Data
        public static List<TestUpdate> AllLatestTestUpdatesOnLocal = new List<TestUpdate> { LocalStartingData[0]
                                                                                            ,LocalStartingData[3]
                                                                                            ,LocalStartingData[5]
                                                                                            ,LocalStartingData[6]};

        public static List<TestUpdate> AllLatestTestUpdatesOnServer = new List<TestUpdate> {  ServerStartingData[0]
                                                                                            ,ServerStartingData[1]
                                                                                            ,ServerStartingData[3]
                                                                                            ,ServerStartingData[4]};

        //Conflicted
        //this is the same wether on local of server
        public static Guid ConflictedTestId = new Guid("3d704ce3-1dc0-eba0-ace3-3b2428f41005");
        public static List<TestUpdate> ConflictedTestUpdates = LocalStartingData.Where(x => x.Id == ConflictedTestId).ToList(); //same on either server or local


        //Unsynced Data has been setup so that Bob Hoskins Guid "e5ec560c - ab81 - 13b3 - ece1 - 43b10bb19e49" is unsynced on both local and server storage
        //Plus additional unsynced on local for FredAstair
        public static List<TestUpdate> LocalUnsyncedUpdates = LocalStartingData.Where(x => x.UpdatedOnServer == null).ToList();

        public static List<TestUpdate> ServerUnsyncedUpdates = ServerStartingData.Where(x => x.Id == new Guid("e5ec560c-ab81-13b3-ece1-43b10bb19e49")).ToList();



        /// <summary>
        /// Returns a List of Sample Updates with new Guids with a broad mixture of potential and edge case values.
        /// </summary>
        /// <returns></returns>
        public static List<TestUpdate> GetNewUpdates()
        {
            return new List<TestUpdate>()
                {
                    new TestUpdate(Guid.NewGuid(), new DateTime(DateTime.UtcNow.Ticks).AddSeconds(20), "mr test", null, false, true, "Bob", "Hoskins", DateTime.Parse("1999-12-31T23:59:59.1234567"), new List<string> { "Cake", "Chocolate", "Biscuits" }, true)
                    ,new TestUpdate(Guid.NewGuid(), new DateTime(DateTime.UtcNow.Ticks).AddSeconds(10), "mr test", DateTime.Parse("2023-05-10T09:00:20.1234567"), false, true, "Jim", "Broadbent", null, new List<string> { },false)
                    ,new TestUpdate(Guid.NewGuid(), new DateTime(DateTime.UtcNow.Ticks), "mr test", DateTime.Parse("2023-05-9T09:35:20.1234567"), false, true, "Fred", "Astair", DateTime.Parse("1945-11-11T11:11:11.1234567"), new List<string> { "Chicken", "Beef" }, true)
                    ,new TestUpdate(Guid.NewGuid(), new DateTime(DateTime.UtcNow.Ticks).AddSeconds(-10), "mr test", DateTime.Parse("2023-05-8T09:43:20.1234567"), false, true, "Fred", "Astair", DateTime.Parse("1945-11-11T11:11:11.1234567"), new List<string> { "Chicken", "Beef", "Lamb" }, false)
                };

        }
        /// <summary>
        /// Returns a List of Sample Updates with the same new Guid Id and with IsConflicted on some of them.
        /// </summary>
        public static List<TestUpdate> GetNewUpdatesWithConflicts()
        {
            Guid id = Guid.NewGuid();
            return new List<TestUpdate>()
                {
                    new TestUpdate(id, new DateTime(DateTime.UtcNow.Ticks).AddSeconds(20), "mr test", null, true, true, "Bob", "Hoskins", DateTime.Parse("1999-12-31T23:59:59.1234567"), new List<string> { "Cake", "Chocolate", "Biscuits" }, true)
                    ,new TestUpdate(id, new DateTime(DateTime.UtcNow.Ticks).AddSeconds(10), "mr test", DateTime.Parse("2023-05-10T09:00:20.1234567"), true, true, "Jim", "Broadbent", null, new List<string> { },false)
                    ,new TestUpdate(id, new DateTime(DateTime.UtcNow.Ticks), "mr test", DateTime.Parse("2023-05-9T09:35:20.1234567"), true, true, "Fred", "Astair", DateTime.Parse("1945-11-11T11:11:11.1234567"), new List<string> { "Chicken", "Beef" }, true)
                    ,new TestUpdate(Guid.NewGuid(), new DateTime(DateTime.UtcNow.Ticks).AddSeconds(-10), "mr test", DateTime.Parse("2023-05-8T09:43:20.1234567"), true, true, "Fred", "Astair", DateTime.Parse("1945-11-11T11:11:11.1234567"), new List<string> { "Chicken", "Beef", "Lamb" }, false)
                };

        }

        /// <summary>
        /// Returns a List of Sample Updates where UpdatedOnServer = null.
        /// </summary>
        public static List<TestUpdate> GetNewUpdatesWithoutUpdatedOnServerDates()
        {
            return new List<TestUpdate>()
                {
                    new TestUpdate(Guid.NewGuid(), new DateTime(DateTime.UtcNow.Ticks).AddSeconds(20), "mr test", null, true, true, "Bob", "Hoskins", DateTime.Parse("1999-12-31T23:59:59.1234567"), new List<string> { "Cake", "Chocolate", "Biscuits" }, true)
                    ,new TestUpdate(Guid.NewGuid(), new DateTime(DateTime.UtcNow.Ticks).AddSeconds(10), "mr test", null, true, true, "Jim", "Broadbent", null, new List<string> { },false)
                    ,new TestUpdate(Guid.NewGuid(), new DateTime(DateTime.UtcNow.Ticks), "mr test", null, true, true, "Fred", "Astair", DateTime.Parse("1945-11-11T11:11:11.1234567"), new List<string> { "Chicken", "Beef" }, true)
                    ,new TestUpdate(Guid.NewGuid(), new DateTime(DateTime.UtcNow.Ticks).AddSeconds(-10), "mr test", null, true, true, "Fred", "Astair", DateTime.Parse("1945-11-11T11:11:11.1234567"), new List<string> { "Chicken", "Beef", "Lamb" }, false)
                };

        }



        /// <summary>
        /// Returns a List of Sample Updates that will cause an error as contain duplicate Id and created.
        /// </summary>
        public static List<TestUpdate> GetNewUpdatesToError()
        {
            Guid id = Guid.NewGuid();
            return new List<TestUpdate>()
                {
                    new TestUpdate(id,new DateTime(DateTime.UtcNow.Ticks).AddSeconds(20), "mr test", null, true, true, "Bob", "Hoskins", DateTime.Parse("1999-12-31T23:59:59.1234567"), new List<string> { "Cake", "Chocolate", "Biscuits" }, true)
                    ,new TestUpdate(id, new DateTime(DateTime.UtcNow.Ticks).AddSeconds(10), "mr test", null, true, true, "Jim", "Broadbent", null, new List<string> { },false)
                    ,new TestUpdate(id, new DateTime(DateTime.UtcNow.Ticks), "mr test", null, true, true, "Fred", "Astair", DateTime.Parse("1945-11-11T11:11:11.1234567"), new List<string> { "Chicken", "Beef" }, true)
                    ,new TestUpdate(id, new DateTime(DateTime.UtcNow.Ticks).AddSeconds(-10), "mr test", null, true, true, "Fred", "Astair", DateTime.Parse("1945-11-11T11:11:11.1234567"), new List<string> { "Chicken", "Beef", "Lamb" }, false)
                };

        }



        /// <summary>
        /// Returns a List of Sample Updates to be saved to local and server respectively that contain conflicts to be picked up by sync process.
        /// </summary>
        public static (List<TestUpdate> localUpdates, List<TestUpdate> serverUpdates) GetNewServerAndLocalUpdatesThatConflict()
        {
            Guid id = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();

            List<TestUpdate> localUpdates = new List<TestUpdate>()
            {
                new TestUpdate(id, DateTime.Parse("2023-5-15T09:04:00.1234567"), "mr test", null, true, true, "Frank", "Smith", DateTime.Parse("1999-12-31T23:59:59.1234567"), new List<string> { "Cake", "Chocolate", "Biscuits" }, true)
                ,new TestUpdate(id, DateTime.Parse("2023-5-15T09:03:00.1234567"), "mr test", null, true, true, "Frank", "Smith", null, new List<string> { },false)
                ,new TestUpdate(id2, DateTime.Parse("2023-5-15T09:02:00.1234567"), "mr test", null, true, true, "Michael", "Hatcher", DateTime.Parse("1945-11-11T11:11:11.1234567"), new List<string> { "Chicken", "Beef" }, true)
                ,new TestUpdate(id2, DateTime.Parse("2023-5-15T09:01:00.1234567"), "mr test", null, true, true, "Michael", "Hatcher", DateTime.Parse("1945-11-11T11:11:11.1234567"), new List<string> { "Chicken", "Beef", "Lamb" }, false)
            };

            List<TestUpdate> serverUpdates = new List<TestUpdate>()
            {
                new TestUpdate(id, DateTime.Parse("2023-5-15T10:07:00.1234567"), "mrs test", null, true, true, "Francesca", "Smith", DateTime.Parse("1999-12-31T23:59:59.1234567"), new List<string> { "Cake", "Chocolate", "Biscuits" }, true)
                ,new TestUpdate(id, DateTime.Parse("2023-5-15T10:06:00.1234567"), "mrs test", null, true, true, "Francesca", "Smith", DateTime.Parse("1999-12-31T23:59:59.1234567"), new List<string> { },false)
                ,new TestUpdate(id2, DateTime.Parse("2023-5-15T10:05:00.1234567"), "mrs test", null, true, true, "Michelle", "Hatter", DateTime.Parse("1945-11-11T11:11:11.1234567"), new List<string> { "Chicken", "Beef" }, true)
                ,new TestUpdate(id2, DateTime.Parse("2023-5-15T10:04:00.1234567"), "mrs test", null, true, true, "Michelle", "Hatter", DateTime.Parse("1945-11-11T11:11:11.1234567"), new List<string> { "Chicken", "Beef", "Lamb" }, false)

            };

            return (localUpdates, serverUpdates);

        }
    }
}
