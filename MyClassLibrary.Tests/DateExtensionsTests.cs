using MyExtensions;
using System.Runtime.CompilerServices;

namespace MyClassLibrary.Tests

{
    public class DateExtensionsTests
    {

        public static readonly object[][] TestData =
        {
            new object[] { new DateTime(2024,3,3),new DateTime(2023,3,3),"years",1},
            new object[] { new DateTime(2023,3,3),new DateTime(2023,3,3),"years",0},
            new object[] { new DateTime(2022,3,3),new DateTime(2023,3,3),"years",-1},
            new object[] { new DateTime(2022,4,3),new DateTime(2022,3,3),"years",0},
            new object[] { new DateTime(2025,3,3),new DateTime(2023,3,4),"years",1},
            new object[] { new DateTime(2025,3,3),new DateTime(2023,3,2),"years",2},

            new object[] { new DateTime(2023,5,3),new DateTime(2023,4,3),"months",1},
            new object[] { new DateTime(2023,4,3),new DateTime(2023,4,3),"months",0},
            new object[] { new DateTime(2023,3,3),new DateTime(2023,4,3),"months",-1},

            new object[] { new DateTime(2023,3,3),new DateTime(2023,3,2),"days",1},
            new object[] { new DateTime(2023,3,2),new DateTime(2023,3,2),"days",0},
            new object[] { new DateTime(2023,3,1),new DateTime(2023,3,2),"days",-1},

            new object[] { new DateTime(2023,4,3,5,0,0),new DateTime(2023,4,3,4,0,0),"hours",1},
            new object[] { new DateTime(2023,5,3,4,0,0),new DateTime(2023,4,3,4,0,0),"hours",0},
            new object[] { new DateTime(2023,3,3,3,0,0),new DateTime(2023,4,3,4,0,0),"hours",-1},

            new object[] { new DateTime(2023,4,3,0,5,0),new DateTime(2023,4,3,0,4,0),"minutes",1},
            new object[] { new DateTime(2023,5,3,0,4,0),new DateTime(2023,4,3,0,4,0),"minutes",0},
            new object[] { new DateTime(2023,3,3,0,3,0),new DateTime(2023,4,3,0,4,0),"minutes",-1},

            new object[] { new DateTime(2023,4,3,0,0,5),new DateTime(2023,4,3,0,0,4),"seconds",1},
            new object[] { new DateTime(2023,5,3,0,0,4),new DateTime(2023,4,3,0,0,4),"seconds",0},
            new object[] { new DateTime(2023,3,3,0,0,3),new DateTime(2023,4,3,0,0,4),"seconds",-1},
        };
        
        [Theory, MemberData(nameof(TestData))]
        public void DateDiffTest(DateTime date1,DateTime date2,string datePart,int expected)
        {
            int actual = date1.DateDiff(date2, datePart);

            Assert.Equal(expected, actual);
        }



    }    
}