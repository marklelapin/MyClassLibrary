using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods
{
    internal static class TestExtensions
    {
        internal static List<TestContent> GenerateTestContents(this List<TestContent> testContents, int quantity,string? contentType = null,List<Guid>? overrideIds = null,DateTime? created = null)
        {
                     
            for (int i = 0; i < quantity; i++)
            {
                testContents.Add(new TestContent(contentType,overrideIds,created));

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
