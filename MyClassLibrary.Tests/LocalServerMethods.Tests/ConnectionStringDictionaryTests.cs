using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    public class ConnectionStringDictionaryTests
    {
        [Fact]
        public void DictionaryTest ()
        {
            ConnectionStringDictionary _connectionStringDictionary = new ConnectionStringDictionary();

            string actual = _connectionStringDictionary.TestConnectionString;
            string expected = "Test Succeeded";
            Assert.Equal(expected,actual);

        }
            


            



    }
}
