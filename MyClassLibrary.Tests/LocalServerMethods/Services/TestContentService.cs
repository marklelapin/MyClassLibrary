using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using MyClassLibrary.Extensions;
using MyClassLibrary.LocalServerMethods;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Services
{
    internal class TestContentService<T> : ITestContent<T> where T : LocalServerIdentityUpdate
    {

        public List<List<T>> Generate(int quantity, string testType = "Default", List<Guid>? overrideIds = null, DateTime? overrideCreated = null) 
        {
            List<List<T>> output = new List<List<T>>();
                        
            for (int i = 0; i < quantity; i++)
            {
                List<Guid> draftIds = overrideIds ?? GenerateDraftIds(20);
                draftIds.Sort((x, y) => x.CompareTo(y));
                output.Add(GenerateList(testType, draftIds,overrideCreated));

                CreationDelay(200); //ensures that each new lists don't have the same created datetime which they might if not left in.
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
                case "TestUpdate": return ConvertToListT<TestUpdate>(GenerateTestUpdateLists(testType, draftIds, created));
                   
                default: throw new NotImplementedException();
            };
        }

        private List<T> ConvertToListT<S>(List<S> listToConvert)
        {
            string json =   JsonConvert.SerializeObject(listToConvert);

            List<T> output = JsonConvert.DeserializeObject<List<T>>(json);

            return output; //JsonSerializer.Deserialize<List<T>>(JsonSerializer.Serialize<List<S>>(listToConvert)) ?? new List<T>();
        }

        private List<TestUpdate> GenerateTestUpdateLists(string testType, List<Guid> draftIds, DateTime createdDate)
        {
            List<TestUpdate> output;

            switch (testType)
            {
                case "Default":
                    output = new List<TestUpdate>()
                        {
                            new TestUpdate(draftIds[1], createdDate, "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" }),
                            new TestUpdate(draftIds[1], createdDate.AddSeconds(1), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" }),
                            new TestUpdate(draftIds[1], createdDate.AddSeconds(2), "mcarter", DateTime.Parse("2023-04-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" }),
                            new TestUpdate(draftIds[2], createdDate.AddSeconds(3), "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                            new TestUpdate(draftIds[2], createdDate.AddSeconds(4), "mcarter", null, false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                            new TestUpdate(draftIds[3], createdDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" }),
                            new TestUpdate(draftIds[4], createdDate.AddSeconds(6), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate" }),
                            new TestUpdate(draftIds[4], createdDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" })
                        };
                    break;

                case "Unsorted":
                    output = new List<TestUpdate>()
                            {
                                new TestUpdate(draftIds[1], createdDate, "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" }),
                                new TestUpdate(draftIds[1], createdDate.AddSeconds(2), "mcarter", DateTime.Parse("2023-04-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" }),
                                new TestUpdate(draftIds[2], createdDate.AddSeconds(3), "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                                new TestUpdate(draftIds[3], createdDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" }),
                                new TestUpdate(draftIds[4], createdDate.AddSeconds(6), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate" }),
                                new TestUpdate(draftIds[4], createdDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" }),
                                new TestUpdate(draftIds[2], createdDate.AddSeconds(4), "mcarter", null, false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                                new TestUpdate(draftIds[1], createdDate.AddSeconds(8), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Raspberries" }),
                                new TestUpdate(draftIds[1], createdDate.AddSeconds(1), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" })
                             };
                    break;
                case "SortedByCreated":
                    output = new List<TestUpdate>()
                            {
                                new TestUpdate(draftIds[1], createdDate, "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" }),
                                new TestUpdate(draftIds[1], createdDate.AddSeconds(1), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" }),
                                new TestUpdate(draftIds[1], createdDate.AddSeconds(2), "mcarter", DateTime.Parse("2023-04-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" }),
                                new TestUpdate(draftIds[2], createdDate.AddSeconds(3), "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                                new TestUpdate(draftIds[2], createdDate.AddSeconds(4), "mcarter", null, false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                                new TestUpdate(draftIds[3], createdDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" }),
                                new TestUpdate(draftIds[4], createdDate.AddSeconds(6), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate" }),
                                new TestUpdate(draftIds[4], createdDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" }),
                                new TestUpdate(draftIds[1], createdDate.AddSeconds(8), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Raspberries" })
                            };
                    break;
                case "SortedById":
                    output = new List<TestUpdate>()
                            {
                                new TestUpdate(draftIds[1], createdDate, "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" }),
                                new TestUpdate(draftIds[1], createdDate.AddSeconds(2), "mcarter", DateTime.Parse("2023-04-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" }),
                                new TestUpdate(draftIds[1], createdDate.AddSeconds(8), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Raspberries" }),
                                new TestUpdate(draftIds[1], createdDate.AddSeconds(1), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" }),
                                new TestUpdate(draftIds[2], createdDate.AddSeconds(3), "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                                new TestUpdate(draftIds[2], createdDate.AddSeconds(4), "mcarter", null, false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                                new TestUpdate(draftIds[3], createdDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" }),
                                new TestUpdate(draftIds[4], createdDate.AddSeconds(6), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate" }),
                                new TestUpdate(draftIds[4], createdDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" })
                            };
                    break;
                case "History":
                    output = new List<TestUpdate>()
                            {
                                new TestUpdate(draftIds[1], createdDate, "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" }),
                                new TestUpdate(draftIds[1], createdDate.AddSeconds(1), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" }),
                                new TestUpdate(draftIds[1], createdDate.AddSeconds(2), "mcarter", DateTime.Parse("2023-04-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" }),
                                new TestUpdate(draftIds[1], createdDate.AddSeconds(8), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Raspberries" }),
                                new TestUpdate(draftIds[2], createdDate.AddSeconds(3), "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                                new TestUpdate(draftIds[2], createdDate.AddSeconds(4), "mcarter", null, false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                                new TestUpdate(draftIds[3], createdDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" }),
                                new TestUpdate(draftIds[4], createdDate.AddSeconds(6), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate" }),
                                new TestUpdate(draftIds[4], createdDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" })
                            };
                    break;
                case "Latest":
                    output = new List<TestUpdate>()
                            {
                                new TestUpdate(draftIds[1], createdDate.AddSeconds(8), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Raspberries" }),
                                 new TestUpdate(draftIds[3], createdDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" }),
                                new TestUpdate(draftIds[4], createdDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" })
                            };
                    break;
                case "SyncTesting":
                    output = new List<TestUpdate>()
                        {
                            new TestUpdate(draftIds[1], createdDate, "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" }),
                            new TestUpdate(draftIds[1], createdDate.AddSeconds(1), "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" }),
                            new TestUpdate(draftIds[1], createdDate.AddSeconds(2), "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" }),
                            new TestUpdate(draftIds[2], createdDate.AddSeconds(3), "mcarter", null, true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                            new TestUpdate(draftIds[2], createdDate.AddSeconds(4), "mcarter", null, false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                            new TestUpdate(draftIds[3], createdDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" }),
                            new TestUpdate(draftIds[4], createdDate.AddSeconds(6), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate" }),
                            new TestUpdate(draftIds[4], createdDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" })
                        };
                    break;
                default: throw new NotImplementedException();
            }

            return output;

        }



        async static private void CreationDelay(int milliSeconds)
        {
            await Task.Delay(milliSeconds);
        }
    }
}
