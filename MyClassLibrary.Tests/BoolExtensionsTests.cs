using MyExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace MyClassLibrary.Tests
{
    public class BoolExtensionsTests
    {
        public static readonly object[][] testData =
        {
            new object[] {"True",true },
            new object[] {"False",false},
            new object[] {"yes",true },
            new object[] {"no",false},
            new object[] {"Yes",true },
            new object[] {"No",false},
            new object[] {"YES",true },
            new object[] {"NO",false},
            new object[] {"1",true },
            new object[] {"0",false},
        };

        [Theory, MemberData(nameof(testData))]
        public void ToBoolTest(string str, bool? expected)
        {
            bool? actual = str.ToBool();

            Assert.Equal(expected, actual);
        }
           

    }
}
