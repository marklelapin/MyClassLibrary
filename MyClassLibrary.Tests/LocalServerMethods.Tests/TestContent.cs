using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyClassLibrary.Extensions;
namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    public class TestContent
    {
        public List<TestObject> TestObjects { get; set; } = new List<TestObject>();
        public DateTime CreatedDate { get; private set; } 

        private List<Guid> GeneratDraftIds(int quantity)
        {
            return Guid.NewGuid().GenerateList(quantity);
        }

        public List<Guid> TestIds() {
            return TestObjects.Select(x => x.Id).Distinct().ToList();
        }

        public List<TestObject> ActiveTestObjects()
        {
            return TestObjects.Where(x=>x.IsActive == true).ToList();
        }

        public TestContent(int testSetupId=0)   
        {
            DateTime DateNow = DateTime.Now;
            CreatedDate = new DateTime(DateNow.Year, DateNow.Month, DateNow.Day, DateNow.Hour, DateNow.Minute, DateNow.Second, 0, DateTimeKind.Unspecified);
            List<Guid> draftIds = GeneratDraftIds(25);

            switch(testSetupId)
            {                
                case 0:
                        TestObjects = new List<TestObject>()
                        {
                            new TestObject(draftIds[1], CreatedDate, "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" }),
                            new TestObject(draftIds[1], CreatedDate.AddSeconds(1), "mcarter", DateTime.Parse("2023-03-02 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" }),
                            new TestObject(draftIds[1], CreatedDate.AddSeconds(2), "mcarter", DateTime.Parse("2023-04-01 09:02:00.000"), true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" }),
                            new TestObject(draftIds[2], CreatedDate.AddSeconds(3), "mcarter", DateTime.Parse("2023-03-01 09:02:00.000"), true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                            new TestObject(draftIds[2], CreatedDate.AddSeconds(4), "mcarter", null, false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                            new TestObject(draftIds[3], CreatedDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" }),
                            new TestObject(draftIds[4], CreatedDate.AddSeconds(6), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate" }),
                            new TestObject(draftIds[4], CreatedDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" })
                        };
                    break;
            }
            
        }
        
    }
}
