using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MyClassLibrary.LocalServerMethods.Extensions
{
    internal static class StringExtensions
    {
        internal static List<T> ConvertSQLJsonUpdateToUpdate<T>(this string? jsonStr) where T : ILocalServerModelUpdate
        {
            List<T> result;

            if (jsonStr == null)
            {
                result = new List<T>();
            } else
            {
                List<SQLUpdateData<T>> listSQLUpdateData = JsonSerializer.Deserialize<List<SQLUpdateData<T>>>(jsonStr) ?? new List<SQLUpdateData<T>>();

                result = listSQLUpdateData.Select(x=>x.JsonUpdate).ToList();

            }
            
           return result;
           
        }
    }
}
