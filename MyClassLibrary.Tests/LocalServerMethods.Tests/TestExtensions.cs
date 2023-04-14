using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    internal static class TestExtensions
    {
        internal static List<TestContent> GenerateTestContents(this List<TestContent> testContents, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                testContents.Add(new TestContent());

                CreationDelay(200);
            };

            return testContents;
        }


        async static private void CreationDelay(int milliSeconds)
        {
            await Task.Delay(milliSeconds);
        }
    }
}
