using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using MyClassLibrary.Extensions;
using MyClassLibrary.LocalServerMethods;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Services
{
    internal class TestContentService<T> : ITestContent<T> where T : LocalServerIdentityUpdate
    {

        public List<List<T>> Generate(int quantity, string testType = "Default", List<Guid>? overrideIds = null, DateTime? overrideCreated = null) where T : LocalServerIdentityUpdate
        {
            List<List<T>> output = new List<List<T>>();
            
            List<Guid> draftIds = overrideIds ?? GenerateDraftIds(quantity);

            draftIds.Sort((x, y) => x.CompareTo(y));   
            
            for (int i = 0; i < quantity; i++)
            {
                output.Add(GenerateList(testType, draftIds,overrideCreated)); 
            }

            return output;
        }

        public List<Guid> ListIds(List<T> updates)
        {
            return updates.Select(x => x.Id).ToList();
        }

         private List<Guid> GenerateDraftIds(int quantity)
        {
            return Guid.NewGuid().GenerateList(quantity);
        }

       
        private List<T> GenerateList(string testType,List<Guid> draftIds,DateTime? overrideCreated = null)
        {
            DateTime DateNow = overrideCreated ?? DateTime.Now;

            DateTime created = new DateTime(DateNow.Year, DateNow.Month, DateNow.Day, DateNow.Hour, DateNow.Minute, DateNow.Second, 0, DateTimeKind.Unspecified);

            switch (typeof(T).Name)
            {
                case "TestUpdate": return ConvertToListT<TestObject>(GenerateTestUpdateLists(testType, draftIds, created));
                   
                default: throw new NotImplementedException();
            };
        }

        private List<T> ConvertToListT<S>(List<S> listToConvert)
        {
            return JsonSerializer.Deserialize<List<T>>(JsonSerializer.Serialize<List<S>>(listToConvert)) ?? new List<T>();
        }

        private List<TestObject> GenerateTestUpdateLists(string testType, List<Guid> draftIds, DateTime createdDate)
        {
            List<TestObject> output;

            switch (testType)
            {
                case "Default":
                    output = new List<TestObject>()
                        {
                            new TestObject(draftIds[1], createdDate, "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" }),
                            new TestObject(draftIds[1], createdDate.AddSeconds(1), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" }),
                            new TestObject(draftIds[1], createdDate.AddSeconds(2), "mcarter", DateTime.Parse("2023-04-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" }),
                            new TestObject(draftIds[2], createdDate.AddSeconds(3), "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                            new TestObject(draftIds[2], createdDate.AddSeconds(4), "mcarter", null, false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                            new TestObject(draftIds[3], createdDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" }),
                            new TestObject(draftIds[4], createdDate.AddSeconds(6), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate" }),
                            new TestObject(draftIds[4], createdDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" })
                        };
                    break;

                case "Unsorted":
                    output = new List<TestObject>()
                            {
                                new TestObject(draftIds[1], createdDate, "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" }),
                                new TestObject(draftIds[1], createdDate.AddSeconds(2), "mcarter", DateTime.Parse("2023-04-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" }),
                                new TestObject(draftIds[2], createdDate.AddSeconds(3), "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                                new TestObject(draftIds[3], createdDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" }),
                                new TestObject(draftIds[4], createdDate.AddSeconds(6), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate" }),
                                new TestObject(draftIds[4], createdDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" }),
                                new TestObject(draftIds[2], createdDate.AddSeconds(4), "mcarter", null, false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                                new TestObject(draftIds[1], createdDate.AddSeconds(8), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Raspberries" }),
                                new TestObject(draftIds[1], createdDate.AddSeconds(1), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" })
                             };
                    break;
                case "SortedByCreated":
                    output = new List<TestObject>()
                            {
                                new TestObject(draftIds[1], createdDate, "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" }),
                                new TestObject(draftIds[1], createdDate.AddSeconds(1), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" }),
                                new TestObject(draftIds[1], createdDate.AddSeconds(2), "mcarter", DateTime.Parse("2023-04-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" }),
                                new TestObject(draftIds[2], createdDate.AddSeconds(3), "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                                new TestObject(draftIds[2], createdDate.AddSeconds(4), "mcarter", null, false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                                new TestObject(draftIds[3], createdDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" }),
                                new TestObject(draftIds[4], createdDate.AddSeconds(6), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate" }),
                                new TestObject(draftIds[4], createdDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" }),
                                new TestObject(draftIds[1], createdDate.AddSeconds(8), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Raspberries" })
                            };
                    break;
                case "SortedById":
                    output = new List<TestObject>()
                            {
                                new TestObject(draftIds[1], createdDate, "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" }),
                                new TestObject(draftIds[1], createdDate.AddSeconds(2), "mcarter", DateTime.Parse("2023-04-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" }),
                                new TestObject(draftIds[1], createdDate.AddSeconds(8), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Raspberries" }),
                                new TestObject(draftIds[1], createdDate.AddSeconds(1), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" }),
                                new TestObject(draftIds[2], createdDate.AddSeconds(3), "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                                new TestObject(draftIds[2], createdDate.AddSeconds(4), "mcarter", null, false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                                new TestObject(draftIds[3], createdDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" }),
                                new TestObject(draftIds[4], createdDate.AddSeconds(6), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate" }),
                                new TestObject(draftIds[4], createdDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" })
                            };
                    break;
                case "History":
                    output = new List<TestObject>()
                            {
                                new TestObject(draftIds[1], createdDate, "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" }),
                                new TestObject(draftIds[1], createdDate.AddSeconds(1), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" }),
                                new TestObject(draftIds[1], createdDate.AddSeconds(2), "mcarter", DateTime.Parse("2023-04-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" }),
                                new TestObject(draftIds[1], createdDate.AddSeconds(8), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Raspberries" }),
                                new TestObject(draftIds[2], createdDate.AddSeconds(3), "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                                new TestObject(draftIds[2], createdDate.AddSeconds(4), "mcarter", null, false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                                new TestObject(draftIds[3], createdDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" }),
                                new TestObject(draftIds[4], createdDate.AddSeconds(6), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate" }),
                                new TestObject(draftIds[4], createdDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" })
                            };
                    break;
                case "Latest":
                    output = new List<TestObject>()
                            {
                                new TestObject(draftIds[1], createdDate.AddSeconds(8), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Raspberries" }),
                                 new TestObject(draftIds[3], createdDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" }),
                                new TestObject(draftIds[4], createdDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" })
                            };
                    break;
                case "SyncTesting":
                    output = new List<TestObject>()
                        {
                            new TestObject(draftIds[1], createdDate, "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" }),
                            new TestObject(draftIds[1], createdDate.AddSeconds(1), "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" }),
                            new TestObject(draftIds[1], createdDate.AddSeconds(2), "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" }),
                            new TestObject(draftIds[2], createdDate.AddSeconds(3), "mcarter", null, true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                            new TestObject(draftIds[2], createdDate.AddSeconds(4), "mcarter", null, false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                            new TestObject(draftIds[3], createdDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" }),
                            new TestObject(draftIds[4], createdDate.AddSeconds(6), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate" }),
                            new TestObject(draftIds[4], createdDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" })
                        };
                    break;
                default: throw new NotImplementedException();

            }

            return output;

        }

    }
}
