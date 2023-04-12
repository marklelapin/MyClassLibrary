using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Extensions
{
    public static class GuidExtensions
    {
        public static List<Guid> GenerateList(this Guid guid,int quantity)
        {
            List<Guid> output = new List<Guid>();
            for (int i = 0; i < quantity; i++)
            {
                output.Add(Guid.NewGuid());
            }

            return output;
            
        }
    }
}
