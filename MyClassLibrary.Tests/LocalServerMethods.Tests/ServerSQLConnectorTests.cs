using MyClassLibrary.Extensions;
using MyClassLibrary.LocalServerMethods;
using MyExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    public class ServerSQLConnectorTests
    {
        private static readonly ConnectionStringDictionary connectionStringDictionary = new ConnectionStringDictionary();
        

        private IServerDataAccess _serverDataAccess = new ServerSQLConnector(connectionStringDictionary.ServerSQL);


        private static readonly List<Guid> TestIds = Guid.NewGuid().GenerateList(20);
       
        private static DateTime DateNow = DateTime.Now;

        private static DateTime Created = new DateTime(DateNow.Year, DateNow.Month, DateNow.Day, DateNow.Hour, DateNow.Minute, DateNow.Second,0,DateTimeKind.Unspecified);


        private static List<TestObject> TestObjects  = new List<TestObject>
                {
                    new TestObject(TestIds[1], Created, "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" }),
                    new TestObject(TestIds[1], Created.AddSeconds(1), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" }),
                    new TestObject(TestIds[1], Created.AddSeconds(2), "mcarter", DateTime.Parse("2023-04-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" }),
                    new TestObject(TestIds[2], Created.AddSeconds(3), "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                    new TestObject(TestIds[2], Created.AddSeconds(4), "mcarter", DateTime.Parse("2023-04-04 09:02:00.000"), false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                    new TestObject(TestIds[3], Created.AddSeconds(5), "mcarter", DateTime.Parse("2023-04-02 05:02:00.000"), true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" }),
                    new TestObject(TestIds[4], Created.AddSeconds(6), "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate" }),
                    new TestObject(TestIds[4], Created.AddSeconds(7), "mcarter", DateTime.Parse("2023-04-01 09:02:00.000"), true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" })
                };
         

        //public static readonly object[][] TestData = 
        //{
  
        //    new object[] {TestObjects[1]},
        //    new object[] {TestObjects[2]},
        //    new object[] {TestObjects[3]},
        //    new object[] {TestObjects[4]},
        //    new object[] {TestObjects[5]},
        //    new object[] {TestObjects[6]},
        //    new object[] {TestObjects[7]},
        //    new object[] {TestObjects[8]},
        //};

        //[Theory, MemberData(nameof(TestData))]

        [Fact]
        public void SaveAndGetTest()
        {
        
            _serverDataAccess.SaveToServer(TestObjects);

            List<Guid>? getIds = TestObjects.Select(x => x.Id).ToList().Distinct().ToList();
            
            List<TestObject> actual = _serverDataAccess.GetFromServer<TestObject>(getIds,false);

            List<TestObject> expected = TestObjects;

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }

 

        /* Then setup similar of local connector
         * 1. save to local as with serverupdate and without.
         * 2. get changes
         * 3. get from local server.
         * 4. Update saveToLocalServer.
        */








    }
}
